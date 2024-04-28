using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using static CTRResources;
using SodaGameLibrary.Utils;

public class CTRResources
{
    //json에서 데이터 읽어와서 각각 객체에 맞게 변환해주는 과정
    [JsonIgnore]
    private static CTRResources instance = null;
    [JsonIgnore]
    //1. json읽어오기
    public static CTRResources Instance
    {
        get
        {
            if (instance == null)
            {
                var text = Resources.Load<TextAsset>("CTR_Resources").text;
                instance = JsonConvert.DeserializeObject<CTRResources>(text);
            }

            return instance;
        }
    }
    //2. 해당하는 클래스 생성해주기
    //3. 클래스 안에 멤버 변수 만들어서 값 넣어주기
    //4. 해당 하는 값 자유롭게 사용하기

    //일단 json에 어떤 데이터 있는지 알고, 그 데이터 어떤식으로 사용할지도 구분

    public CrossTheRiverData[] crossTheRiverData;
    public CrossTheRiverBoardData[] crossTheRiverBoardData;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this).ToString();
    }

    public enum eColor
    {
        None,
        Red,
        Blue,
        Green,
        Yellow
    }
    public enum eShape
    {
        None,
        Circle,
        Star,
        Triangle,
        Rectangle
    }
    public enum eRuleType
    {
        Color,
        Shape,
        Count
    }
}



public class CrossTheRiverData
{
    public bool IsPractice => false; //항상 거짓
    public int Stage { get; set; } 
    public int Level { get; set; }
    public int Number { get; set; }
    public string[] rules { get; set; } //규칙

    public string[] showenRules { get; set; } //보여줄 힌트
    public int BoardNumber { get; set; } //보드판 번호
    public int[] Correct { get; set; } //정답cell 위치번호(여러개)
}
public class CrossTheRiverBoardData
{
    public int Number { get; set; }
    public string[] cells { get; set; } //cell위치번호_색상_문양_개수 / 순서(format으로 잘라줘야됨) => CrossTheRiverCellData로 나눠줌


}

public class CrossTheRiverCellData //: IStringParser
{
    public int cellPositionNumber;

    public Vector2Int position => new Vector2Int((cellPositionNumber - 1) % 8, (cellPositionNumber - 1) / 8);

    public eColor color;

    public eShape shape;

    public int count;


    public void FromJsonStore(string data)
    {
        var values = data.Split('_');
        cellPositionNumber = int.Parse(values[0]);
        color = EnumUtil.Parsing<eColor>(values[1]);

        var value = string.Empty;
        if (values[2].Contains("Tryangle"))
            value = "Triangle";
        else if (values[2].Contains("Square"))
            value = "Rectangle";
        else
            value = values[2];

        shape = EnumUtil.Parsing<eShape>(value);
        count = int.Parse(values[3]);
    }

    public string ToJsonStore()
    {
        return string.Format("{0},{1}_{2}_{3}_{4}", cellPositionNumber, color, shape, count);
    }
}
