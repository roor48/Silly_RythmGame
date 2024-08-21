using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterManager : MonoBehaviour
{
    GameManager gameManager;
    CharacterManager characterManager;
    NodeManager nodeManager;

    [SerializeField]
    Transform[] Spawns;

    [SerializeField]
    int WaveNum = 0;

    [SerializeField]
    int waveMonCount = 5;

    public GameObject prefabMon;
    public GameObject barriMon;

    public List<Monster> monsters = new List<Monster>();
    Vector3 originPos = new Vector3(0, 20, 0);

    float bossTimer = 20.0f;
    float bossTiming = 0;
    bool isBoss = false;
    public bool isCamera = false;
    public float cameraTimer = 5;
    public float cameraTiming = 0;
    bool isItemStop = true;
    public float itemStopTimer = 2.0f;
    public float itemStopTiming = 0;
    
    public enum MonsterInfo
    {
        Normal,
        Bomb,
        Heal,
        MoveAdd,
        MonsterDivide,
        Barricate,

        subBoss,
        Boss,
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        nodeManager = GameObject.Find("NodeManager").GetComponent<NodeManager>();

        InvokeRepeating("OnMoveAdd", 2.0f, 1.0f);
    }

    private void Update()
    {
        if (isBoss)
        {
            if (bossTiming > bossTimer)
            {
                isBoss = false;
                if (WaveNum == 6)
                {
                    GameObject obj = Instantiate(prefabMon, new Vector3(-20, 12, 0), prefabMon.transform.rotation,
                        this.transform);
                    obj.transform.localScale *= 4.0f;
                    Monster subBoss = obj.GetComponent<Monster>();
                    subBoss.monsterInfo = MonsterInfo.subBoss;
                    // obj.GetComponent<Renderer>().material.mainTexture = monTexture[(int)MonsterInfo.subBoss];
                    subBoss.SetMonster();
                    monsters.Add(obj.GetComponent<Monster>());
                }
                
                // 중간, 보스 등장
                bossTiming = 0;
                nodeManager.isCloudy = true;
                nodeManager.isGhost = true;
                isCamera = true;
                GameObject.Find("RightCamera").GetComponent<Camera>().orthographicSize *= -1f;
            }
            bossTiming += Time.deltaTime;
        }

        if (isCamera)
        {
            if (cameraTiming > cameraTimer)
            {
                cameraTiming = 0;
                isCamera = false;
                GameObject.Find("RightCamera").GetComponent<Camera>().orthographicSize *= -1;
            }
            else
            {
                cameraTiming += Time.deltaTime;
            }
        }

        if (isItemStop)
        {
            if (itemStopTiming > itemStopTimer)
            {
                itemStopTiming = 0;
                isItemStop = false;
                for (int i = 0; i < monsters.Count; i++)
                {
                    monsters[i].isStop = false;
                }
            }
            else
            {
                itemStopTiming += Time.deltaTime;
            }
        }
    }

    public void Wave()
    {
        int num = UnityEngine.Random.Range(0, 4);

        if (WaveNum < 5)
        {
            for (int i = 0; i < waveMonCount; i++)
            {
                GameObject obj = Instantiate(prefabMon, originPos + new Vector3(Spawns[num].position.x, i* 1.5f, 0), prefabMon.transform.rotation, this.transform);
                Monster temp = obj.GetComponent<Monster>();
                MonsterInfo type = (MonsterInfo)Random.Range(0, (int)MonsterInfo.subBoss);
                temp.monsterInfo = type;
                //obj.GetComponent<Renderer>().material.mainTexture = monte
                temp.SetMonster();
                monsters.Add(obj.GetComponent<Monster>());
            }

        }
        else if (WaveNum == 5)
        {
            gameManager.isWave = false;
            
            // 중간보스 등장
            bossTimer = 10.0f;
            isBoss = true;
        }
        else if (WaveNum == 6)
        {
            // 보스 등장
            bossTimer = 20.0f;
            isBoss = true;
        }

        WaveNum++;
    }
    
    public void DieEvent(Monster monster)
    {
        monsters.Remove(monster);
        Destroy(monster.gameObject);
    }

    public void SetItemStop()
    {
        isItemStop = true;
        itemStopTiming = 0;
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].isStop = true;
        }
    }
    
    public void SetMonsterEvent(Monster monster)
    {
        Debug.Log(monster.monsterInfo);
        switch (monster.monsterInfo)
        {
            case MonsterInfo.Normal:
                break;
            case MonsterInfo.Bomb:
                {
                    List<Monster> temp = monsters.FindAll(mon => Vector3.Distance(monster.transform.position, mon.transform.position) < 5f && mon.monsterInfo == MonsterInfo.Bomb);
                    foreach (Monster mon in temp)
                    {
                        mon.curHealth -= 2;
                    }
                }
                break;
            case MonsterInfo.Heal:
                characterManager.curHealth += characterManager.curHealth / 2;
                if(characterManager.curHealth > characterManager.maxHealth)
                {
                    characterManager.curHealth = characterManager.maxHealth;
                }
                break;
            case MonsterInfo.MoveAdd:
                break;
            case MonsterInfo.MonsterDivide:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject obj = Instantiate(prefabMon, new Vector3(monster.transform.position.x + 0.1f * i, monster.transform.position.y, 0), prefabMon.transform.rotation, transform);
                        Monster temp = obj.GetComponent<Monster>();
                        temp.monsterInfo = MonsterInfo.Normal;
                        temp.isBottom = false;
                        temp.MaxHealth = 1;
                        temp.curHealth = 1;
                        temp.isStop = false;

                        monsters.Add(temp);
                    }
                }
                break;
            case MonsterInfo.Barricate:
                {
                    GameObject obj = Instantiate(barriMon, monster.transform.position, monster.transform.rotation, this.transform);
                    Destroy(obj.gameObject, 10.0f);
                }
                break;
            case MonsterInfo.subBoss:
                {

                }
                break;
            case MonsterInfo.Boss:
                {

                }
                break;
        }

        
        foreach(Monster m in monsters)
        {
            if (m.curHealth <= 0)
                DieEvent(m);
        }
    }
    
    public void GameStop()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            monsters[i].isStop = true;
        }
    }
    
    public void OnMoveAdd()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].moveBuff = false;
        }
        foreach(Monster mon in monsters.FindAll(_ => _.monsterInfo == MonsterInfo.MoveAdd))
        {
            for(int i = 0; i < monsters.Count; i++)
            {
                if(Vector3.Distance(mon.transform.position, monsters[i].transform.position) < 5.0f)
                {
                    monsters[i].moveBuff = true;
                }
            }
            mon.moveBuff = true;
        }
    }
    public List<Monster> GetMonsterData(bool isBottom)
    {
        if (isBottom)
        {
            List<Monster> tempMonsters = new List<Monster>();
            for(int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] != null)
                {
                    if (monsters[i].isBottom)
                    {
                        if (monsters[i].transform.position.y <= characterManager.attackDistance)
                        {
                            tempMonsters.Add(monsters[i]);
                        }
                    }
                }
            }
            tempMonsters.Sort((Monster a, Monster b) => a.transform.position.y > b.transform.position.y ? 1 : -1);
            return tempMonsters;
        }
        else
        {
            List<Monster> tempMonsters = new List<Monster>();
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] != null)
                {
                    if (!monsters[i].isBottom)
                    {
                        if (monsters[i].transform.position.y <= characterManager.attackDistance)
                        {
                            tempMonsters.Add(monsters[i]);
                        }
                    }
                }
            }
            tempMonsters.Sort((Monster a, Monster b) => a.transform.position.y > b.transform.position.y ? 1 : -1);
            return tempMonsters;
        }
    }
}
