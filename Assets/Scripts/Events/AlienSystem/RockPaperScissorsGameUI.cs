using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class RockPaperScissorsGameUI : MonoBehaviour
{
    public TMP_Text[] choiceTexts;  // Rock, Paper, Scissors �ؽ�Ʈ
    public Image[] choiceImage;     // Rock, Paper, Scissors �̹���
    private int selectedIndex = 0;
    private string[] choices = { "Rock", "Scissors", "Paper" };

    public Image[] alienChoiceImage;    // �ܰ��� ��

    private PlayerInput playerInput;
    private InputAction navigateAction;
    private InputAction submitAction;

    public bool isSubmitted = false; // �̹� �����ߴ��� ���θ� �����ϴ� �÷���

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
        //navigateAction.performed += ctx => OnNavigate();
        //submitAction.performed += ctx => OnSubmit();
    }

    private void OnDisable()
    {
        // Unsubscribe from action events
        //navigateAction.performed -= ctx => OnNavigate();
        //submitAction.performed -= ctx => OnSubmit();
    }

    private void Start()
    {
        UpdateChoiceUI();
    }

    void OnNavigate(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // Debug.Log(input.x);

        if (input.x > 0) // ������ ����Ű
        {
            MoveSelection(-1);
        }
        else if (input.x < 0) // ���� ����Ű
        {
            MoveSelection(1);
        }
    }

    void OnSubmit(InputValue value)
    {
        // �̹� ����� ���¸� �Լ� ����
        if (isSubmitted)
            return;

        isSubmitted = true; // ���� ���·� ����

        Debug.Log("����� ȣ���");
        string playerChoice = choices[selectedIndex];
        string alienChoice = choices[Random.Range(0, choices.Length)];
        Debug.Log("Player choice: " + playerChoice);
        Debug.Log("Alien choice: " + alienChoice);

        // Alien choice �̹��� ������Ʈ
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
