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
        if (collision.gameObject.TryGetComponent<HPManager>(out var result))
        {
            good.enabled = false;
            bad.enabled = false;

            if (!NiceDocking)
            {
                Time.timeScale = 0f;
                conversation.SetActive(true);
                conversation.transform.Find("Script").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = "우주 정거장에 어서오세요!\n수리는 해드리겠지만 다음에는 좀 더 부드럽게 착륙해주세요!\n<color=yellow>50% 수리</color>";
            }
            else
            {
                Time.timeScale = 0f;
                conversation.SetActive(true);
                conversation.transform.Find("Script").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = "우주 정거장에 어서오세요!\n좋은 운전 실력을 보여주신 대가로 수리를 해드릴게요!\n<color=yellow>100% 수리</color>";
            }
        }

    }
}
