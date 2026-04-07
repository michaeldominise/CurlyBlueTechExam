using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerPickController chargeSource;
    [SerializeField] private Slider slider;

    [Header("Color")]
    [SerializeField] private Image fillImage; // optional (for color)
    [SerializeField] private Gradient colorGradient;
    
    [Header("Label")]
    [SerializeField] private TextMeshProUGUI valueText;

    private void OnEnable()
    {
        if (chargeSource == null) return;

        chargeSource.OnChargeStart += HandleStart;
        chargeSource.OnChargeChanged += HandleChanged;
        chargeSource.OnChargeEnd += HandleEnd;
    }

    private void OnDisable()
    {
        if (chargeSource == null) return;

        chargeSource.OnChargeStart -= HandleStart;
        chargeSource.OnChargeChanged -= HandleChanged;
        chargeSource.OnChargeEnd -= HandleEnd;
    }

    private void HandleStart()
    {
        if (slider == null) return;

        slider.gameObject.SetActive(true);
        slider.value = 0f;

        UpdateColor(0f);
    }

    private void HandleChanged(float value)
    {
        if (slider == null) return;

        slider.value = value;
        valueText.text = $"{value * 100:N0}%";
        UpdateColor(value);
    }

    private void HandleEnd()
    {
        if (slider == null) return;

        
    }

    private void UpdateColor(float value)
    {
        if (fillImage == null) return;

        fillImage.color = colorGradient.Evaluate(value);
    }
}