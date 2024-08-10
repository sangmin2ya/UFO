using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlionMusk : MonoBehaviour
{
    [SerializeField] GameObject ilonMissile;
    GameObject Player;
    public int maximum_Missile_Num = 5;
    public int followMissileNum = 2;
    [SerializeField]int count = 0;

    bool event_Start = false;
    float t_Event = 0;

    [SerializeField] float EventTime = 10;

    //EMP
    bool _isEmp = false;
    //TimeBubble
    bool _isTimeBubble = false;
    bool _isStorm = false;
    [SerializeField] float timescale_min_max;
    [SerializeField] GameUIManager UImanager;
    [SerializeField] TMP_Text WaveText;

    private void Start()
    {
        UImanager = GameObject.Find("Canvas").GetComponent<GameUIManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("occur_Random_Event", 5, 15f);
    }

    private void Update()
    {
        WaveText.text = $"에일리온 머스크의 공격으로부터 생존하세요!\n({count}/8)";
        if (event_Start)
        {
            t_Event += Time.deltaTime;
            if (t_Event >= EventTime)
            {
                UImanager.HideEventText();

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
        for (int i = 0; i < maximum_Missile_Num; ++i)
        {
            int rand3 = Random.Range(0, 101);

            Vector2 leftright1 = new Vector2(Random.Range(-12f, -9.5f), Random.Range(-10f, 10f));
            Vector2 leftright2 = new Vector2(Random.Range(9.5f, 12f), Random.Range(-10f, 10f));
            Vector2 upVec = new Vector2(Random.Range(-9f, 9f), Random.Range(6f, 10f));
            Vector2 downVec = new Vector2(Random.Range(-9f, 9f), Random.Range(-10f, -6f));

            var missile = Instantiate(ilonMissile, rand3 >= 75 ? leftright1 : rand3 >= 50 ? leftright2 : rand3 >= 25 ? upVec : downVec, Quaternion.identity);
            missile.GetComponent<ElonMissile>().state = rand2 <= 50 ? MagnetState.N : MagnetState.S;
            if (num_follow < followMissileNum) { num_follow += 1; missile.GetComponent<ElonMissile>().isFollowPlayer = true; }
        }
    }

    void occur_Random_Event()
    {
        count += 1;
        if (count >= 9) {
            Time.timeScale = 1;
            GameManager.Instance.AddStage();
            GetComponent<Animator>().Play("BossClear");
            LoadingScene.instance.LoadingStart();
            return;
        }
        t_Event = 0;
        event_Start = true;
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: ShootMissile(); break;
            case 1: EMP(); break;
            case 2: TimeBubble(); break;
            case 3: CosmicStorm(); break;
        }
    }

    void CosmicStorm()
    {
        UImanager.showEvent("우주 폭풍이 몰아쳐 시야가 제한됩니다 !");

        _isStorm = true;
        Player.GetComponent<UfoController>()._isStorm = true;
    }

    void TimeBubble()
    {

        _isTimeBubble = true;
        UImanager.showTimeBubbleEffect();
        float x = Random.Range(1.2f, timescale_min_max);
        Time.timeScale = x;
        UImanager.showEvent(x >= 1 ? "타임 버블로 인해 시간이 빨라집니다 !" : "타임 버블로 인해 시간이 느려집니다..");

    }

    void EMP()
    {
        GetComponent<Animator>().Play("EMP");
    }

    public void setEmpState()
    {
        UImanager.showEvent("EMP 발동 ! 당분간 자성 변경이 불가능합니다.");
        Player.GetComponent<UfoController>()._isEMP = true;
        _isEmp = true;
    }
}
