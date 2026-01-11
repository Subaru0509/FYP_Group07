using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private GameObject continueIndicator;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTypingEffect = true;

    [Header("Camera Settings")]
    [SerializeField] private float cameraZoomSize = 3f;
    [SerializeField] private float cameraTransitionSpeed = 2f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1f, -10f);

    private Camera mainCamera;
    private float originalCameraSize;
    private Vector3 originalCameraPosition;
    private Transform originalCameraParent;
    private Transform currentFocusTarget;
    private bool isInDialogue = false;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private Coroutine typingCoroutine;
    private Coroutine cameraCoroutine;

    private Player player;

    public bool IsInDialogue => isInDialogue;
    public event System.Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        player = FindObjectOfType<Player>();

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isInDialogue) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    /// <summary>
    /// 开始对话
    /// </summary>
    public void StartDialogue(DialogueData dialogueData, Transform focusTarget = null)
    {
        if (isInDialogue) return;

        isInDialogue = true;
        dialogueQueue.Clear();

        foreach (var line in dialogueData.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        // 暂停玩家控制
        if (player != null)
        {
            player.SetVelocity(0, 0);
            player.enabled = false;
        }

        // 保存原始摄像机状态
        originalCameraSize = mainCamera.orthographicSize;
        originalCameraPosition = mainCamera.transform.position;
        originalCameraParent = mainCamera.transform.parent;

        // 对话时摄像机脱离跟随
        mainCamera.transform.SetParent(null);

        // 聚焦目标
        if (focusTarget != null)
        {
            currentFocusTarget = focusTarget;
            FocusCameraOnTarget(focusTarget);
        }

        dialoguePanel.SetActive(true);
        DisplayNextLine();
    }

    /// <summary>
    /// 显示下一句对话
    /// </summary>
    private void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();

        // 更新说话者名字
        if (speakerNameText != null)
            speakerNameText.text = line.speakerName;

        // 更新头像
        if (speakerPortrait != null)
        {
            if (line.portrait != null)
            {
                speakerPortrait.sprite = line.portrait;
                speakerPortrait.gameObject.SetActive(true);
            }
            else
            {
                speakerPortrait.gameObject.SetActive(false);
            }
        }

        // 如果有新的聚焦目标，切换摄像机
        if (line.focusTarget != null && line.focusTarget != currentFocusTarget)
        {
            currentFocusTarget = line.focusTarget;
            FocusCameraOnTarget(line.focusTarget);
        }

        // 显示对话内容
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    /// <summary>
    /// 打字机效果
    /// </summary>
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        if (useTypingEffect)
        {
            foreach (char c in text)
            {
                if (skipTyping)
                {
                    dialogueText.text = text;
                    break;
                }

                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else
        {
            dialogueText.text = text;
        }

        isTyping = false;

        if (continueIndicator != null)
            continueIndicator.SetActive(true);
    }

    /// <summary>
    /// 聚焦摄像机到目标
    /// </summary>
    private void FocusCameraOnTarget(Transform target)
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(MoveCameraToTarget(target));
    }

    /// <summary>
    /// 摄像机移动协程
    /// </summary>
    private IEnumerator MoveCameraToTarget(Transform target)
    {
        Vector3 targetPosition = target.position + cameraOffset;

        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.05f ||
               Mathf.Abs(mainCamera.orthographicSize - cameraZoomSize) > 0.05f)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                cameraTransitionSpeed * Time.deltaTime
            );

            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize,
                cameraZoomSize,
                cameraTransitionSpeed * Time.deltaTime
            );

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = cameraZoomSize;
    }

    /// <summary>
    /// 结束对话
    /// </summary>
    private void EndDialogue()
    {
        isInDialogue = false;
        dialoguePanel.SetActive(false);

        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(ResetCamera());

        // 恢复玩家控制
        if (player != null)
        {
            player.enabled = true;
        }

        OnDialogueEnd?.Invoke();
    }

    /// <summary>
    /// 恢复摄像机
    /// </summary>
    private IEnumerator ResetCamera()
    {
        while (Vector3.Distance(mainCamera.transform.position, originalCameraPosition) > 0.05f ||
               Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.05f)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                originalCameraPosition,
                cameraTransitionSpeed * Time.deltaTime
            );

            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize,
                originalCameraSize,
                cameraTransitionSpeed * Time.deltaTime
            );

            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
        mainCamera.orthographicSize = originalCameraSize;

        // 恢复摄像机父对象
        if (originalCameraParent != null)
            mainCamera.transform.SetParent(originalCameraParent);
    }
}
