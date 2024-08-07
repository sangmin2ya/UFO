using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSystem : MonoBehaviour
{
    [SerializeField] private GameObject miniGameUI; // Rock Paper Scissors Mini Game UI

    [SerializeField] private GameObject drawUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    [SerializeField] private float displayDuration = 2.0f; // 결과 UI가 표시되는 시간

    FuelManager fuelManager;
    GameObject player;

    private void Awake()
    {
        // 특정 경로를 사용하여 비활성화된 오브젝트 찾기
        
        Transform canvasTransform = GameObject.Find("Canvas").transform;

        miniGameUI = canvasTransform.GetChild(0).gameObject;
        loseUI = canvasTransform.GetChild(1).gameObject;
        drawUI = canvasTransform.GetChild(2).gameObject;
        winUI = canvasTransform.GetChild(3).gameObject;
        



        player = GameObject.FindGameObjectWithTag("Player");
        fuelManager = player.GetComponent<FuelManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        fuelManager = player.GetComponent<FuelManager>();
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

            fuelManager.AddFuel(100);

            // 외계인이 연료를 가득 채워줬습니다.
            StartCoroutine(DisplayResultUI(winUI));
        }
        else // lose
        {
            // Actions when a player loses
            Debug.Log("Player lost the mini game!");

            fuelManager.UseFuel(10);

            // 외계인이 연료를 10% 강탈했습니다.
            StartCoroutine(DisplayResultUI(loseUI));
        }
    }

    private IEnumerator DisplayResultUI(GameObject resultUI)
    {
        resultUI.SetActive(true); // 활성화
        yield return new WaitForSecondsRealtime(displayDuration); // 딜레이

        // 외계인 픽 이미지 비활성화
        for(int i = 0; i < miniGameUI.GetComponent<RockPaperScissorsGameUI>().alienChoiceImage.Length; i++)
        {
            miniGameUI.GetComponent<RockPaperScissorsGameUI>().alienChoiceImage[i].gameObject.SetActive(false);
        }

        miniGameUI.GetComponent<RockPaperScissorsGameUI>().isSubmitted = false;

        resultUI.SetActive(false); // 비활성화

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
