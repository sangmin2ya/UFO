using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // 아이템의 크기
    public float size;

    // 아이템의 타입. (Item, Obstacle, Stuck)
    public ItemType type;

    // N극, S극, Off 상태
    public MagnetState magnetState;

    // 아이템 능력 종류
    public ItemAbility ability;

    // 아이템, 장애물의 능력 구분 위한 번호
    public int index_ability;

    private void Start()
    {
        // 랜덤으로 N극 S극 정해줌
        magnetState = (Random.value > 0.5f) ? MagnetState.N : MagnetState.S;
    }


    // 아이템의 능력에 따른 행동을 정의
    public void ExecuteAbility()
    {
        switch (type)
        {
            case ItemType.Item: // Item
                ExecuteItemAbility();
                break;
            case ItemType.Obstacle: // Obstacle
                ExecuteObstacleAbility();
                break;
            default:
                Debug.Log("타입이 지정되지 않음");
                break;
        }
    }

    private void ExecuteItemAbility()
    {
        // index_ability에 따른 Item 능력 구현
        switch (ability)
        {
            case ItemAbility.FuelFilling: // 연료 충전
                FuelFilling();
                break;
            case ItemAbility.SurfaceCleaning: // 부착 장애물 제거
                SurfaceCleaning();
                break;
            case ItemAbility.PoleChange: // 맵에 있는 모든 오브젝트 하나의 pole으로 전환
                PoleChange();
                break;
        }
    }

    // 장애물의 종류는 N 극과 S 극만 구분해주면 됨. 별도의 능력 없음. 플레이어에게 붙기만 함.
    private void ExecuteObstacleAbility()
    {
        Debug.Log(magnetState);
    }


    // Item_플레이어 연료 충전
    public void FuelFilling()
    {
        Debug.Log("연료 충전");
        // 연료 변수 참조해서 값만 늘려주면 됨.

        DestroyItem();
    }

    // Item_ 부착 장애물 제거
    public void SurfaceCleaning()
    {
        Debug.Log("부착 장애물 제거");
        // 플레이어와 접촉 시, 자식 오브젝트 중 Stuck 타입 아이템을 삭제

        DestroyItem();
    }

    public void PoleChange()
    {
        Debug.Log("pole 전환");
        // 배열에 있는 아이템들의 magnetState 을 전환
        // N 극 S 극 랜덤?

        DestroyItem();
    }

    // Stuck_ItemType 을 stuck 으로 바꾸고, 이동 정지
    public void Freeze()
    {
        this.type = ItemType.Stuck;
        this.magnetState = MagnetState.Off;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void DestroyItem()
    {
        // 아이템이 자연스러운 애니메이션 등을 재생하며 삭제
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExecuteAbility();
        }
    }
}
