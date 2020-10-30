using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FlexGroup : MonoBehaviour
{
    public enum FlexDirectionOption
    {
        row = 1,
        column = 2
    }

    [SerializeField]
    public FlexDirectionOption _flexDirection;
    public FlexDirectionOption FlexDirection => _flexDirection;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _paddingHorizontal;
    private float PaddingHorizontal => _paddingHorizontal;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _paddingVertical;
    private float PaddingVertical => _paddingVertical;

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

        if (FlexDirection == FlexDirectionOption.row)
            SetupRow(flexElements, flexAmount);
        else
            SetupColumn(flexElements, flexAmount);

    }

    private void SetupRow(List<FlexElement> flexElements, int flexAmount)
    {
        var width = rectTransform.rect.width;
        var minWidthUsed = width * 2 * PaddingHorizontal;
        var ignoredFlexUnits = 0;
        var orderedElements = flexElements.OrderByDescending(e => e.Flex > 0 ? e.MinWidth/e.Flex : e.MinWidth);
        foreach(var element in orderedElements)
        {
            if (element.Flex == 0 ||
                (width - minWidthUsed)/(flexAmount - ignoredFlexUnits) * element.Flex <= element.MinWidth
            )
            {
                minWidthUsed += element.MinWidth;
                ignoredFlexUnits += element.Flex;
            }
        }

        var xMin = PaddingHorizontal;
        var correctedFlexAmount = flexAmount - ignoredFlexUnits;

        for(int i = 0; i < flexElements.Count; i++)
        {
            var elementRectTransform = flexElements[i].GetComponent<RectTransform>();

            var xMax = xMin;
            var yMin = PaddingVertical;
            var yMax = 1 - PaddingVertical;

            if (correctedFlexAmount == 0 ||
                flexElements[i].Flex == 0 ||
                (width - minWidthUsed)/(flexAmount - ignoredFlexUnits) * flexElements[i].Flex  < flexElements[i].MinWidth
            )
            {
                xMax += WidthToPercent(flexElements[i].MinWidth);
            }
            else
            {
                xMax += (1 - WidthToPercent(minWidthUsed))/correctedFlexAmount * flexElements[i].Flex;
            }

            var posMin = new Vector2(xMin, yMin);
            var posMax = new Vector2(xMax, yMax);
            xMin = xMax;

            elementRectTransform.anchorMin = posMin;
            elementRectTransform.anchorMax = posMax;

            elementRectTransform.offsetMax = Vector2.zero;
            elementRectTransform.offsetMin = Vector2.zero;
        }
    }

    private void SetupColumn(List<FlexElement> flexElements, int flexAmount)
    {
        var height = rectTransform.rect.height;
        var minHeightUsed = height * 2 * PaddingVertical;
        var ignoredFlexUnits = 0;
        var orderedElements = flexElements.OrderByDescending(e => e.Flex > 0 ? e.MinHeight/e.Flex : e.MinHeight);
        foreach(var element in orderedElements)
        {
            if (element.Flex == 0 ||
                ((height - minHeightUsed)/(flexAmount - ignoredFlexUnits)) * element.Flex  <= element.MinHeight
            )
            {
                minHeightUsed += element.MinHeight;
                ignoredFlexUnits += element.Flex;
            }
        }

        var yMax = 1 - PaddingVertical;
        var correctedFlexAmount = flexAmount - ignoredFlexUnits;

        for(int i = 0; i < flexElements.Count; i++)
        {
            var elementRectTransform = flexElements[i].GetComponent<RectTransform>();

            var xMin = PaddingHorizontal;
            var xMax = 1 - PaddingHorizontal;
            var yMin = yMax;

            if (correctedFlexAmount == 0 ||
                flexElements[i].Flex == 0 ||
                (height - minHeightUsed)/(flexAmount - ignoredFlexUnits) * flexElements[i].Flex < flexElements[i].MinHeight
            )
            {
                yMin -= HeightToPercent(flexElements[i].MinHeight);
            }
            else
            {
                yMin -= (1 - HeightToPercent(minHeightUsed))/correctedFlexAmount * flexElements[i].Flex;
            }

            var posMin = new Vector2(xMin, yMin);
            var posMax = new Vector2(xMax, yMax);
            yMax = yMin;

            elementRectTransform.anchorMin = posMin;
            elementRectTransform.anchorMax = posMax;

            elementRectTransform.offsetMax = Vector2.zero;
            elementRectTransform.offsetMin = Vector2.zero;
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