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

    private int clickMaxNumber = 50; //게임 전체 클릭 횟수
    private int clickCount = 0; //사용자 클릭 횟수
    private int testcellanswer = 0;

    //필요한 변수 정리
    //타이머
    //게임이 끝난 후 정답인지 오답인지
    //연속 정답인지 연속 오답인지 확인용 변수
    private int playTimes = 0;//총 게임 횟수
    private int currentLevel = 0;//현재 진행 중인 레벨
    private int currentNumber = 0; //현재 진행 중인 넘버
    List<String> correctCellList = new List<string>();//정답 cell 데이터 넣는 리스트 변수
    //게임 내 전체 레벨(필요한가?)
    //게임 내 전체 넘버(필요한가?)
    


    //여기서 할일은
    //0. 조건 만들기(0단계부터 시작해서 맞추면 어떻게 올라가는 식)
    //1. 보드판 깔기
    //2. 눌린 보드판 데이터 가져오기
    //3. 맞는지 틀린지 확인하기
    //4. 한 게임 끝나면 조건에 맞게 스테이지 변경하기.

    //0번 초기화
    void Clear()
    { 
        currentLevel = 1;
        currentNumber = 1;

        clickCount = 0;
    }

    private void Start()
    {
        Clear();
        TestSetBoard();
    }

    void TestSetBoard() //지금은 지정된 데이터로 보드판 깔기 (테스트로 지정함)
    {
        CrossTheRiverCellData cellData = new CrossTheRiverCellData();
        //Addressables.Release(Handle);  

        // 셀 버튼 위치 번호 가져오기
        for (int i = 0; i < cellButton.Length; i++)
        {
            Image image = cellButton[i].GetComponent<Image>();
            String cell = CTRResources.Instance.crossTheRiverBoardData[2].cells[i]; //***2 대신 현재 레벨로 수정***

            cellData.SplitCellData(cell); //셀데이터 나눴고

            if (int.Parse(cellButton[i].name) == cellData.cellPositionNumber)
            {
              
                var value = string.Format("CR_{0}_{1}_{2}",
                   cellData.color.ToString(), cellData.shape.ToString(), cellData.count.ToString());
                //Debug.Log(value);
                //addressable manager 사용해서 가져오기
                Addressables.LoadAssetAsync<Sprite>(value).Completed += (AsyncOperationHandle<Sprite> obj) =>
                {
                    image.sprite = obj.Result;
                };
            }
            //정답 카운트 가져오기
            correctCount = CTRResources.Instance.crossTheRiverData[2].Correct.Length; //정답 위치 셀의 전체 갯수. //***2 대신 현재 레벨로 수정***
            //정답 cell 위치 번호 넣기
            foreach (int number in CTRResources.Instance.crossTheRiverData[2].Correct)
            {
                correctCellList.Add(number.ToString());
            }

        }

        
       
    }

    //1.보드판 깔기
    void SetBoard(int level, int number) //처음에는 0 -> 1 올리고 1내리고 이렇게 한단말이지
    {
        //CTRResources.Instance.crossTheRiverData
        //원하는거 : json데이터 파싱하기
        //1. 현재 레벨에 맞게 json데이터에서 해당 보드판 데이터 가져오기
        /*for (int i = 1; i <= user.MaxLevel; i++) {
            for (int j = 1; j <= user.MaxNumber; j++)
            {
                if (i == level && j == number)
                {
                    
                }
            }

        }
        */

    }



    public void OnClickCell() //현재 누른걸 가져오기 (여기서 정답 오답을 구현?) //맞게 누를떄만 해야하므로
    {
        testcellanswer += 1; //***테스트용***

        clickCount += 1;
        Image btnImage;
        GameObject currentClickBtn = EventSystem.current.currentSelectedGameObject;
        
        if (testcellanswer < 5) //***정답 맞추면***으로 변경예정 ***if (currentClickBtn.name == correctCellList[clickedButtonList.Count])
        {
            btnImage = currentClickBtn.GetComponent<Image>(); // 해당 오브젝트의 Image 컴포넌트를 받음
            btnImage.color = new Color(0.3f, 0.3f, 0.3f); // 해당 이미지의 색상 변경

            currentClickBtn.transform.GetChild(0).gameObject.SetActive(true); //자식 오브젝트도 클릭. 

            clickedButtonList.Add(currentClickBtn);

            //한번 눌린 버튼은 못 누르게 해야함.
            currentClickBtn.GetComponent<Button>().interactable = false;
            ClickCount();
        }
        else //정답 틀리면
        {
            foreach (GameObject i in clickedButtonList)
            {
                i.GetComponent<Image>().color = Color.white;
                i.transform.GetChild(0).gameObject.SetActive(false);
                i.GetComponent<Button>().interactable = true;

            }
            testcellanswer = 0; //***테스트용***
            ClickCount();
        }
        //추후, 50번 횟수 제한, 타이머, 주변 안눌리게 구현
        //PlayGameManager();

       

       
    }

    void ClickCount() //정답 카운트 비교
    {

        if (clickCount < clickMaxNumber) //50번 이상이면 게임 오버
        {
            LevelChange(); //50번 넘겼으니 레벨 변경
            return;
        }

        else { //50번 이상이 아니면

            if (clickedButtonList.Count == correctCount) //정답을 다 맞춘 경우(정답이 들어간 버튼의 갯수와 셀 정답 갯수가 일치하면)
            { //정답을 모두 맞춘 상태이므로 해당 조건 추가
                LevelChange();

                return;
            }
        }

        void LevelChange()
        {
            if (playTimes == 5) //level 변화 없이 5번 반복 gameover.(한 level에서 성공 및 실패를 
            {
                GameOver();
                return;
            }

            else
            {
                //시행 3번 연속 정답 level up
                //시행 2번 연속 오답 level down
                
            }

            //1, 게임 시작하면 level 1, stage 1로 시작함. (그걸로 판 세팅)
            //2. 게임 한판 하면 승패가 결정되고 그걸 결과값에 넣음.(clickcount가 50이 되면 정해짐)
            //3. 총 횟수 로직 구현 해야함. 
        }

        void GameOver()
        {
            
        }
    }
}
