using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSystem : MonoBehaviour
{
    [SerializeField] private GameObject miniGameUI; // Rock Paper Scissors Mini Game UI

    [SerializeField] private GameObject drawUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    [SerializeField] private float displayDuration = 2.0f; // ��� UI�� ǥ�õǴ� �ð�

    HPManager fuelManager;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Ư�� ��θ� ����Ͽ� ��Ȱ��ȭ�� ������Ʈ ã��
        
        Transform canvasTransform = player.transform.Find("PlayerCanvas").transform;

        miniGameUI = canvasTransform.GetChild(0).gameObject;
        loseUI = canvasTransform.GetChild(1).gameObject;
        drawUI = canvasTransform.GetChild(2).gameObject;
        winUI = canvasTransform.GetChild(3).gameObject;
    
        
        fuelManager = player.GetComponent<HPManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        fuelManager = player.GetComponent<HPManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartMiniGame();
        }
    }

    private void StartMiniGame()
    {
        Time.timeScale = 0f; // Stop game time.
        miniGameUI.SetActive(true); // Activates the minigame UI.
    }

    public void EndMiniGame(int playerWon)
    {
        if (playerWon == 1)  // draw
        {
            // Actions when a player draw
            Debug.Log("Player draw the mini game!");
            // Bye Bye..
            StartCoroutine(DisplayResultUI(drawUI));
        }
        else if (playerWon == 2)  // player win
        {
            // Actions when a player wins
            Debug.Log("Player won the mini game!");

            fuelManager.RepairedPercent(1);

            // �ܰ����� ���Ḧ ���� ä������ϴ�.
            StartCoroutine(DisplayResultUI(winUI));
        }
        else // lose
        {
            // Actions when a player loses
            Debug.Log("Player lost the mini game!");

            fuelManager.Damaged(10);

            // �ܰ����� ���Ḧ 10% ��Ż�߽��ϴ�.
            StartCoroutine(DisplayResultUI(loseUI));
        }
    }

    private IEnumerator DisplayResultUI(GameObject resultUI)
    {
        resultUI.SetActive(true); // Ȱ��ȭ
        yield return new WaitForSecondsRealtime(displayDuration); // ������

        // �ܰ��� �� �̹��� ��Ȱ��ȭ
        for(int i = 0; i < miniGameUI.GetComponent<RockPaperScissorsGameUI>().alienChoiceImage.Length; i++)
        {
            miniGameUI.GetComponent<RockPaperScissorsGameUI>().alienChoiceImage[i].gameObject.SetActive(false);
        }

        miniGameUI.GetComponent<RockPaperScissorsGameUI>().isSubmitted = false;

        resultUI.SetActive(false); // ��Ȱ��ȭ

        miniGameUI.SetActive(false); // Disables the minigame UI.
        Time.timeScale = 1f; // Resume game time.

        Destroy(gameObject);
    }

    private GameObject FindInactiveObjectByPath(string path)
    {
        Transform objTransform = transform.Find(path);
        if (objTransform != null)
        {
            return objTransform.gameObject;
        }
        return null;
    }
}
