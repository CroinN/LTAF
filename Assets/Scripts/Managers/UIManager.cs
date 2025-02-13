using UnityEngine;

public class UIManager : MonoBehaviour, IService
{
    [SerializeField] private UIStaminaBar _staminaBar;

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    public void UpdateStamina(float currentAmount, float maxAmount)
    {
        _staminaBar.UpdateStaminaAmount(currentAmount, maxAmount);
    }

    public void RegisterService()
    {
        SL.Register(this);
    }

    public void UnregisterService()
    {
        SL.Unregister(this);
    }
}
