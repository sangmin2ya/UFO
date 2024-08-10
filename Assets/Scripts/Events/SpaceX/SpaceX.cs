using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceX : MonoBehaviour
{
    [SerializeField] GameObject ilonMissile;
    public GameObject Player;
    public int maximum_Missile_Num = 7;
    public int followMissileNum = 3;

    bool event_Start = false;
    float t_Event = 0;

    [SerializeField] float EventTime = 15;

    //EMP
    bool _isEmp = false;
    //TimeBubble
    bool _isTimeBubble = false;
    bool _isStorm = false;
    [SerializeField] float timescale_min_max;
    [SerializeField] GameUIManager UImanager;
    [SerializeField] private int _patternCount = 0;
    [SerializeField] private int _hpCount = 0;

    [SerializeField] GameObject Burst_Effect;
    [SerializeField] Transform[] Burst_Transform;
    [SerializeField] TMP_Text Notice;

    Animator anim;

    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        anim.Play("Intro");

        UImanager = GameObject.Find("Canvas").GetComponent<GameUIManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("occur_Random_Event", 5, 20f);

    }

    private void Update()
    {
        CheckEnd();
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
        Notice.text = $"SPACE X에게 반격하기 위해 쓰레기를 <color=red>{_hpCount * 2 + 3}</color>개 이상 붙이세요";
    }

    void ShootMissile()
    {
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
            missile.GetComponent<ElonMissile>().isFollowPlayer = true;
        }
    }

    void occur_Random_Event()
    {
        t_Event = 0;
        _patternCount++;
        if (_patternCount == 3)
        {
            OccurMagnetPattern();
            return;
        }
        event_Start = true;
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: ShootMissile(); break;
            case 1: EMP(); break;
            case 2: TimeBubble(); break;
            case 3: anim.Play("Cosmic_Storm"); break;
        }
    }
    private void OccurMagnetPattern()
    {
        _patternCount = 0;
        CancelInvoke("occur_Random_Event");
        UImanager.showEvent("스페이스 X가 크기 " + (_hpCount * 2 + 3) + "의 강력한 자기장을 발생시킵니다!");
        anim.Play("Space_Attack_Charging");
        MagnetPattern();
    }
    private void MagnetPattern()
    {
        Invoke("CompareMass", 8);
    }
    private void CompareMass()
    {
        if (Player.GetComponent<UfoManager>()._AttacedObjects.Count >= (_hpCount * 2 + 3))
        {
            _hpCount++;
            anim.SetTrigger("Success");
            UImanager.showEvent("더욱 강력한 자기장으로 스페이스 X에게 반격했습니다!");
            GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnBomb();
            Player.GetComponent<UfoManager>().ClearAll();
        }
        else
        {
            Player.GetComponent<Animator>().enabled = true;
            anim.SetTrigger("Failed");
            GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnBomb();
            UImanager.HideEventText();
            Player.GetComponent<UfoManager>().ClearAll();
        }
        InvokeRepeating("occur_Random_Event", 10, 20f);
    }

    IEnumerator enableAnimation()
    {
        Player.GetComponent<Animator>().enabled = true;
        Player.GetComponent<Animator>().applyRootMotion = false;
        yield return new WaitForSeconds(1f);
        Player.GetComponent<Animator>().enabled = false;

    }

    public void startCameraShaking() => Camera.main.GetComponent<CameraShake>().startCameraShake(.2f, .35f);

    public void BurstSpaceX()
    {
        foreach (var x in Burst_Transform)
        {
            var burst_effect = Instantiate(Burst_Effect, x.position, Quaternion.identity);
            Destroy(burst_effect, 1);
        }

    }

    private void CheckEnd()
    {
        if (_hpCount == 3)
        {
            GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnBomb();
            Player.GetComponent<UfoManager>().ClearAll();
            UImanager.showEvent("스페이스 X를 물리쳤습니다!");
            CancelInvoke("occur_Random_Event");
            StartCoroutine(EndEvent());
            StartCoroutine(FadeOutImage());
        }
    }
    IEnumerator EndEvent()
    {
        Camera.main.GetComponent<CameraShake>().startCameraShake(0.01f, 5);
        GameObject go = transform.Find("Burst").gameObject;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            go.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    IEnumerator FadeOutImage()
    {
        yield return new WaitForSeconds(3);
        Image img = GameObject.Find("BlackOut").GetComponent<Image>();
        Color originalColor = img.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

        float elapsedTime = 0;
        while (elapsedTime < 2)
        {
            img.color = Color.Lerp(originalColor, targetColor, elapsedTime / 2);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        img.color = targetColor;
        SceneManager.LoadScene("HappyEnding");
    }
    public void CosmicStorm()
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
        UImanager.showEvent("타임 버블로 인해 시간이 빨라집니다 !");
    }

    void EMP()
    {
        anim.Play("EMP");
    }

    public void setEmpState()
    {
        UImanager.showEvent("EMP 발동 ! 당분간 자성 변경이 불가능합니다.");
        Player.GetComponent<UfoController>()._isEMP = true;
        _isEmp = true;
    }
}
