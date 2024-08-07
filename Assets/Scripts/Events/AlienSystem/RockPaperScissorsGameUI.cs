using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class RockPaperScissorsGameUI : MonoBehaviour
{
    public TMP_Text[] choiceTexts;  // Rock, Paper, Scissors 텍스트
    public Image[] choiceImage;     // Rock, Paper, Scissors 이미지
    private int selectedIndex = 0;
    private string[] choices = { "Rock", "Scissors", "Paper" };

    public Image[] alienChoiceImage;    // 외계인 픽

    private PlayerInput playerInput;
    private InputAction navigateAction;
    private InputAction submitAction;

    public bool isSubmitted = false; // 이미 제출했는지 여부를 추적하는 플래그

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        // Get the actions from the PlayerInput component
        navigateAction = playerInput.actions["Navigate"];
        submitAction = playerInput.actions["Submit"];
    }

    private void OnEnable()
    {
        // Subscribe to action events with lambda expressions
        navigateAction.performed += ctx => OnNavigate();
        submitAction.performed += ctx => OnSubmit();
    }

    private void OnDisable()
    {
        // Unsubscribe from action events
        navigateAction.performed -= ctx => OnNavigate();
        submitAction.performed -= ctx => OnSubmit();
    }

    private void Start()
    {
        UpdateChoiceUI();
    }

    private void OnNavigate()
    {
        Vector2 input = navigateAction.ReadValue<Vector2>();

        // Debug.Log(input.x);

        if (input.x > 0) // 오른쪽 방향키
        {
            MoveSelection(1);
        }
        else if (input.x < 0) // 왼쪽 방향키
        {
            MoveSelection(-1);
        }
    }

    private void OnSubmit()
    {
        // 이미 제출된 상태면 함수 종료
        if (isSubmitted)
            return;

        isSubmitted = true; // 제출 상태로 설정

        Debug.Log("서브밋 호출됨");
        string playerChoice = choices[selectedIndex];
        string alienChoice = choices[Random.Range(0, choices.Length)];
        Debug.Log("Player choice: " + playerChoice);
        Debug.Log("Alien choice: " + alienChoice);

        // Alien choice 이미지 업데이트
        for (int i = 0; i < alienChoiceImage.Length; i++)
        {
            alienChoiceImage[i].gameObject.SetActive(false);
        }
        if (alienChoice == "Rock")
        {
            alienChoiceImage[0].gameObject.SetActive(true);
        }
        else if (alienChoice == "Scissors")
        {
            alienChoiceImage[1].gameObject.SetActive(true);
        }
        else if (alienChoice == "Paper")
        {
            alienChoiceImage[2].gameObject.SetActive(true);
        }

        int result = DetermineWinner(playerChoice, alienChoice);

        // Call EndMiniGame on the AlienSystem
        GameObject.FindObjectOfType<AlienSystem>().SendMessage("EndMiniGame", result);
    }

    private void MoveSelection(int direction)
    {
        selectedIndex = (selectedIndex - direction + choices.Length) % choices.Length;
        UpdateChoiceUI();
    }

    private void UpdateChoiceUI()
    {
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            choiceTexts[i].color = (i == selectedIndex) ? Color.yellow : Color.white;
            choiceImage[i].color = (i == selectedIndex) ? Color.yellow : Color.white;
        }
    }

    private int DetermineWinner(string playerChoice, string alienChoice)
    {
        if (playerChoice == alienChoice)
        {
            // Draw
            return 1;
        }

        else if ((playerChoice == "Rock" && alienChoice == "Scissors") ||
            (playerChoice == "Paper" && alienChoice == "Rock") ||
            (playerChoice == "Scissors" && alienChoice == "Paper"))
        {
            // Player wins
            return 2;
        }

        // Player loses
        return 3;
    }
}
