using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 웨이브 간격
    public float waveTimer = 15f;
    public float waveTimming = 15f;
    public bool isWave = false;
    private MonsterManager monsterManager;

    private void Start()
    {
        monsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
    }

    private void Update()
    {
        if (isWave)
        {
            if (waveTimer > waveTimming)
            {
                waveTimming += Time.deltaTime;
            }
            else
            {
                waveTimming = 0;
                monsterManager.Wave();
            }
        }
    }
}
