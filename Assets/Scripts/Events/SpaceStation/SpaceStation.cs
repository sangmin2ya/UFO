using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] bool NiceDocking = false;
    [SerializeField] float speed = .02f;

    [SerializeField] Collider2D good, bad;

    private GameObject conversation;
    void Start()
    {
        conversation = GameObject.Find("PlayerCanvas").transform.Find("Conversation").gameObject;
    }
    private void FixedUpdate()
    {
        transform.parent.gameObject.transform.Translate(Vector2.up * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelManager>(out var result))
        {
            good.enabled = false;
            bad.enabled = false;

            if (!NiceDocking)
            {
                // 덜 좋은 효과. 최대 75%까지만 주유. 이미 75% 이상이라면 주유 되지 않음
                result._fuel = Mathf.Max(50, result._fuel);
                Time.timeScale = 0f;
                conversation.SetActive(true);
                conversation.transform.Find("Script").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = "우주 정거장에 어서오세요!\n연료는 드리겠지만 다음에는 좀 더 부드럽게 착륙해주세요!\n<color=yellow>연료 50% 회복</color>";
            }
            else
            {
                // 좋은 효과. 주유 가득
                result.AddFuel(100);
                Time.timeScale = 0f;
                conversation.SetActive(true);
                conversation.transform.Find("Script").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = "우주 정거장에 어서오세요!\n좋은 운전 실력을 보여주신 대가로 연료를 채워드릴게요!\n<color=yellow>연료 100% 회복</color>";
            }
        }

    }
}
