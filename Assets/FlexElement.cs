using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexElement : MonoBehaviour
{
    [SerializeField]
    private int _flex;
    public int Flex => _flex;

    [SerializeField]
    private int _minWidth;
    public int MinWidth => _minWidth;

    [SerializeField]
    private int _minHeight;
    public int MinHeight => _minHeight;


    private void OnValidate()
    {
        transform.parent.GetComponent<FlexGroup>().Init();
    }
}
