using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public ControlManager controlManager;
    public CharacterManager characterManager;
    public MonsterManager monsterManager;
    [SerializeField] Transform[] Spawns;


    //노드 프리팹
    public GameObject prefabNode;

    public int NodeCount = 10;
    
    // 구름이 생길 범위
    public Transform LeftTop;
    public Transform RightBottom;
    public GameObject Cloudy;
    
    public bool isCloudy = false;
    public float cloudyTimer = 5f;
    public float cloudyTiming = 0;

    public float cloudyTimerRepeat = 1f;
    public float cloudyTimingRepeat = 0;

    public bool isGhost = false;
    public float ghostTimer = 3f;
    public float ghostTiming = 0;

    public bool isNodeDown = false;
    public float downTimer = 2.0f;
    public float downTiming = 0;

    public float invisibleTimer;

    public Texture[] items;
    
    public float speed;
    
    public enum NodeInfo
    {
        Normal,
        Invisibile,
        Size,

        Health,
        MonsterStop,
        Energy,
        Miss,
        Timer,
        Perfect,
    }

    private void Start()
    {
        monsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        controlManager = GameObject.Find("ControlManager").GetComponent<ControlManager>();
        
        Spawns = new Transform[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Spawns[i] = this.transform.GetChild(i);
        }
        Wave();
    }

    private void Update()
    {
        if (isCloudy)
        {
            if (cloudyTimer > cloudyTiming)
            {
                cloudyTiming += Time.deltaTime;
                if (cloudyTimerRepeat > cloudyTimingRepeat)
                {
                    cloudyTimingRepeat += Time.deltaTime;
                }
                else
                {
                    cloudyTimingRepeat = 0;
                    SetCloudy();
                }
            }
            else
            {
                isCloudy = false;
                cloudyTimingRepeat = 0;
                cloudyTiming = 0;
            }
        }

        if (isGhost)
        {
            if (ghostTimer > ghostTiming)
            {
                ghostTiming += Time.deltaTime;
                SetNodeInvisible();
            }
            else
            {
                isGhost = false;
                ghostTiming = 0;
            }
        }

        if (isNodeDown)
        {
            if (downTiming >= downTimer)
            {
                downTiming = 0;
                isNodeDown = false;
                speed /= 2;
            }
            else
            {
                downTiming += Time.deltaTime;
            }
        }
    }
    public void Wave()
    {
        bool isBottom = false;

        for(int i = 0; i < 200; i++)
        {
            GameObject obj = Instantiate(prefabNode, Spawns[i % 4].position + new Vector3(0, i, 0), prefabNode.transform.rotation, this.transform);
            Node node = obj.GetComponent<Node>();
            node.isBottom = !isBottom;
            isBottom = !isBottom;
            node.nodeInfo = (NodeInfo)(i % 9);
            //if(node.nodeInfo >= NodeInfo.Health)
            //{
            //    obj.GetComponent<Renderer>().material.mainTexture = items[(int)node.nodeInfo - 3];
            //}
            node.SetNode();
            if (node.isBottom)
            {
                node.GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                node.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
    public void SetItem(NodeInfo nodeInfo)
    {
        switch (nodeInfo)
        {
            case NodeInfo.Health:
                characterManager.curHealth += characterManager.maxHealth / 2;
                if(characterManager.curHealth > characterManager.maxHealth)
                {
                    characterManager.curHealth = characterManager.maxHealth;
                }
                break;

            case NodeInfo.MonsterStop:
            {
                monsterManager.SetItemStop();
                break;
            }
            case NodeInfo.Energy:
            {
                controlManager.SetDoubleEnergy();
                break;
            }
            case NodeInfo.Miss:
            {
                controlManager.blockMiss += 2;
                if(controlManager.blockMiss > controlManager.maxBlockMiss)
                {
                    controlManager.blockMiss = controlManager.maxBlockMiss;
                }
                break;
            }
            case NodeInfo.Timer:
            {
                characterManager.SetDoubleAttackSpeed();
                isNodeDown = true;
                downTiming = 0;
                speed /= 2;
                break;
            }
            case NodeInfo.Perfect:
            {
                
                controlManager.SetPerfect();
                break;
            }
        }
    }

    public void SetMiss()
    {
        controlManager.SetMiss();
    }

    public void SetCloudy()
    {
        int cloudyNum = Random.Range(2, 6);
        for (int i = 0; i < cloudyNum; i++)
        {
            float posX = Random.Range(LeftTop.position.x, RightBottom.position.x);
            float posY = Random.Range(RightBottom.position.y, LeftTop.position.y);
            GameObject obj = Instantiate(Cloudy, new Vector3(posX, posY, -1),
                Cloudy.transform.rotation, this.transform);
            Destroy(obj, 0.5f);
        }
    }

    public void SetNodeInvisible()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name.Contains("Node"))
            {
                if (this.transform.GetChild(i).position.y <= speed * invisibleTimer)
                {
                    this.transform.GetChild(i).GetComponent<Renderer>().material.color =
                        new Color(1f, 1f, 1f, 0);
                }
            }
        }
    }
}
