using Assets.PixelFantasy.Common.Scripts;
using UnityEngine;

namespace Assets.PixelFantasy.PixelMonsters.Common.Scripts.ExampleScripts
{
    [RequireComponent(typeof(Monster))]
    [RequireComponent(typeof(MonsterController2D))]
    [RequireComponent(typeof(MonsterAnimation))]
    public class MonsterControls : MonoBehaviour
    {
        private Monster _monster;
        private MonsterController2D _controller;
        private MonsterAnimation _animation;

        public void Start()
        {
            _monster = GetComponent<Monster>();
            _controller = GetComponent<MonsterController2D>();
            _animation = GetComponent<MonsterAnimation>();
        }

        public void Update()
        {
            Move();
            Attack();

            // Play other animations, just for example.
            if (Input.GetKeyDown(KeyCode.I)) { _animation.SetState(MonsterState.Idle); }
            if (Input.GetKeyDown(KeyCode.R)) { _animation.SetState(MonsterState.Ready); }
            if (Input.GetKeyDown(KeyCode.D)) _animation.SetState(MonsterState.Die);
            if (Input.GetKeyUp(KeyCode.H)) _animation.Hit();
            if (Input.GetKeyUp(KeyCode.L)) EffectManager.Instance.Blink(_monster);
        }

        private void Move()
        {
            _controller.Input = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _controller.Input.x = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _controller.Input.x = 1;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _controller.Input.y = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _controller.Input.y = -1;
            }
        }

        private void Attack()
        {
            if (Input.GetKeyDown(KeyCode.A)) _animation.Attack();
            if (Input.GetKeyDown(KeyCode.F)) _animation.Fire();
        }
    }
}