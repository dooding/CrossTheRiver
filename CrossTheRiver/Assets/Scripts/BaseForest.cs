using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseForest : MonoBehaviour
{
    //텍스트
    public Text scriptTextA;
    public Text scriptTextB;
    public Button nextBtn;
    public GameObject character;

    public GameObject panelA;
    public GameObject panelB;

    private List<string> forestScript = new List<string>();
    private int scriptIndex;


    //테스트
    private bool check;


    void Clear()
    {
        scriptTextA.gameObject.SetActive(true);
        scriptTextB.gameObject.SetActive(true);
        forestScript.Clear(); 
        scriptIndex = 0;
        panelA.SetActive(true);
        panelB.SetActive(false);

        check = false;


    }

    // Start is called before the first frame update
    void Start()
    {

        Clear();
        for (int i = 0; i < CTRResources.Instance.forestAudioScriptData.Length; i++)
        {
            forestScript.Add(CTRResources.Instance.forestAudioScriptData[i].value);
        }
        Debug.Log(forestScript[scriptIndex]);
        scriptTextA.text = forestScript[scriptIndex];




    }

    public void clickNextBtn() //버튼이 나타난 시점에선 계속 이걸로 하다가
    {
        ++scriptIndex;
        check = true;

        if (scriptIndex == 2)
        {
            breathing();
        }
       
        if (scriptIndex == 9)
        {
            stretching();
        }

        else
        {
            scriptTextA.text = forestScript[scriptIndex];
            scriptTextB.text = forestScript[scriptIndex];
        }

    }

    void onCamera()
    {
        
    }

    void breathing()
    {
        //애니메이션 실행
        Debug.Log("숨쉬기 실행");

        //애니메이션이 끝나면
        StartCoroutine(WaitBreathing());
    }

    void stretching()
    {
        //스트레칭 관련 관리하기
        Debug.Log("스트레칭 실행");

        //여기서 할일
        //1. 캐릭터 및 패널 이동
        panelA.SetActive(false);
        panelB.SetActive(true);

        //2. 카메라 켜기
        StartCoroutine(WaitStretching());
    }

    /*
    IEnumerator WaitState()
    {
        bool btnActive = CTRResources.Instance.forestAudioScriptData[scriptIndex].btnActive;
       
        nextBtn.gameObject.SetActive(btnActive);

        if (!btnActive)
        {
            scriptTextA.text = forestScript[scriptIndex];
            scriptTextB.text = forestScript[scriptIndex];
            scriptIndex++;
            yield return new WaitForSeconds(3.0f);
        }
    }
    */
    

    IEnumerator WaitBreathing()
    {
        //숨쉬기 + 애니메이션 처리하고 완료되면 버튼 setactive ture;
        nextBtn.gameObject.SetActive(false);

        yield return new WaitForSeconds(2.0f);
        scriptTextA.text = forestScript[scriptIndex++];

        yield return new WaitForSeconds(2.0f);
        scriptTextA.text = forestScript[scriptIndex++];

        yield return new WaitForSeconds(2.0f);
        scriptTextA.text = forestScript[scriptIndex++];

        yield return new WaitForSeconds(2.0f);
        Debug.Log("눈 뜬 이미지 변경");
        scriptTextA.text = forestScript[scriptIndex];

        nextBtn.gameObject.SetActive(true);
        yield break;

    }

    IEnumerator WaitStretching()
    {
        bool btnActive = CTRResources.Instance.forestAudioScriptData[scriptIndex].btnActive;

        nextBtn.gameObject.SetActive(btnActive);

        while (scriptIndex <= 27)
        {
            if (!btnActive)
            {
                scriptTextB.text = forestScript[scriptIndex++];
                yield return new WaitForSeconds(3.0f);
            } 
        }

    }





   
}
