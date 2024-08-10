using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float _hp;
    private Image _hpBar;
    private float _maxHp;

    void Start()
    {
        _hpBar = GameObject.Find("Fuel_Image").GetComponent<Image>();
        _maxHp = _hp;
    }

    void Update()
    {
        UpdateFuelUI();
    }
    /// <summary>
    /// 연료량 출력 함수(수정필요)
    /// </summary>
    private void UpdateFuelUI()
    {
        if (_hpBar != null)
        {
            var fuel_percentage = _hp / _maxHp;
            _hpBar.fillAmount = fuel_percentage;
            _hpBar.color = new Color(1, fuel_percentage, fuel_percentage);

        }
    }
    public void Damaged(float amount)
    {
        _hp = Mathf.Max(0, _hp - amount);
    }
    public void Repaired(float amount)
    {
        _hp = Mathf.Min(_maxHp, _hp + amount);
    }
    public void RepairedPercent(float percent)
    {
        _hp = Mathf.Min(_maxHp, _hp + (_maxHp * percent));
    }
    public bool IsAlive()
    {
        return _hp > 0;
    }
}
