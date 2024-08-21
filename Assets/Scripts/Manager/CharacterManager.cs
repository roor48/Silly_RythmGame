using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    MonsterManager monsterManager;
    bool isStop = true;

    public int maxHealth = 50;
    public int curHealth = 50;

    public int curBottomEnergy = 0;
    public int maxBottomEnergy = 100;

    public int curTopEnergy = 0;
    public int maxTopEnergy = 100;

    public float attackDistance = 7.0f;
    public int attackDamage = 1;

    public int attackNum = 1;
    public float attackTimer = 1.0f;
    public float attackTiming = 0;
    public LayerMask attackLayer;

    public GameObject Bullet;

    public bool isDoubleAttackSpeed = false;
    public float doubleTimer = 2.0f;
    public float doubleTiming = 0;
    // Start is called before the first frame update
    private void Start()
    {
        monsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
        isStop = false;
    }

    public void Update()
    {
        if (!isStop)
        {
            if (isDoubleAttackSpeed)
            {
                if(doubleTimer > doubleTiming)
                {
                    doubleTiming += Time.deltaTime;
                }
                else
                {
                    doubleTiming = 0;
                    isDoubleAttackSpeed = false;
                    attackTimer *= 2;
                }
            }
            if(attackTiming >= attackTimer)
            {
                if(curBottomEnergy > 0)
                {
                    if(curBottomEnergy <= 20)
                    {
                        attackNum = 1;
                    }
                    else if(curBottomEnergy <= 40)
                    {
                        attackNum = 2;
                    }
                    else if(curBottomEnergy <= 80)
                    {
                        attackNum = 4;
                    }
                    else
                    {
                        attackNum = 8;
                    }
                    List<Monster> botMonsters = monsterManager.GetMonsterData(true);
                    attackNum = attackNum < botMonsters.Count ? attackNum : botMonsters.Count;
                    for(int i = 0; i < attackNum; i++)
                    {
                        GameObject obj = Instantiate(Bullet, new Vector3(botMonsters[i].transform.position.x, 0, 0), Quaternion.identity);
                        Bullet bullet = obj.GetComponent<Bullet>();
                        bullet.target = botMonsters[i].transform;
                        bullet.isBottom = true;
                        bullet.attackDamage = attackDamage;
                    }
                }
                if (curTopEnergy > 0)
                {
                    if (curTopEnergy <= 20)
                    {
                        attackNum = 1;
                    }
                    else if (curTopEnergy <= 40)
                    {
                        attackNum = 2;
                    }
                    else if (curTopEnergy <= 80)
                    {
                        attackNum = 4;
                    }
                    else
                    {
                        attackNum = 8;
                    }
                    List<Monster> botMonsters = monsterManager.GetMonsterData(false);
                    attackNum = attackNum < botMonsters.Count ? attackNum : botMonsters.Count;
                    for (int i = 0; i < attackNum; i++)
                    {
                        GameObject obj = Instantiate(Bullet, new Vector3(botMonsters[i].transform.position.x, 0, 0), Quaternion.identity);
                        Bullet bullet = obj.GetComponent<Bullet>();
                        bullet.target = botMonsters[i].transform;
                        bullet.isBottom = false;
                        bullet.attackDamage = attackDamage;
                    }
                }
                attackTiming = 0;
                return;
            }
            attackTiming += Time.deltaTime;
        }
    }

    public void SetDoubleAttackSpeed()
    {
        if (!isDoubleAttackSpeed)
        {
            attackTimer /= 2;
        }

        isDoubleAttackSpeed = true;
        doubleTiming = 0;
    }
    
    public void Damage(int value)
    {
        curHealth -= value;
        if (curHealth <= 0)
        {
            Debug.Log("Game Over");
            monsterManager.GameStop();
            isStop = true;
        }
        else
        {
            Debug.Log("Damage : " + value);
        }
    }
}
