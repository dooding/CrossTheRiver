using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class BaseCrossTheRiver : MonoBehaviour
{
    public GameObject[] cellButton; //판깔기 위해 필요한 버튼 변수
    List<GameObject> clickedButtonList = new List<GameObject> (); //누른 버튼 담을 변수


    //변수부
    private int correctCount = 0;

    private int clickMaxNumber; //게임 전체 클릭 횟수
    private int clickCount; //사용자 클릭 횟수
    private int answercount = 0;

    //필요한 변수 정리
    //타이머
    private int numSuccess = 0;//연속 정답인지 연속 오답인지 확인용 변수
    private int numFail = 0;//연속 정답인지 연속 오답인지 확인용 변수
    private int sameLevelplayTimes = 0;//같은 레벨 내에서 총 게임 횟수
    private int currentNumber = 0; //현재 진행 중인 넘버
    List<String> correctCellList = new List<string>();//정답 cell 데이터 넣는 리스트 변수
    private int exclickButton = 0; //이전에 누른 버튼 데이터 저장변수



    //여기서 할일은
    //0. 조건 만들기(0단계부터 시작해서 맞추면 어떻게 올라가는 식)
    //1. 보드판 깔기
    //2. 눌린 보드판 데이터 가져오기
    //3. 맞는지 틀린지 확인하기
    //4. 한 게임 끝나면 조건에 맞게 스테이지 변경하기.

    void Clear()
    {
        clickMaxNumber = 50;
        clickCount = 0;
        answercount = 0;
        correctCellList.Clear();
        correctCount = 0;
        exclickButton = 0;

        foreach (GameObject i in clickedButtonList)
        {
            i.GetComponent<Image>().color = Color.white;
            i.transform.GetChild(0).gameObject.SetActive(false);
            i.GetComponent<Button>().interactable = true;
        }
    }

    private void Start()
    {
        Clear();
        SetBoard();
    }

    void SetBoard() //지금은 지정된 데이터로 보드판 깔기 (테스트로 지정함)
    {
        Clear();
        CrossTheRiverCellData cellData = new CrossTheRiverCellData(); 

        // 셀 버튼 위치 번호 가져오기
        for (int i = 0; i < cellButton.Length; i++)
        {
            Image image = cellButton[i].GetComponent<Image>();
            int BoardNumber =  CTRResources.Instance.crossTheRiverData[currentNumber].BoardNumber;
            String cell = CTRResources.Instance.crossTheRiverBoardData[BoardNumber-1].cells[i]; //현재 넘버에 해당하는 셀 가져오기(-1은 배열이라서)

            cellData.SplitCellData(cell); //셀데이터 나눴고

            if (int.Parse(cellButton[i].name) == cellData.cellPositionNumber)
            {
              
                var value = string.Format("CR_{0}_{1}_{2}",
                   cellData.color.ToString(), cellData.shape.ToString(), cellData.count.ToString());

                //addressable manager 사용해서 가져오기
                Addressables.LoadAssetAsync<Sprite>(value).Completed += (AsyncOperationHandle<Sprite> obj) =>
                {
                    image.sprite = obj.Result;
                };
            }
            //정답 카운트 가져오기
            correctCount = CTRResources.Instance.crossTheRiverData[currentNumber].Correct.Length; //정답 위치 셀의 전체 갯수.
            //정답 cell 위치 번호 넣기
            foreach (int number in CTRResources.Instance.crossTheRiverData[currentNumber].Correct)
            {
                correctCellList.Add(number.ToString());
            }

        }
        //테스트코드
        Debug.Log("----지금 플레이 중----");
        Debug.Log("Level : " + CTRResources.Instance.crossTheRiverData[currentNumber].Level);
        Debug.Log("Number : " + CTRResources.Instance.crossTheRiverData[currentNumber].Number);
        Debug.Log("BoardNumber : " + CTRResources.Instance.crossTheRiverData[currentNumber].BoardNumber);
    }

    public bool Ispush(int btnNumber)
    {
        if (answercount == 0) //한번도 안누른 상태, 즉 처음 상태라면
        {
            if (57 <= btnNumber && btnNumber <= 64) //가장 하단 부분
            { 
                exclickButton = btnNumber;
                return true;
            }
        }

        else //그게 아니라면
        {
            int gap = Mathf.Abs(btnNumber - exclickButton);

            if (gap == 8 || gap == 1)
            {
                exclickButton = btnNumber;
                return true;
            }
        }
        return false;
    }


    public void OnClickCell() //현재 누른걸 가져오기 (여기서 정답 오답을 구현?) //맞게 누를떄만 해야하므로
    {       
        Image btnImage;
        GameObject currentClickBtn = EventSystem.current.currentSelectedGameObject;

        if (!Ispush(int.Parse(currentClickBtn.name))) //누를 수 없는거
            return;
        

        if (correctCellList[answercount] == currentClickBtn.name) //***정답 맞추면***으로 변경예정 ***if (currentClickBtn.name == correctCellList[clickedButtonList.Count])
        {
            btnImage = currentClickBtn.GetComponent<Image>(); // 해당 오브젝트의 Image 컴포넌트를 받음
            btnImage.color = new Color(0.3f, 0.3f, 0.3f); // 해당 이미지의 색상 변경

            currentClickBtn.transform.GetChild(0).gameObject.SetActive(true); //자식 오브젝트도 클릭. 

            clickedButtonList.Add(currentClickBtn);

            //한번 눌린 버튼은 못 누르게 해야함.
            currentClickBtn.GetComponent<Button>().interactable = false;
           // ClickCount();
            exclickButton = int.Parse(currentClickBtn.name);
            answercount += 1;
        }
        else //정답 틀리면
        {
            foreach (GameObject i in clickedButtonList)
            {
                i.GetComponent<Image>().color = Color.white;
                i.transform.GetChild(0).gameObject.SetActive(false);
                i.GetComponent<Button>().interactable = true;
            }
            clickedButtonList.Clear();
            answercount = 0;
            
           // ClickCount();
        }

        //추후, 50번 횟수 제한, 타이머, 주변 안눌리게 구현
        clickCount += 1;
        ClickCount();


    }

    void ClickCount() //정답 카운트 비교
    {

        if (clickCount > clickMaxNumber) //50번 이상이면 게임 오버
        {
            //테스트코드
            if (clickCount % 10 == 0)
                Debug.Log(clickCount);

            LevelCheck(false); //50번 넘겼으니 레벨 변경
            return;
        }

        else { //50번 이상이 아니면

            if (clickedButtonList.Count == correctCount) //정답을 다 맞춘 경우(정답이 들어간 버튼의 갯수와 셀 정답 갯수가 일치하면)
            { //정답을 모두 맞춘 상태이므로 해당 조건 추가
                Debug.Log("정답입니다.");
                LevelCheck(true);
                return;
            }
        }

    }


    void LevelCheck(bool isSuccess)
    {
        /*
         * 1.3번 연속 정답일 떄 level up
         * 2. 2번 연속 오답일 때 level down
         * 3. level 변화 없이 정답과 오답이 5번 반복되면 gameover.
         */


        if (isSuccess) //정답을 맞춘 경우
        {
            numSuccess += 1;
            //1.3번 연속 정답인가?
            if (numSuccess == 3)
            {
                sameLevelplayTimes = 0;
                LevelUp();
            }
            else
            {
                //3번 연속은 아닌 경우 f s / f ss / 그리고 다음 넘버로
                sameLevelplayTimes += 1;
                NextStage(); //다음 판으로
            }
        }

        else //오답인 경우
        {
            numFail += 1;
            //2. 2번 연속 오답인가?
            if (numFail == 2)
            {
                sameLevelplayTimes = 0;
                LevelDown();
            }

            else 
            {
                sameLevelplayTimes += 1;
                NextStage(); //다음 판으로
            }
        }

        if (sameLevelplayTimes == 5) //level 변화 없이 5번 반복 gameover.
        {
            GameOver();
            return;
        }

        SetBoard();

        //1, 게임 시작하면 level 1, stage 1로 시작함. (그걸로 판 세팅)
        //2. 게임 한판 하면 승패가 결정되고 그걸 결과값에 넣음.(clickcount가 50이 되면 정해짐)
        //3. 총 횟수 로직 구현 해야함. 
    }

    void LevelUp()
    {
        if (83 < currentNumber) //현재 레벨이 5라면
        {
            GameOver();
            return;
        }
        
        currentNumber += 21;
        Debug.Log("레벨업");

    }

    void LevelDown()
    {
        if (currentNumber < 21) //현재 레벨이 5라면
        {
            GameOver();
            return;
        }

        currentNumber -= 21;
        Debug.Log("레벨다운");
    }

    void NextStage()
    {
        if ((currentNumber + 1) % 21 == 0) //나머지가 없단 뜻은 해당 레벨의 끝에 왔단 뜻.
        {
            currentNumber -= 20;
        }
        else
        {
            currentNumber += 1;
        }
        Debug.Log("레벨 변화 없이 다음 판");

    }

    void GameOver()
    {
        Debug.Log("게임오버");
    }
}
