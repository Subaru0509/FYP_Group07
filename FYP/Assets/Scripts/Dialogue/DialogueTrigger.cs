using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogueData;

    [Header("Camera Focus")]
    [SerializeField] private Transform focusTarget;

    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool autoTrigger = false;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactDistance = 2f;  // 交互距离

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt;

    private bool hasTriggered = false;
    private bool playerInRange = false;
    private Transform player;

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (focusTarget == null)
            focusTarget = transform;

        // 查找玩家
        Player playerComponent = FindObjectOfType<Player>();
        if (playerComponent != null)
            player = playerComponent.transform;
    }

    private void Update()
    {
        if (player == null) return;

        // 检测距离
        float distance = Vector2.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactDistance;

        // 进入范围
        if (playerInRange && !wasInRange)
        {
            OnPlayerEnterRange();
        }
        // 离开范围
        else if (!playerInRange && wasInRange)
        {
            OnPlayerExitRange();
        }

        // 按键检测
        if (playerInRange && !autoTrigger && Input.GetKeyDown(interactKey))
        {
            if (DialogueSystem.Instance != null && !DialogueSystem.Instance.IsInDialogue)
            {
                Debug.Log("E pressed, starting dialogue");
                TriggerDialogue();
            }
        }
    }

    private void OnPlayerEnterRange()
    {
        if (triggerOnce && hasTriggered) return;

        Debug.Log("Player entered range");

        if (autoTrigger)
        {
            TriggerDialogue();
        }
        else if (interactPrompt != null)
        {
            interactPrompt.SetActive(true);
        }
    }

    private void OnPlayerExitRange()
    {
        Debug.Log("Player exited range");
        
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    public void TriggerDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("DialogueData is not assigned!");
            return;
        }
        if (DialogueSystem.Instance == null)
        {
            Debug.LogWarning("DialogueSystem.Instance is null!");
            return;
        }
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        DialogueSystem.Instance.StartDialogue(dialogueData, focusTarget);
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }

    // 显示交互范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
