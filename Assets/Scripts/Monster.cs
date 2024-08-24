using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    MonsterManager monsterManager;
    CharacterManager characterManager;
    public bool isStop = false;

    public bool isDamageStop = false;

    public bool isBottom = true;
    public MonsterManager.MonsterInfo monsterInfo = MonsterManager.MonsterInfo.Normal;

    public int MaxHealth = 0;
    public int curHealth = 0;
    public float stopTimer = 0.5f;
    public float stopTiming = 0f;
    public float speed = 0.1f;
    public float barricateTimer = 10.0f;
    public float barricateTiming = 0f;
    public Transform ForwardPos;
    public LayerMask targetLayer;

    public bool moveBuff = false;

    public bool isDie = false;

    private void Start()
    {
        monsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        ForwardPos = transform.Find("ForwardPos");
        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

    }
    private void Update()
    {
        if (!isStop)
        {
            if (isDamageStop)
            {
                if (isBottom)
                {
                    RaycastHit[] raycastHits = Physics.SphereCastAll(ForwardPos.position, 0.1f, Vector3.up, 0, targetLayer.value);
                    for(int i = 0; i < raycastHits.Length; i++)
                    {
                        if (raycastHits[i].transform.name.Contains("Barri"))
                        {
                            transform.Rotate(Vector3.up);
                            return;
                        }
                        if (raycastHits[i].transform.name.Contains("Monster") && raycastHits[i].transform != this.transform)
                        {
                            return;
                        }
                    }
                    if(transform.rotation.eulerAngles.x < 90.0f)
                    {
                        transform.Rotate(Vector3.down);
                    }

                }
                if (!moveBuff)
                {
                    transform.position += speed * Time.deltaTime * transform.forward;
                }
                else
                {
                    transform.position += speed * 1.5f * Time.deltaTime * transform.forward;
                }

                if(transform.position.y <= 1)
                {
                    characterManager.Damage(curHealth);
                    monsterManager.monsters.Remove(this);
                    Destroy(gameObject);
                }
            }
            else
            {
                if(stopTiming > stopTimer)
                {
                    isDamageStop = true;
                }
                else
                {
                    stopTiming += Time.deltaTime;
                }
            }
        }
    }
    public void Damage(int value)
    {
        if (!isDie)
        {
            curHealth -= value;
            if(curHealth <= 0)
            {
                monsterManager.SetMonsterEvent(this);
                isDie = true;
            }
            else
            {
                Debug.Log("Damage : " + value);
            }
        }
    }
    
    public void SetMonster()
    {
        switch (monsterInfo)
        {
            case MonsterManager.MonsterInfo.Normal:
                isBottom = Random.Range(0, 2) == 0 ? true : false;
                MaxHealth = 1;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.Barricate:
                isBottom = true;
                MaxHealth = 2;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.Boss:
                isBottom = true;
                MaxHealth = 50;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.MonsterDivide:
                isBottom = false;
                MaxHealth = 6;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.Bomb:
                isBottom = true;
                MaxHealth = 2;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.Heal:
                isBottom = true;
                MaxHealth = 4;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.MoveAdd:
                isBottom = false;
                MaxHealth = 4;
                curHealth = MaxHealth;
                break;

            case MonsterManager.MonsterInfo.subBoss:
                isBottom = false;
                MaxHealth = 20;
                curHealth = MaxHealth;
                break;

        }
    }
}
