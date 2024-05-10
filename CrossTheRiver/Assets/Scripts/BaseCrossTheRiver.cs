using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class BaseCrossTheRiver : MonoBehaviour
{
    public GameObject[] cellButton = new GameObject[64]; 
    UserData user = new UserData();

    private SpriteRenderer spriteRenderer;
    
    //여기서 할일은
    //0. 조건 만들기(0단계부터 시작해서 맞추면 어떻게 올라가는 식)
    //1. 보드판 깔기
    //2. 눌린 보드판 데이터 가져오기
    //3. 맞는지 틀린지 확인하기
    //4. 한 게임 끝나면 조건에 맞게 스테이지 변경하기.

    //0번 초기화
    void Clear()
    { 
        user.currentLevel = 1;
        user.currentNumber = 1;
    }

    void TestBoard() //지금은 지정된 데이터로 보드판 깔기
    {
        CrossTheRiverCellData cellData = new CrossTheRiverCellData();

       

        // 셀 버튼 위치 번호 가져오기
        for (int i = 0; i < cellButton.Length; i++)
        {
            String cell = CTRResources.Instance.crossTheRiverBoardData[0].cells[i];
            cellData.SplitCellData(cell); //셀데이터 나눴고

            if (int.Parse(cellButton[i].name) == cellData.cellPositionNumber)
            {
              
                var value = string.Format("CR_{0}_{1}_{2}",
                   cellData.color.ToString(), cellData.shape, cellData.count.ToString());

               //addressable manager 사용해서 가져오기
            }


        }

        
       
    }

    //1.보드판 깔기
    void SetBoard(int level, int number) //처음에는 0 -> 1 올리고 1내리고 이렇게 한단말이지
    {
        //CTRResources.Instance.crossTheRiverData
        //원하는거 : json데이터 파싱하기
        //1. 현재 레벨에 맞게 json데이터에서 해당 보드판 데이터 가져오기
        for (int i = 1; i <= user.MaxLevel; i++) {
            for (int j = 1; j <= user.MaxNumber; j++)
            {
                if (i == level && j == number)
                {
                    
                }
            }

        }




    }
}
