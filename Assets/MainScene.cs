using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<FlexGroup>().Init();
        foreach(var flexGroup in GetComponentsInChildren<FlexGroup>())
        {
            flexGroup.Init();
        }
    }
}
