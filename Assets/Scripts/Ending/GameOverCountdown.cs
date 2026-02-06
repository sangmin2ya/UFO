using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOverCountdown : MonoBehaviour
{
    public InputActionAsset inputActions;  // UFO Input Action Asset 참조
    private int[] level_Sequence = new int[5] { 1, 1, 1, 1, 3 };
    private TextMeshProUGUI countdownText;
    public Button startButton;
    public Button exitButton;
    private InputAction moveAction;
    private InputAction submitAction;
    private int selectedIndex = 0;
    private Button[] buttons;
    // Start is called before the first frame update
    void OnEnable()
    {
        countdownText = transform.Find("CountDown").GetComponent<TextMeshProUGUI>();

        StartCoroutine(StartCountdown(10));

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

    // Update is called once per frame
    void Update()
    {

    }
    private void StopBG()
    {
        BackgroundRepeating[] backgrounds = GameObject.Find("Backgrounds").transform.GetComponentsInChildren<BackgroundRepeating>();
        foreach (var obj in backgrounds)
        {
            obj.Stop();
        }
    }
    private IEnumerator StartCountdown(int seconds)
    {
        int timeLeft = seconds;

        while (timeLeft > 0)
        {
            countdownText.text = timeLeft.ToString();  // 남은 시간을 텍스트로 표시
            yield return new WaitForSeconds(1);  // 1초 대기
            timeLeft--;  // 시간 감소
        }

        countdownText.text = "0";  // 카운트다운이 끝나면 0으로 설정
        LeaveGame();  // 추가 동작 호출
    }
    public void LeaveGame()
    {
        GameManager.Instance.ResetGame();
        LoadingScene.instance.idx = -1;
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        GameManager.Instance.ResetProgress();
        GameManager.Instance._isDead = false;
        SceneManager.LoadScene(level_Sequence[LoadingScene.instance.idx]);
    }
}
