using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{
    public GameObject objMain;
    public GameObject objRank;
    public GameObject objSub;
    public GameObject objOption;

    public Transform SubkeyBoard;
    public Transform OptionToggle;
    public Transform OptionKeyBoard;
    public Transform NodeType;
    public Transform NodePos;

    private GameData gameData;
    public bool isKeyChange;
    public int isKeyNum;

    private void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    private void Update()
    {
        if (isKeyChange)
        {
            KeyCode tempkey = DetectPressedKeyCode();
            if (tempkey != KeyCode.None)
            {
                for (int i = 0; i < gameData.keyCode.Length; i++)
                {
                    if (gameData.keyCode[i] == tempkey)
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
            Debug.Log(tempkey.ToString());
            gameData.SetLine(isKeyNum, (int)tempkey);
            OptionKeyBoard.GetChild(isKeyNum).GetChild(0).GetComponent<Text>().text =
                gameData.keyCode[isKeyNum].ToString();
            isKeyChange = false;
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameSub()
    {
        objMain.SetActive(false);
        objSub.SetActive(true);

        for (int i = 0; i < SubkeyBoard.childCount; i++)
        {
            SubkeyBoard.GetChild(i).GetChild(0).GetComponent<Text>().text = gameData.keyCode[i].ToString();
        }
    }

    public void Rank()
    {
        objMain.SetActive(false);
        objRank.SetActive(true);

        Text rankText = objRank.transform.GetChild(2).GetComponent<Text>();
        rankText.text = "";
        for (int i = 0; i < gameData.Score.Count; i++)
        {
            rankText.text += i + ". " + gameData.Score[i] + Environment.NewLine;
        }
    }

    public void Option()
    {
        objMain.SetActive(false);
        objOption.SetActive(true);
        
        // 토글 관련
        OptionToggle.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.isBackMusic == 1 ? "V" : "";
        OptionToggle.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.isSound == 1 ? "V" : "";
        OptionToggle.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.isJudSound == 1 ? "V" : "";
        OptionToggle.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.blackMode == 1 ? "V" : "";
        OptionToggle.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.UIBlackMode == 1 ? "V" : "";
        OptionToggle.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.effect == 1 ? "V" : "";
        
        // 키보드 관련
        for (int i = 0; i < OptionKeyBoard.childCount; i++)
        {
            Debug.Log(gameData.keyCode[i].ToString());
            OptionKeyBoard.GetChild(i).GetChild(0).GetComponent<Text>().text = gameData.keyCode[i].ToString();
        }

        NodeType.GetChild(0).GetComponent<Text>().text = gameData.nodeTypeString[gameData.nodeType];
        NodePos.GetChild(0).GetComponent<Text>().text = gameData.nodeStartPosString[gameData.nodeStartPos];
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetClose()
    {
        objRank.SetActive(false);
        objOption.SetActive(false);
        objSub.SetActive(false);
        objMain.SetActive(true);
    }

    public void TouchToggleBackMusic()
    {
        gameData.isBackMusic = gameData.isBackMusic == 1 ? 0 : 1;
        gameData.SetBackMusic(gameData.isBackMusic);
        OptionToggle.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.isBackMusic == 1 ? "V" : "";
    }

    public void TouchToggleSound()
    {
        gameData.isSound = gameData.isSound == 1 ? 0 : 1;
        gameData.SetSound(gameData.isSound);
        OptionToggle.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.isSound == 1 ? "V" : "";
    }
    
    public void TouchToggleJudSound()
    {
        gameData.isJudSound = gameData.isJudSound == 1 ? 0 : 1;
        gameData.SetJudSound(gameData.isJudSound);
        OptionToggle.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.isJudSound == 1 ? "V" : "";
    }
    
    public void TouchToggleBlackMode()
    {
        gameData.blackMode = gameData.blackMode == 1 ? 0 : 1;
        gameData.SetBlackMode(gameData.blackMode);
        OptionToggle.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.blackMode == 1 ? "V" : "";
    }
    
    public void TouchToggleUIBlackMode()
    {
        gameData.UIBlackMode = gameData.UIBlackMode == 1 ? 0 : 1;
        gameData.SetUIBlackMode(gameData.UIBlackMode);
        OptionToggle.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.UIBlackMode == 1 ? "V" : "";
    }
    
    public void TouchToggleEffect()
    {
        gameData.effect = gameData.effect == 1 ? 0 : 1;
        gameData.SetEffect(gameData.effect);
        OptionToggle.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text = gameData.effect == 1 ? "V" : "";
    }

    public void TouchKeyBoard(int num)
    {
        isKeyNum = num;
        isKeyChange = true;
    }

    public void TouchNodeType()
    {
        gameData.SetNodeType(gameData.nodeType + 1);
        NodeType.GetChild(0).GetComponent<Text>().text = gameData.nodeTypeString[gameData.nodeType];
    }
    
    public void TouchNodePos()
    {
        gameData.SetNodeStartPos(gameData.nodeStartPos + 1);
        NodePos.GetChild(0).GetComponent<Text>().text = gameData.nodeStartPosString[gameData.nodeStartPos];
    }
    
    private KeyCode DetectPressedKeyCode()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                return kcode;
            }
        }
        return KeyCode.None;
    }
}
