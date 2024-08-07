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

    [Header("EMP")]
    [SerializeField] float EMP_Time = 10;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("occur_Random_Event", 10, 10f);
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
        int rand = Random.Range(0, 101);
        if (rand <= 50) EMP();
        else ShootMissile();
    }

    void EMP()
    {
        GetComponent<Animator>().Play("EMP");
    }

    public void setEmpState()
    {
        Player.GetComponent<UfoController>()._isEMP = true;
        StopAllCoroutines();
        StartCoroutine(EMP_State());
    }

    IEnumerator EMP_State()
    {
        yield return new WaitForSeconds(EMP_Time);
        Player.GetComponent<UfoController>()._isEMP = false;
        if (count++ >= 7) GetComponent<Animator>().Play("BossClear");
    }
}
