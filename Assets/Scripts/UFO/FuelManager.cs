using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    [SerializeField] private float _fuel;
    [SerializeField] private float _fuelConsumption;
    public float FuelConsumption => _fuelConsumption;
    private Image _fuelBar;

    void Start()
    {
        //_fuelBar = GameObject.Find("FuelBar").GetComponent<Image>();
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
        if (_fuelBar != null)
            _fuelBar.fillAmount = _fuel / 100f;
    }
    public void UseFuel(float amount)
    {
        _fuel = Mathf.Max(0, _fuel - amount);
    }
    public void AddFuel(float amount)
    {
        _fuel = Mathf.Min(100, _fuel + amount);
    }
    public bool CanAccel()
    {
        return _fuel > 0;
    }
}
