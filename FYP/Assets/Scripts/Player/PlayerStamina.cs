using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float dashStaminaCost = 25f;  // 每次冲刺消耗的精力
    [SerializeField] private float staminaRegenRate = 15f; // 每秒恢复的精力
    [SerializeField] private float regenDelay = 1f;        // 使用精力后开始恢复的延迟时间

    [Header("UI Reference")]
    [SerializeField] private StaminaBar staminaBar;

    private float currentStamina;
    private float lastStaminaUseTime;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    public float DashStaminaCost => dashStaminaCost;

    private void Start()
    {
        currentStamina = maxStamina;
        if (staminaBar != null)
        {
            staminaBar.SetMaxStamina(maxStamina);
        }
    }

    private void Update()
    {
        RegenerateStamina();
    }

    /// <summary>
    /// 恢复精力
    /// </summary>
    private void RegenerateStamina()
    {
        // 延迟一段时间后才开始恢复精力
        if (Time.time - lastStaminaUseTime < regenDelay)
            return;

        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdateStaminaBar();
        }
    }

    /// <summary>
    /// 检查是否有足够的精力进行冲刺
    /// </summary>
    public bool HasEnoughStaminaToDash()
    {
        return currentStamina >= dashStaminaCost;
    }

    /// <summary>
    /// 使用精力进行冲刺
    /// </summary>
    public void UseStaminaForDash()
    {
        currentStamina -= dashStaminaCost;
        currentStamina = Mathf.Max(currentStamina, 0);
        lastStaminaUseTime = Time.time;
        UpdateStaminaBar();
    }

    /// <summary>
    /// 使用指定数量的精力
    /// </summary>
    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Max(currentStamina, 0);
            lastStaminaUseTime = Time.time;
            UpdateStaminaBar();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 恢复指定数量的精力
    /// </summary>
    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaBar();
    }

    /// <summary>
    /// 完全恢复精力
    /// </summary>
    public void RestoreFullStamina()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    /// <summary>
    /// 更新精力条UI
    /// </summary>
    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.SetStamina(currentStamina, maxStamina);
        }
    }

    /// <summary>
    /// 设置精力条UI引用
    /// </summary>
    public void SetStaminaBar(StaminaBar bar)
    {
        staminaBar = bar;
        if (staminaBar != null)
        {
            staminaBar.SetMaxStamina(maxStamina);
            UpdateStaminaBar();
        }
    }
}
