using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // 아이템, 장애물의 크기
    public float size;

    // 아이템, 장애물의 타입. (Item, Obstacle, Stuck)
    public ItemType type;

    // 아이템, 장애물의 능력 구분 위한 번호
    public int index_ability;

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
            case ItemType.Stuck: // Stuck
                ExecuteStuckAbility();
                break;
        }
    }

    private void ExecuteItemAbility()
    {
        // index_ability에 따른 Item 능력 구현
        switch (index_ability)
        {
            case 0: // 연료 충전
                FuelFilling();
                break;
            case 1: // 부착 장애물 제거
                SurfaceCleaning();
                break;
            case 2: // 맵에 있는 모든 오브젝트 하나의 pole으로 전환
                PoleChange();
                break;
        }
    }

    // 장애물의 종류는 N 극과 S 극만 구분해주면 됨. 별도의 능력 없음. 플레이어에게 붙기만 함.
    private void ExecuteObstacleAbility()
    {
        // index_ability에 따른 Obstacle 능력 구현
        switch (index_ability)
        {
            case 0: // N 극
                Debug.Log("N극");
                break;
            case 1: // S 극
                Debug.Log("S극");
                break;
        }
    }

    private void ExecuteStuckAbility()
    {
        // index_ability에 따른 Stuck 능력 구현
        switch (index_ability)
        {
            case 0:
                // Stuck 은 아무 능력 없음. 붙어 있기만 함
                Debug.Log("stuck");
                break;
        }
    }

    // Item_플레이어 연료 충전
    public void FuelFilling()
    {
        Debug.Log("연료 충전");
        // 연료 변수 참조해서 값만 늘려주면 됨.
    }

    // Item_ 부착 장애물 제거
    public void SurfaceCleaning()
    {
        Debug.Log("부착 장애물 제거");
    }

    public void PoleChange()
    {
        Debug.Log("pole 전환");
    }

    // Stuck_ItemType 을 stuck 으로 바꾸고, 이동 정지
    public void Freeze()
    {
        this.type = ItemType.Stuck;
        //
    }

    public void DestroyItem()
    {
        // 아이템이 자연스러운 애니메이션 등을 재생하며 삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExecuteAbility();
        }
    }
}
