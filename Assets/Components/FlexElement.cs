using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexElement : MonoBehaviour
{

    [SerializeField]
    [Min(0)]
    private int _flex;
    public int Flex => _flex;

    [SerializeField]
    private UnitType _minWidthType;
    public UnitType MinWidthType => _minWidthType;

    [SerializeField]
    [Min(0)]
    private int _minWidthUnit;
    private int MinWidthUnit => _minWidthUnit;

    [SerializeField]
    [Range(0, 1)]
    private float _minWidthPercent;
    private float MinWidthPercent => _minWidthPercent;

    [SerializeField]
    private UnitType _minHeightType;
    private UnitType MinHeightType => _minHeightType;

    [SerializeField]
    [Min(0)]
    private int _minHeightUnit;
    public int MinHeightUnit => _minHeightUnit;

    [SerializeField]
    [Range(0, 1)]
    private float _minHeightPercent;
    public float MinHeightPercent => _minHeightPercent;

    private void OnValidate()
    {
        transform.parent.GetComponent<FlexGroup>().Init();
    }

    public float MinWidth(float parentWidth)
    {
        if (MinWidthType == UnitType.percent)
            return parentWidth * MinWidthPercent;
        else
            return MinWidthUnit;
    }
}
