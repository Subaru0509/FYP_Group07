using UnityEngine;

/// <summary>
/// Boss战触发区域 - 放置在Boss房间入口
/// 玩家进入时触发Boss战
/// </summary>
public class BossTriggerArea : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private bool triggerOnce = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered && triggerOnce) return;
        
        if (collision.CompareTag("Player"))
        {
            if (boss != null)
            {
                boss.StartBossBattle();
                hasTriggered = true;
            }
        }
    }

    /// <summary>
    /// 重置触发器（用于重新开始战斗）
    /// </summary>
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
