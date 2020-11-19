using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FlexGroup : MonoBehaviour
{
    [SerializeField]
    public FlexDirection _flexDirection;
    public FlexDirection FlexDirection => _flexDirection;

    [SerializeField]
    public AlignmentType _itemAlignment;
    public AlignmentType ItemAlignment => _itemAlignment;

    [SerializeField]
    private UnitType _paddingHorizontalType;
    public UnitType PaddingHorizontalType => _paddingHorizontalType;

    [SerializeField]
    [Range(0.0f, 0.5f)]
    private float _paddingHorizontalPercent;
    private float PaddingHorizontalPercent => _paddingHorizontalPercent;

    [SerializeField]
    [Min(0)]
    private int _paddingHorizontalUnit;
    private int PaddingHorizontalUnit => _paddingHorizontalUnit;

    [SerializeField]
    private UnitType _paddingVerticalType;
    public UnitType PaddingVerticalType => _paddingVerticalType;

    [SerializeField]
    [Range(0.0f, 0.5f)]
    private float _paddingVerticalPercent;
    private float PaddingVerticalPercent => _paddingVerticalPercent;

    [SerializeField]
    [Min(0)]
    private int _paddingVerticalUnit;
    private int PaddingVerticalUnit => _paddingVerticalUnit;


    private float PaddingHorizontal
    {
        get
        {
            return PaddingHorizontalType == UnitType.percent ? PaddingHorizontalPercent : PaddingHorizontalUnit / rectTransform.rect.width;
        }
    }
    private float PaddingVertical
    {
        get
        {
            return PaddingVerticalType == UnitType.percent ? PaddingVerticalPercent : PaddingVerticalUnit / rectTransform.rect.height;
        }
    }

    private RectTransform rectTransform;

    private void OnValidate()
    {
        Init();
    }

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        var flexElements = new List<FlexElement>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var flexElement = child.GetComponent<FlexElement>();
            if (flexElement != null)
                flexElements.Add(flexElement);
            else
                Debug.LogError($"FlexGroup child {i}:{child.name} missing FlexElement");
        }

        var flexAmount = flexElements.Sum(element => element.Flex);

        if (FlexDirection == FlexDirection.row)
            SetupRow(flexElements, flexAmount);
        else
            SetupColumn(flexElements, flexAmount);

    }

    private void SetupRow(List<FlexElement> flexElements, int flexAmount)
    {
        var width = rectTransform.rect.width;
        var height = rectTransform.rect.height;
        var minWidthUsed = width * 2 * PaddingHorizontal;
        var ignoredFlexUnits = 0;
        var orderedElements = flexElements.OrderByDescending(e => e.Flex > 0 ? e.MinWidth(width)/e.Flex : e.MinWidth(width));
        foreach(var element in orderedElements)
        {
            if (element.Flex == 0 ||
                (width - minWidthUsed)/(flexAmount - ignoredFlexUnits) * element.Flex <= element.MinWidth(width)
            )
            {
                minWidthUsed += element.MinWidth(width);
                ignoredFlexUnits += element.Flex;
            }
        }

        var xMin = PaddingHorizontal;
        var correctedFlexAmount = flexAmount - ignoredFlexUnits;

        for(int i = 0; i < flexElements.Count; i++)
        {
            var elementRectTransform = flexElements[i].GetComponent<RectTransform>();

            var xMax = xMin;
            var posY = GetYPos(flexElements[i].MinHeight(height));

            if (correctedFlexAmount == 0 ||
                flexElements[i].Flex == 0 ||
                (width - minWidthUsed)/(flexAmount - ignoredFlexUnits) * flexElements[i].Flex  < flexElements[i].MinWidth(width)
            )
            {
                xMax += WidthToPercent(flexElements[i].MinWidth(width));
            }
            else
            {
                xMax += (1 - WidthToPercent(minWidthUsed))/correctedFlexAmount * flexElements[i].Flex;
            }

            var posMin = new Vector2(xMin, posY[0]);
            var posMax = new Vector2(xMax, posY[1]);
            xMin = xMax;

            elementRectTransform.anchorMin = posMin;
            elementRectTransform.anchorMax = posMax;

            elementRectTransform.offsetMax = Vector2.zero;
            elementRectTransform.offsetMin = Vector2.zero;
        }
    }

    private Vector2 GetYPos(float minHeight)
    {
        var height = rectTransform.rect.height;

        switch (ItemAlignment)
        {
            case AlignmentType.start:
                return new Vector2(
                    minHeight / height > 1 - 2 * PaddingVertical ? (1 - PaddingVertical) - minHeight / height : PaddingVertical,
                    1 - PaddingVertical
                );
            case AlignmentType.center:
                return new Vector2(
                    minHeight / height > 1 - 2 * PaddingVertical ? 0.5f - (minHeight / height)/2 : PaddingVertical,
                    minHeight / height > 1 - 2 * PaddingVertical ? 0.5f + (minHeight / height)/2 : 1 - PaddingVertical
                );
            case AlignmentType.end:
                return new Vector2(
                    PaddingVertical,
                    minHeight / height > 1 - 2 * PaddingVertical ? PaddingVertical + minHeight / height : 1 - PaddingVertical
                );
            default:
                throw new System.Exception("Unidentified alignment");

        }
    }

    private void SetupColumn(List<FlexElement> flexElements, int flexAmount)
    {
        var width = rectTransform.rect.width;
        var height = rectTransform.rect.height;
        var minHeightUsed = height * 2 * PaddingVertical;
        var ignoredFlexUnits = 0;
        var orderedElements = flexElements.OrderByDescending(e => e.Flex > 0 ? e.MinHeight (height)/ e.Flex : e.MinHeight(height));
        foreach(var element in orderedElements)
        {
            if (element.Flex == 0 ||
                ((height - minHeightUsed)/(flexAmount - ignoredFlexUnits)) * element.Flex  <= element.MinHeight(height)
            )
            {
                minHeightUsed += element.MinHeight(height);
                ignoredFlexUnits += element.Flex;
            }
        }

        var yMax = 1 - PaddingVertical;
        var correctedFlexAmount = flexAmount - ignoredFlexUnits;

        for(int i = 0; i < flexElements.Count; i++)
        {
            var elementRectTransform = flexElements[i].GetComponent<RectTransform>();

            var xPos = GetXPos(flexElements[i].MinWidth(width));
            var xMin = PaddingHorizontal;
            var xMax = flexElements[i].MinWidth(width) / width > 1 - 2 * PaddingHorizontal  ? xMin + flexElements[i].MinWidth(width) / width : 1 - PaddingHorizontal;
            var yMin = yMax;

            if (correctedFlexAmount == 0 ||
                flexElements[i].Flex == 0 ||
                (height - minHeightUsed)/(flexAmount - ignoredFlexUnits) * flexElements[i].Flex < flexElements[i].MinHeight(height)
            )
            {
                yMin -= HeightToPercent(flexElements[i].MinHeight(height));
            }
            else
            {
                yMin -= (1 - HeightToPercent(minHeightUsed))/correctedFlexAmount * flexElements[i].Flex;
            }

            var posMin = new Vector2(xPos[0], yMin);
            var posMax = new Vector2(xPos[1], yMax);
            yMax = yMin;

            elementRectTransform.anchorMin = posMin;
            elementRectTransform.anchorMax = posMax;

            elementRectTransform.offsetMax = Vector2.zero;
            elementRectTransform.offsetMin = Vector2.zero;
        }
    }

    private Vector2 GetXPos(float minWidth)
    {
        var width = rectTransform.rect.width;

        switch (ItemAlignment)
        {
            case AlignmentType.start:
                return new Vector2(
                    PaddingHorizontal,
                    minWidth / width > 1 - 2 * PaddingHorizontal ? PaddingHorizontal + minWidth / width : 1 - PaddingHorizontal
                );
            case AlignmentType.center:
                return new Vector2(
                    minWidth / width > 1 - 2 * PaddingHorizontal ? 0.5f - (minWidth / width) / 2 : PaddingHorizontal,
                    minWidth / width > 1 - 2 * PaddingHorizontal ? 0.5f + (minWidth / width) / 2 : 1 - PaddingHorizontal
                );
            case AlignmentType.end:
                return new Vector2(
                    minWidth / width > 1 - 2 * PaddingHorizontal ? (1 - PaddingHorizontal) - minWidth / width : PaddingHorizontal,
                    1 - PaddingHorizontal
                );
            default:
                throw new System.Exception("Unidentified alignment");

        }
    }

    private float WidthToPercent(float w)
    {
        return w / rectTransform.rect.width;
    }

    private float HeightToPercent(float h)
    {
        return h / rectTransform.rect.height;
    }
}