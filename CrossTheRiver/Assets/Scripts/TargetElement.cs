using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetElement : MonoBehaviour
{
    public Button button;
    public Image image;
    public Image childImage;
    

    public bool CheckCorrect()
    {

        Debug.Log("버튼 눌림");
        var isCheck = false;
        //todo
        //지금 눌린 데이터가 ruledata에 적합한 상태라면 눌린 표시로 바꾸기.

        

        return isCheck;
    }
}
