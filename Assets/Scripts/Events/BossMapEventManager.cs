using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMapEventManager : MonoBehaviour
{
    [SerializeField] GameObject BurstEffect;
    [SerializeField] GameObject SpaceX;
    [SerializeField] SpaceX AlionMusk;
    [SerializeField] GameObject Player;

    public void DisablePlayerAnimator() => Player.GetComponent<Animator>().enabled = false;

    public void StartCameraShake()
    {
        Camera.main.GetComponent<CameraShake>().startCameraShake(.075f, .5f);
    }

    public void ShowBurstEffect() {
        Camera.main.GetComponent<CameraShake>().startCameraShake(.05f, .2f);
        Instantiate(BurstEffect, SpaceX.transform.position, Quaternion.identity);
    }

    public void EMP()
    {
        AlionMusk.setEmpState();
    }

    public void Cosmic_Storm() => AlionMusk.CosmicStorm();
}
