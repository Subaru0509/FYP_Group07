using System;
using UnityEngine;

namespace Assets.PixelFantasy.Common.Scripts
{
    /// <summary>
    /// Used to block animation transitions.
    /// </summary>
    public class SoloState : StateMachineBehaviour
    {
        public bool Continuous;
        public bool Active;
        public Func<bool> Continue;

        private float _enterTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enterTime = Time.time;
            animator.SetBool("Action", true);
            Active = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime >= 1 && !Continuous)
            {
                Exit(animator, stateInfo);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Exit(animator, stateInfo);
        }

        private void Exit(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!Active || Time.time - _enterTime < stateInfo.length) return;

            Active = false;

            if (Continue == null)
            {
                animator.SetBool("Action", false);
            }
            else
            {
                if (!Continue())
                {
                    animator.SetBool("Action", false);
                }

                Continue = null;
            }
        }
    }
}