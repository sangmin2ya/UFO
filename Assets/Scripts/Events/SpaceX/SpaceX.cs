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

    Animator anim;
    [SerializeField] float PlayerDamage = 5f;
    [SerializeField] float MaxHP = 100;
    float currentHP;
    [SerializeField] Image HP_Effect_Image;
    [SerializeField] Image HP_Image;

    [SerializeField] float HP_Speed;
    [SerializeField] float HP_Effect_Speed;

    public float BombDamage = 20f;

    [SerializeField] TMP_Text Boss_HP_Text;
    [SerializeField] TMP_Text PatternLeftText;

    private void Start()
    {
        GameObject.Find("Canvas").GetComponent<GameUIManager>().showDescription(1);

        currentHP = MaxHP;
        anim = transform.parent.GetComponent<Animator>();
        anim.Play("Intro");

        UImanager = GameObject.Find("Canvas").GetComponent<GameUIManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("occur_Random_Event", 5, 20f);

    }

    private void Update()
    {
        CheckEnd();
        ManageHP();
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
        PatternLeftText.text = 2 - _patternCount <= 0 ? "<color=red>반격을 준비하십시오 !" : $"다음 반격까지 남은 패턴 : <color=red>{2 - _patternCount}</color>회";
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

    void ManageHP()
    {
        float target = Mathf.Lerp(HP_Image.fillAmount, currentHP / MaxHP, HP_Speed * Time.deltaTime);
        float targetEffect = Mathf.Lerp(HP_Effect_Image.fillAmount, currentHP / MaxHP, HP_Effect_Speed * Time.deltaTime);

        HP_Image.fillAmount = target;
        HP_Effect_Image.fillAmount = targetEffect;

        Boss_HP_Text.text = $"에일리언 머스크 대마왕 (<color=green>{(target*100).ToString("N1")}</color> %)";
    }

    public void SpawnBombEffect()
    {
        var effect = Instantiate(Burst_Effect, new Vector2(4.63f, 0.11f), Quaternion.identity);
        effect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 96.3f));
        effect.transform.localScale = new Vector2(30, 30);
    }

    public void Take_Damage(float amount) => currentHP -= amount;

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
        CancelInvoke("occur_Random_Event");
        UImanager.showEvent("스페이스 X에 대항하십시오 !");
        anim.Play("Space_Attack_Charging");
        MagnetPattern();
    }
    private void MagnetPattern()
    {
        Invoke("CompareMass", 8);
    }
    private void CompareMass()
    {
        _patternCount = 0;

        Take_Damage(Player.GetComponent<UfoManager>()._AttacedObjects.Count * PlayerDamage);

        if (currentHP <= 0) return;

        anim.SetTrigger("Failed");

        UImanager.showEvent($"스페이스 X에게 <color=red>{Player.GetComponent<UfoManager>()._AttacedObjects.Count * PlayerDamage}</color> 만큼의 대미지로 반격했습니다!");
        GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnBomb();
        Player.GetComponent<UfoManager>().ClearAll();

        Camera.main.GetComponent<CameraShake>().startCameraShake(.05f, .2f);

        InvokeRepeating("occur_Random_Event", 10, 20f);
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
        if (currentHP <= 0)
        {
            anim.Play("Die");
            GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnBomb();
            Player.GetComponent<UfoManager>().ClearAll();
            UImanager.showEvent("에일리언 머스크를 혼냈습니다 !");
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
