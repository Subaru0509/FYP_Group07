using UnityEngine;

public class Player_AnimationTriggers : MonoBehaviour
{   
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }


    private void CurrentStateTrigger()
    {
        player.CallAnimationTrigger();
    }
}
