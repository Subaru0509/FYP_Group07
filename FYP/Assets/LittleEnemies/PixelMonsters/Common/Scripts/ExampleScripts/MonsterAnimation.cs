using System;
using Assets.PixelFantasy.Common.Scripts;
using UnityEngine;

namespace Assets.PixelFantasy.PixelMonsters.Common.Scripts.ExampleScripts
{
    [RequireComponent(typeof(Monster))]
    public class MonsterAnimation : MonoBehaviour
    {
        private Monster _monster;

        public void Start()
        {
            _monster = GetComponent<Monster>();
        }

        public void SetState(MonsterState state)
        {
            foreach (var variable in new[] { "Idle", "Ready", "Walk", "Run", "Jump", "Die" })
            {
                _monster.Animator.SetBool(variable, false);
            }

            switch (state)
            {
                case MonsterState.Idle: _monster.Animator.SetBool("Idle", true); break;
                case MonsterState.Ready: _monster.Animator.SetBool("Ready", true); break;
                case MonsterState.Walk: _monster.Animator.SetBool("Walk", true); break;
                case MonsterState.Run: _monster.Animator.SetBool("Run", true); break;
                case MonsterState.Jump: _monster.Animator.SetBool("Jump", true); break;
                case MonsterState.Die: _monster.Animator.SetBool("Die", true); break;
                default: throw new NotSupportedException();
            }

            //Debug.Log("SetState: " + state);
        }

        public MonsterState GetState()
        {
            if (_monster.Animator.GetBool("Idle")) return MonsterState.Idle;
            if (_monster.Animator.GetBool("Ready")) return MonsterState.Ready;
            if (_monster.Animator.GetBool("Walk")) return MonsterState.Walk;
            if (_monster.Animator.GetBool("Run")) return MonsterState.Run;
            if (_monster.Animator.GetBool("Jump")) return MonsterState.Jump;
            if (_monster.Animator.GetBool("Die")) return MonsterState.Die;

            return MonsterState.Ready;
        }

        public void Idle()
        {
            SetState(MonsterState.Idle);
        }

        public void Ready()
        {
            if (GetState() == MonsterState.Walk)
            {
                EffectManager.Instance.CreateSpriteEffect(_monster, "Brake");
            }
            else if (GetState() == MonsterState.Idle)
            {
                return;
            }

            SetState(MonsterState.Ready);
        }

        public void Run()
        {
            if (GetState() != MonsterState.Walk)
            {
                EffectManager.Instance.CreateSpriteEffect(_monster, "Run");
            }

            SetState(MonsterState.Walk);
        }

        public void Jump()
        {
            EffectManager.Instance.CreateSpriteEffect(_monster, "Jump");
            SetState(MonsterState.Run);
        }

        public void Fall()
        {
            SetState(MonsterState.Run);
        }

        public void Land()
        {
            EffectManager.Instance.CreateSpriteEffect(_monster, "Fall");
        }
        
        public void Die()
        {
            SetState(MonsterState.Die);
        }
        
        public void Attack()
        {
            _monster.Animator.SetTrigger("Attack");
        }

        public void Fire()
        {
            _monster.Animator.SetTrigger("Fire");
        }

        public void Hit()
        {
            _monster.Animator.SetTrigger("Hit");
        }
    }
}