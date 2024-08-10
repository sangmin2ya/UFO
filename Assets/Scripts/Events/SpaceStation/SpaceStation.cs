using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] bool NiceDocking = false;
    [SerializeField] float speed = .02f;

    private GameObject conversation;

    public int maxHP = 100;
    public int currentHP;
    public Image hpImage;
    public float moveDuration = 2.0f;
    public Vector3 startPosition;

    private SpriteRenderer spriteRenderer;
    private bool isInvulnerable = false;

    void Start()
    {
        conversation = GameObject.Find("PlayerCanvas").transform.Find("Conversation").gameObject;
        
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 현재 위치에서 (0,0)으로 이동
        startPosition = transform.position;
        StartCoroutine(MoveToPosition(Vector3.zero, moveDuration));
    }

    IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        float time = 0;
        Vector3 start = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = target;

        EnableFunctionality();
    }

    void EnableFunctionality()
    {
        // on rigidbody, collider
        this.enabled = true;
    }

    void UpdateHP(int amount)
    {
        if (isInvulnerable) return;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        hpImage.fillAmount = (float)currentHP/maxHP;

        if (amount < 0)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            //gameoverscene
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    IEnumerator InvulnerabilityCooldown(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ItemInfo>().type == ItemType.Obstacle && collision.gameObject.GetComponent<ItemInfo>().onSpaceStation == false)
        {
            collision.gameObject.GetComponent<ItemInfo>().onSpaceStation = true;
            UpdateHP(-5);
        }
    }

    /*
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
    */
}
