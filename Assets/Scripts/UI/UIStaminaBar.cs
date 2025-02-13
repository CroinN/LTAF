using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void UpdateStaminaAmount(float currentAmount, float maxAmount)
    {
        float value = currentAmount / maxAmount;
        Assert.IsTrue(value >= 0 && value <= 1, "Stamina value is not between 0 and 1");
        _slider.value = value;
    }
}
