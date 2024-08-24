using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public GameData gameData;
    public CharacterManager characterManager;
    public Transform[] cons;

    public bool isPerfect = false;
    public float conRadius;
    public int comboCount;
    public bool isEnemyAdd = false;
    public int blockMiss = 2;
    public int maxBlockMiss = 10;

    public float perfectTimer = 5.0f;
    public float perfectTiming = 0;

    public GameObject[] prefabTouchEffect;

    public int doubleEnergy = 1;
    public bool isDouble = false;
    public float doubleTimer = 4.0f;
    public float doubleTiming = 0;

    public LayerMask layer;

    private void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(gameData.keyCode[0]))
        {
            ConNode(cons[0].position, layer, true);
        }
        if (Input.GetKeyDown(gameData.keyCode[1]))
        {
            ConNode(cons[1].position, layer, true);
        }
        if (Input.GetKeyDown(gameData.keyCode[2]))
        {
            ConNode(cons[2].position, layer, true);
        }
        if (Input.GetKeyDown(gameData.keyCode[3]))
        {
            ConNode(cons[3].position, layer, true);
        }
        if (Input.GetKeyDown(gameData.keyCode[4]))
        {
            ConNode(cons[0].position, layer, false);
        }
        if (Input.GetKeyDown(gameData.keyCode[5]))
        {
            ConNode(cons[1].position, layer, false);
        }
        if (Input.GetKeyDown(gameData.keyCode[6]))
        {
            ConNode(cons[2].position, layer, false);
        }
        if (Input.GetKeyDown(gameData.keyCode[7]))
        {
            ConNode(cons[3].position, layer, false);
        }

        if (isDouble)
        {
            if(doubleTimer < doubleTiming)
            {
                doubleTimer += Time.deltaTime;
            }
            else
            {
                doubleEnergy = 1;
                doubleTiming = 0;
                isDouble = false;
            }
        }

        if (isPerfect)
        {
            if(perfectTimer > perfectTiming)
            {
                perfectTiming += Time.deltaTime;
            }
            else
            {
                perfectTiming = 0;
                isPerfect = false;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="originPos"></param>
    /// <param name="curLayer"></param>
    /// <param name="isBottom"></param>
    private void ConNode(Vector3 originPos, LayerMask curLayer, bool isBottom)
    {
        RaycastHit[] raycastHits = Physics.SphereCastAll(originPos, conRadius, Vector3.up, 0, curLayer.value);

        for(int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].transform.name.Contains("Node"))
            {
                //Debug.Log(raycastHits[i].transform.name);
                float distance = Vector3.Distance(raycastHits[i].transform.position, originPos);

                //강제 퍼펙트
                //if (isPerfect)
                //{
                    
                //}
                if(distance <= 0.25f)
                {
                    if (!raycastHits[i].transform.GetComponent<Node>().isItem)
                    {
                        //perfect
                        Debug.Log("perfect : " + ((1 - distance) * 100) + "%");
                        comboCount++;
                        if(comboCount > 1)
                        {
                            if(comboCount - 1 % 2 == 0)
                            {
                                //SetEnergy(isBottom, 2);
                                //SetEnergy(!isBottom, 1);
                            }
                            else
                            {
                                //SetEnergy(isBottom, 1);
                            }
 
                        }

                        Instantiate(prefabTouchEffect[0], originPos, prefabTouchEffect[0].transform.rotation);
                        Debug.Log((comboCount - 1) + "콤보");
                    }
                    else
                    {

                    }
                }
                else if(distance < 0.8f)
                {
                    if (!raycastHits[i].transform.GetComponent<Node>().isItem)
                    {
                        Debug.Log("Good : " + ((1 - distance) * 100) + "%");
                        comboCount++;
                        Debug.Log((comboCount - 1) + "콤보");
                        
                        Instantiate(prefabTouchEffect[0], originPos, prefabTouchEffect[0].transform.rotation);
                    }
                }
                else
                {
                    if (!raycastHits[i].transform.GetComponent<Node>().isItem)
                    {
                        if(blockMiss > 0)
                        {
                            blockMiss--;
                            Debug.Log("Good : " + ((1 - distance) * 100) + "%");
                            comboCount++;
                            //SetEnergy(isBottom, 1);
                            Debug.Log((comboCount - 1) + "콤보");
                            
                            Instantiate(prefabTouchEffect[0], originPos, prefabTouchEffect[0].transform.rotation);
                        }
                        else
                        {
                            Debug.Log("miss : " + ((1 - distance) * 100) + "%");
                            comboCount = 0;
                            
                            Instantiate(prefabTouchEffect[1], originPos, prefabTouchEffect[0].transform.rotation);
                        }
                    }
                }
                Destroy(raycastHits[i].transform.gameObject);
            }
        }
        if(blockMiss > 0)
        {
            blockMiss--;
        }
        else
        {
            Debug.Log("miss");
            comboCount = 0;
        }
    }
    public void SetMiss()
    {
        if(blockMiss > 0)
        {
            blockMiss--;
        }
        else
        {
            Debug.Log("miss");
            comboCount = 0;
        }
    }
    public void SetEnergy(bool isBottom, int num)
    {
        if (isBottom)
        {
            if(characterManager.curBottomEnergy < characterManager.maxBottomEnergy)
            {
                characterManager.curBottomEnergy += num * doubleEnergy;
                if(characterManager.curBottomEnergy > characterManager.maxBottomEnergy)
                {
                    characterManager.curBottomEnergy += characterManager.maxBottomEnergy;
                }
            }
        }
        else
        {
            if (characterManager.curTopEnergy < characterManager.maxTopEnergy)
            {
                characterManager.curTopEnergy += num * doubleEnergy;
                if (characterManager.curTopEnergy > characterManager.maxTopEnergy)
                {
                    characterManager.curTopEnergy += characterManager.maxTopEnergy;
                }
            }
        }
    }
    public void SetDoubleEnergy()
    {
        isDouble = true;
        doubleTiming = 0;
        doubleEnergy = 2;
    }
    public void SetPerfect()
    {
        isPerfect = true;
        perfectTiming = 0;
    }
}
