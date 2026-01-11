using System.Linq;
using UnityEngine;

namespace Assets.PixelFantasy.PixelMonsters.Common.Scripts.ExampleScripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MonsterAnimation))]
    public class MonsterController2D : MonoBehaviour
    {
        public Vector2 Input;
        public bool IsGrounded;

        public float Acceleration = 40;
        public float MaxSpeed = 8;
        public float JumpForce = 1000;
        public float Gravity = 70;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private MonsterAnimation _animation;

        private bool _jump;

        public void Start()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animation = GetComponent<MonsterAnimation>();
        }

        public void FixedUpdate()
        {
            var state = _animation.GetState();

            if (state == MonsterState.Die) return;
            
            var velocity = _rigidbody.velocity;

            if (Input.x == 0)
            {
                if (IsGrounded)
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, 0, Acceleration * 3 * Time.fixedDeltaTime);
                }
            }
            else
            {
                var maxSpeed = MaxSpeed;
                var acceleration = Acceleration;

                if (_jump)
                {
                    acceleration /= 2;
                }

                velocity.x = Mathf.MoveTowards(velocity.x, Input.x * maxSpeed, acceleration * Time.fixedDeltaTime);
                Turn(velocity.x);
            }
            
            if (IsGrounded)
            {
                if (!_jump)
                {
                    if (Input.x == 0)
                    {
                        _animation.Ready();
                    }
                    else
                    {
                        _animation.Run();
                    }
                }

                if (Input.y > 0 && !_jump)
                {
                    _jump = true;
                    _rigidbody.AddForce(Vector2.up * JumpForce);
                    _animation.Jump();
                }
            }
            else
            {
                velocity.y -= Gravity * Time.fixedDeltaTime;

                if (velocity.y < 0)
                {
                    _jump = true;
                    _animation.Fall();
                }
            }

            _rigidbody.velocity = velocity;
        }

        private void Turn(float direction)
        {
            var scale = transform.localScale;

            scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);

            transform.localScale = scale;
        }

        private Collider2D _ground;

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.All(i => i.point.y <= _collider.bounds.min.y + 0.1f))
            {
                IsGrounded = true;
                _ground = collision.collider;

                if (_jump)
                {
                    _jump = false;
                    _animation.Land();
                }
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (IsGrounded && collision.collider == _ground)
            {
                IsGrounded = false;
            }
        }
    }
}