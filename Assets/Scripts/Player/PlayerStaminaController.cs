using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStaminaController : MonoBehaviour
{
    [Serializable]
    public struct StaminaStats
    {
        public float maxStamina;
        public float staminaRegenerationAmount;
        public float jumpStaminaCost;
        public float dashStaminaCost;
    }

    [SerializeField] private StaminaStats _staminaStats;

    private UIManager _uiManager;
    private float _currentStamina;

    public bool CanJump => HasEnoughStamina(_staminaStats.jumpStaminaCost);
    public bool CanDash => HasEnoughStamina(_staminaStats.dashStaminaCost);

    private void Awake() 
    {
        _currentStamina = _staminaStats.maxStamina;
    }

    private void Start()
    {
        _uiManager = SL.Get<UIManager>();
        _uiManager.UpdateStamina(_currentStamina, _staminaStats.maxStamina);
    }

    private void FixedUpdate()
    {
        RegenerateStamina();
    }

    public void OnJump()
    {
        ChangeStamina(-_staminaStats.jumpStaminaCost);
    }

    public void OnDash()
    {
        ChangeStamina(-_staminaStats.dashStaminaCost);
    }

    private void RegenerateStamina()
    {
        if (_currentStamina < _staminaStats.maxStamina)
        {
            float staminaRegenAmount = _staminaStats.staminaRegenerationAmount * Time.deltaTime;
            staminaRegenAmount = Mathf.Clamp(staminaRegenAmount, 0, _staminaStats.maxStamina - _currentStamina);
            ChangeStamina(staminaRegenAmount);
        }
    }

    private void ChangeStamina(float amount)
    {
        float newStamina = _currentStamina + amount;
        Assert.IsTrue(newStamina >= 0 && newStamina <= _staminaStats.maxStamina, "Stamina is out of bounds");
        _currentStamina = Mathf.Clamp(newStamina, 0, _staminaStats.maxStamina);
        _uiManager.UpdateStamina(_currentStamina, _staminaStats.maxStamina);
    }

    private bool HasEnoughStamina(float amount)
    {
        return _currentStamina >= amount;
    }
}
