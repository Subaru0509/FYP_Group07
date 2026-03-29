using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class CustomOutline : MonoBehaviour
{
    [Header("描边")]
    public bool openOutline = true;
    public Color outlineColor = Color.black;
    public float outlineWidth = 0.2f;

    [Header("底色/阴影")]
    public bool openUnderlay = false;
    public Color underlayColor = new Color(0f, 0f, 0f, 1f); // 默认黑，作为底色/阴影
    public float underlayDilation = 1f; // 模糊/膨胀度

    private TMP_Text _tmp;
    private Material _materialInstance; // 使用实例化的材质，避免污染原材质

    private void Awake()
    {
        _tmp = GetComponent<TMP_Text>();

        // 关键：创建材质实例，而不是直接修改共享材质
        // 这样不会影响场景中其他使用相同字体的文本
        _materialInstance = new Material(_tmp.fontMaterial);
        _tmp.fontMaterial = _materialInstance;
    }

    private void Start()
    {
        UpdateMaterial();
    }

    private void OnValidate()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (_materialInstance == null)
            return;

        // --- 描边控制 ---
        if (openOutline)
        {
            _materialInstance.SetColor("_OutlineColor", outlineColor);
            _materialInstance.SetFloat("_OutlineWidth", outlineWidth);
            _materialInstance.EnableKeyword("OUTLINE_ON"); // 启用描边Shader功能
        }
        else
        {
            _materialInstance.SetColor("_OutlineColor", Color.clear); // 颜色设为透明
            _materialInstance.SetFloat("_OutlineWidth", 0);
            _materialInstance.DisableKeyword("OUTLINE_ON");
        }

        // --- 底色/阴影控制 ---
        if (openUnderlay)
        {
            _materialInstance.SetColor("_UnderlayColor", underlayColor);
            _materialInstance.SetFloat("_UnderlayDilate", underlayDilation);
            _materialInstance.EnableKeyword("UNDERLAY_ON"); // 启用底色/阴影Shader功能
        }
        else
        {
            _materialInstance.SetColor("_UnderlayColor", Color.clear);
            _materialInstance.DisableKeyword("UNDERLAY_ON");
        }
    }

    // 可选：在编辑器失去焦点或脚本重载时清理，防止内存溢出（视项目情况而定）
    private void OnDestroy()
    {
        if (_materialInstance != null && Application.isPlaying)
        {
            Destroy(_materialInstance);
        }
    }
}