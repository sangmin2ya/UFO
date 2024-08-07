using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlionMusk : MonoBehaviour
{
    [SerializeField] GameObject ilonMissile;
    GameObject Player;
    public int maximum_Missile_Num = 5;
    public int followMissileNum = 2;
    int count = 0;

    bool event_Start = false;
    float t_Event = 0;

    [SerializeField] float EventTime = 10;

    //EMP
    bool _isEmp = false;
    //TimeBubble
    bool _isTimeBubble = false;
    bool _isStorm = false;
    [SerializeField] Vector2 timescale_min_max;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("occur_Random_Event", 5, 10f);
    }

    private void Update()
    {
        if (event_Start)
        {
            t_Event += Time.deltaTime;
            if(t_Event >= EventTime)
            {
                t_Event = 0;
                event_Start = false;

                if (_isEmp) { Player.GetComponent<UfoController>()._isEMP = false; _isEmp = false; }
                if (_isTimeBubble) { Time.timeScale = 1; _isTimeBubble = false; }
                if (_isStorm) { Player.GetComponent<UfoController>()._isStorm = false; _isStorm = false; }

            }
        }   
    }

    void ShootMissile()
    {
        int num_follow = 0;
        int rand2 = Random.Range(0, 101);
        if (count++ >= 7) { GetComponent<Animator>().Play("BossClear"); return; }
        for (int i = 0; i < maximum_Missile_Num; ++i) {
            int rand = Random.Range(0, 101);
            var missile = Instantiate(ilonMissile, new Vector2(Random.Range(-8f, 8f), Random.Range(-5f, 5f)), Quaternion.identity);
            missile.GetComponent<ElonMissile>().state = rand2 <= 50 ? MagnetState.N : MagnetState.S;
            if (num_follow < followMissileNum) { num_follow += 1; missile.GetComponent<ElonMissile>().isFollowPlayer = true; }
        }
    }

    void occur_Random_Event()
    {
        event_Start = true;
        int rand = Random.Range(0, 4);
        switch (rand) {

            case 0: ShootMissile(); break;
            case 1: EMP(); break;
            case 2: TimeBubble(); break;
            case 3: CosmicStorm(); break;   
        }
    }

    void CosmicStorm()
    {
        _isStorm = true;
        Player.GetComponent<UfoController>()._isStorm = true;
    }

    void TimeBubble()
    {
        _isTimeBubble = true;
        GameObject.Find("Canvas").GetComponent<GameUIManager>().showTimeBubbleEffect();
        Time.timeScale = Random.Range(timescale_min_max.x, timescale_min_max.y);
    }

    void EMP()
    {
        GetComponent<Animator>().Play("EMP");
    }

    public void setEmpState()
    {
        Player.GetComponent<UfoController>()._isEMP = true;
        _isEmp = true;
        event_Start = true;
    }
}
