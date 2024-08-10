using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipEntered : MonoBehaviour
{
    private bool isInputBlocked = false;
    [SerializeField] private UfoManager ufoManager;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(BlockInputForSeconds(3f));

        }
    }

    private IEnumerator BlockInputForSeconds(float seconds)
    {
        isInputBlocked = true;
        yield return new WaitForSeconds(seconds);
        ufoManager.CleanSurface();
        //-HP
        isInputBlocked = false;
    }

    void Update()
    {
        if (isInputBlocked)
        {
            //moveX 3seconds
        }
    }
}
