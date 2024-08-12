using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndingMenu : MonoBehaviour
{

    int[] level_Sequence = new int[5] { 1, 1, 1, 1, 3 };

    public InputActionAsset inputActions;  // UFO Input Action Asset 참조
    public Button startButton;
    public Button exitButton;

    private InputAction moveAction;
    private InputAction submitAction;
    private Button[] buttons;
    private int selectedIndex = 0;

    private void OnEnable()
    {
        // 특정 씬에서만 활성화
        if (SceneManager.GetActiveScene().name != "MainMenu" 
            && SceneManager.GetActiveScene().name != "HappyEnding" 
            && SceneManager.GetActiveScene().name != "BadEnding" 
            && SceneManager.GetActiveScene().name != "GameOver")
        {
            gameObject.SetActive(false);
            return;
        }

        // InputAction 설정
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        submitAction = inputActions.FindActionMap("Player").FindAction("Submit");

        moveAction.Enable();
        submitAction.Enable();

        moveAction.started += OnMovePerformed;
        submitAction.started += OnSubmitPerformed;
    }

    private void OnDisable()
    {
        // 씬 전환 시 InputAction 비활성화 및 이벤트 해제
        if (moveAction != null)
        {
            moveAction.started -= OnMovePerformed;
            moveAction.Disable();
        }

        if (submitAction != null)
        {
            submitAction.started -= OnSubmitPerformed;
            submitAction.Disable();
        }
    }

    private void Start()
    {
        buttons = new Button[] { startButton, exitButton };
        UpdateButtonSelection();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (buttons == null || buttons.Length == 0)
            return;  // 버튼이 없으면 입력을 무시

        Vector2 input = context.ReadValue<Vector2>();

        if (input.y > 0)
        {
            SendMessage("OnMoveUp");
        }
        else if (input.y < 0)
        {
            SendMessage("OnMoveDown");
        }
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        if (buttons == null || buttons.Length == 0)
            return;  // 버튼이 없으면 입력을 무시

        SendMessage("OnSubmit");
    }

    private void OnMoveUp()
    {
        selectedIndex--;
        if (selectedIndex < 0)
            selectedIndex = buttons.Length - 1;
        UpdateButtonSelection();
    }

    private void OnMoveDown()
    {
        selectedIndex++;
        if (selectedIndex >= buttons.Length)
            selectedIndex = 0;
        UpdateButtonSelection();
    }

    private void OnSubmit()
    {
        buttons[selectedIndex].onClick.Invoke();
    }

    private void UpdateButtonSelection()
    {
        // 모든 버튼의 텍스트 색상을 비활성화 색상으로 변경
        foreach (Button button in buttons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.color = Color.white;  // 비활성화 색상 (원하는 색으로 변경 가능)
            }
        }

        // 선택된 버튼의 텍스트 색상을 활성화 색상으로 변경
        TextMeshProUGUI selectedText = buttons[selectedIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (selectedText != null)
        {
            selectedText.color = Color.yellow;  // 활성화 색상 (원하는 색으로 변경 가능)
        }
    }

    public void GameStartClick()
    {
        if(LoadingScene.instance.idx < 0) LoadingScene.instance.LoadingStart();
    }

    public void ExitMenu() {
        LoadingScene.instance.idx = -1;
        SceneManager.LoadScene(0);
    }
    public void RestartClick() {
        GameManager.Instance.ResetProgress();
        SceneManager.LoadScene(level_Sequence[LoadingScene.instance.idx]);
    }
    public void GameExit() => Application.Quit();

}
