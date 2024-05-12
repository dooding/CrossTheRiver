using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUseData : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
       string jsonString = CTRResources.Instance.crossTheRiverBoardData.ToString();

        //Debug.Log(CTRResources.Instance.crossTheRiverBoardData[0].cells[0]);



    }
}
