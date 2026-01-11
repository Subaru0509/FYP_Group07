using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image fillImage;
    public float smoothSpeed = 5f;

    private float targetFill = 1f;

    void Update()
    {
        if (Mathf.Abs(fillImage.fillAmount - targetFill) > 0.001f)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, smoothSpeed * Time.deltaTime);
        }
        else
        {
            fillImage.fillAmount = targetFill;
        }
    }

    public void SetMaxStamina(float stamina)
    {
        targetFill = 1f;
        fillImage.fillAmount = 1f;
    }

    public void SetStamina(float stamina, float maxStamina)
    {
        targetFill = stamina / maxStamina;
    }
}
