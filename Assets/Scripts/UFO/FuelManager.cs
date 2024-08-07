using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    public float _fuel;
    [SerializeField] private float _fuelConsumption;
    public float FuelConsumption => _fuelConsumption;
    private Image _fuelBar;
    private float _maxFuel;

    void Start()
    {
        _fuelBar = GameObject.Find("Fuel_Image").GetComponent<Image>();
        _maxFuel = _fuel;
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
        {
            var fuel_percentage = _fuel / _maxFuel;
            _fuelBar.fillAmount = fuel_percentage;
            _fuelBar.color = new Color(1, fuel_percentage, fuel_percentage);

        }
    }
    public void UseFuel(float amount)
    {
        _fuel = Mathf.Max(0, _fuel - amount);
    }
    public void AddFuel(float amount)
    {
        _fuel = Mathf.Min(_maxFuel, _fuel + amount);
    }
    public void AddFuelPercent(float percent)
    {
        _fuel = Mathf.Min(_maxFuel, _fuel + (_maxFuel * percent));
    }
    public bool CanAccel()
    {
        return _fuel > 0;
    }
}
