using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public List<string> Score = new List<string>();
    public int dieMonsterNum = 0;
    public float gameTime = 0;
    public int curScore = 0;
    public int pickItem = 0;
    public int nodeNum = 0;
    public float toalSuccess = 0;
    public float averageSuccess = 0;
    
    // 배경음악
    public int isBackMusic = 1;     // 1 On 0 Off
    // 전투효과음
    public int isSound = 1;
    // 판정성공음
    public int isJudSound = 1;
    // 흑백모드
    public int blackMode = 0;
    // UI 흑백모드
    public int UIBlackMode = 0;
    // 전투 이펙트
    public int effect = 1;

    public KeyCode[] keyCode =
    {
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.H,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L
    };

    public int nodeType = 0;
    public int nodeStartPos = 0;
    public string[] nodeTypeString = { "기본", "색약", "도형" };
    public string[] nodeStartPosString = { "왼쪽", "중앙", "오른쪽" };

    private void Start()
    {
        var temp = FindObjectsOfType<GameData>();
        if (temp.Length > 1)
            Destroy(this.gameObject);
        
        DontDestroyOnLoad(this.gameObject);
        isBackMusic = PlayerPrefs.GetInt("isBackMusic", 1);
        isSound = PlayerPrefs.GetInt("isSound", 1);
        isJudSound = PlayerPrefs.GetInt("isJudSound", 1);
        blackMode = PlayerPrefs.GetInt("blackMode", 0);
        UIBlackMode = PlayerPrefs.GetInt("UIBlackMode", 0);
        effect = PlayerPrefs.GetInt("effect", 1);


        keyCode[0] = PlayerPrefs.GetInt("Line1", (int)KeyCode.A) == (int)KeyCode.A ? KeyCode.A : (KeyCode)PlayerPrefs.GetInt("Line1");
        keyCode[1] = PlayerPrefs.GetInt("Line2", (int)KeyCode.S) == (int)KeyCode.S ? KeyCode.S : (KeyCode)PlayerPrefs.GetInt("Line2");
        keyCode[2] = PlayerPrefs.GetInt("Line3", (int)KeyCode.D) == (int)KeyCode.D ? KeyCode.D : (KeyCode)PlayerPrefs.GetInt("Line3");
        keyCode[3] = PlayerPrefs.GetInt("Line4", (int)KeyCode.F) == (int)KeyCode.F ? KeyCode.F : (KeyCode)PlayerPrefs.GetInt("Line4");
        keyCode[4] = PlayerPrefs.GetInt("Line5", (int)KeyCode.H) == (int)KeyCode.H ? KeyCode.H : (KeyCode)PlayerPrefs.GetInt("Line5");
        keyCode[5] = PlayerPrefs.GetInt("Line6", (int)KeyCode.J) == (int)KeyCode.J ? KeyCode.J : (KeyCode)PlayerPrefs.GetInt("Line6");
        keyCode[6] = PlayerPrefs.GetInt("Line7", (int)KeyCode.K) == (int)KeyCode.K ? KeyCode.K : (KeyCode)PlayerPrefs.GetInt("Line7");
        keyCode[7] = PlayerPrefs.GetInt("Line8", (int)KeyCode.L) == (int)KeyCode.L ? KeyCode.L : (KeyCode)PlayerPrefs.GetInt("Line8");

        nodeType = PlayerPrefs.GetInt("nodeType", 0);
        nodeStartPos = PlayerPrefs.GetInt("nodeStartPos", 0);

        if (PlayerPrefs.GetString("Score", "") != "")
        {
            string[] tempScore = PlayerPrefs.GetString("Score", "").Split('|');
            for (int i = 0; i < tempScore.Length; i++)
            {
                Score.Add(tempScore[i]);
            }
        }
    }
    
    // 초기화
    public void SetInit()
    {
        isBackMusic = 1;
        isSound = 1;
        isJudSound = 1;
        blackMode = 0;
        UIBlackMode = 0;
        effect = 1;

        for (int i = 0; i < keyCode.Length; i++)
        {
            keyCode[i] = (KeyCode)i;
        }

        nodeType = 0;
        nodeStartPos = 0;
        
        PlayerPrefs.SetInt("isBackMusic", 1);
        PlayerPrefs.SetInt("isSound", 1);
        PlayerPrefs.SetInt("isJudSound", 1);
        PlayerPrefs.SetInt("blackMod", 0);
        PlayerPrefs.SetInt("UIBlackMode", 0);
        PlayerPrefs.SetInt("effect", 1);

        PlayerPrefs.SetInt("Line1", (int)KeyCode.A);
        PlayerPrefs.SetInt("Line2", (int)KeyCode.S);
        PlayerPrefs.SetInt("Line3", (int)KeyCode.D);
        PlayerPrefs.SetInt("Line4", (int)KeyCode.F);
        PlayerPrefs.SetInt("Line5", (int)KeyCode.H);
        PlayerPrefs.SetInt("Line6", (int)KeyCode.J);
        PlayerPrefs.SetInt("Line7", (int)KeyCode.K);
        PlayerPrefs.SetInt("Line8", (int)KeyCode.L);
        
        PlayerPrefs.SetInt("nodeType", 0);
        PlayerPrefs.SetInt("nodeStartPos", 0);
    }

    public void SetBackMusic(int num)
    {
        PlayerPrefs.SetInt("isBackMusic", num);
    }

    public void SetSound(int num)
    {
        PlayerPrefs.SetInt("isSound", num);
    }

    public void SetJudSound(int num)
    {
        PlayerPrefs.SetInt("isJudSound", num);
    }

    public void SetBlackMode(int num)
    {
        PlayerPrefs.SetInt("blackMode", num);
    }

    public void SetUIBlackMode(int num)
    {
        PlayerPrefs.SetInt("UIBlackMode", num);
    }

    public void SetEffect(int num)
    {
        PlayerPrefs.SetInt("effect", num);
    }

    public void SetLine(int num, int value)
    {
        PlayerPrefs.SetInt("Line"+(num+1), value);
        keyCode[num] = (KeyCode)value;
    }

    public string SetNodeType(int num)
    {
        if (num > 2)
        {
            num = 0;
        }
        PlayerPrefs.SetInt("nodeType", num);

        nodeType = num;
        return nodeTypeString[num];
    }

    public string SetNodeStartPos(int num)
    {
        if (num > 2)
        {
            num = 0;
        }
        
        PlayerPrefs.SetInt("nodeStartPos", num);

        nodeStartPos = num;
        return nodeStartPosString[num];
    }

    public void SetScore(int num, string name)
    {
        if (Score.Count == 0)
        {
            Score.Add(name + "," + num);
        }
        else
        {
            bool isInsert = false;
            for (int i = 0; i < Score.Count; i++)
            {
                string[] temp = Score[i].Split(',');
                if (num > int.Parse(temp[0]))
                {
                    Score.Insert(i, name + "," + num);
                    isInsert = true;
                }
            }

            if (!isInsert && Score.Count < 10)
            {
                Score.Add(name + "," + num);
            }
            else
            {
                Score.RemoveAt(Score.Count);
            }
        }

        string data = "";
        for (int i = 0; i < Score.Count; i++)
        {
            data += Score[i] + '|';
        }
        PlayerPrefs.SetString("Score", data);
    }
}
