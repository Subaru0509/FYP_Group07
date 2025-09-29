using Assets.PixelFantasy.Common.Scripts;
using UnityEngine;

namespace Assets.PixelFantasy.PixelMonsters.Common.Scripts
{
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(Animator))]
    public class InanimateControls : MonoBehaviour
    {
        private Creature _creature;
        private Animator _animator;

        public void Start()
        {
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _animator.SetBool("Idle", true);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SetState(idle: true);
            }
            else if (Input.GetKeyDown(KeyCode.D) && _animator.HasState(0, Animator.StringToHash("Destroy")))
            {
                SetState(destroy: true);
                EffectManager.Instance.CreateSpriteEffect(_creature, "Fall");
            }
            else if (Input.GetKeyDown(KeyCode.O) && _animator.HasState(0, Animator.StringToHash("Open")))
            {
                SetState(open: true);
                EffectManager.Instance.CreateSpriteEffect(_creature, "Fall");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                EffectManager.Instance.Blink(_creature);
            }
        }

        private void SetState(bool idle = false, bool destroy = false, bool open = false)
        {
            _animator.SetBool("Idle", idle);
            _animator.SetBool("Destroy", destroy);
            _animator.SetBool("Open", open);
        }
    }
}