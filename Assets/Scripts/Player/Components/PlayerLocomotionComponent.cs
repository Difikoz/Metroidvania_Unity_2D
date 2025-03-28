using System;
using UnityEngine;

namespace WinterUniverse
{
    public class PlayerLocomotionComponent : PlayerComponent
    {
        public Action OnStartMove;
        public Action OnStopMove;
        public Action OnDash;
        public Action OnJump;
        public Action OnLand;

        public float MaxMoveSpeed = 10f;
        public float DashForce = 20f;
        public float DashStaminaCost = 10f;
        public float JumpForce = 10f;
        public float JumpStaminaCost = 10f;
        public float Gravity = -10f;
        public float GravityJumpMultiplier = 1f;
        public float GravityFallMultiplier = 2f;
        public float MaxFallSpeed = 40f;
        public int JumpCount = 1;
        public float TimeToDash = 0.5f;
        public float TimeToJump = 0.25f;
        public float TimeToFall = 0.25f;
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Transform _wallCheckPoint;
        [SerializeField] private float _wallCheckRadius;
        [SerializeField] private Transform _ceilingCheckPoint;
        [SerializeField] private float _ceilingCheckRadius;
        [SerializeField] private Transform _vaultCheckPoint;
        [SerializeField] private float _vaultCheckRadius;
        public LayerMask ObstacleMask;

        private Collider2D _groundCollider;
        private Collider2D _wallCollider;
        private Collider2D _ceilingCollider;
        private Collider2D _vaultCollider;
        private int _jumpCount;
        private float _jumpTime;
        private float _groundedTime;

        public float GroundVelocity { get; private set; }
        public float DashVelocity { get; private set; }
        public float FallVelocity { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsDashing { get; private set; }
        public bool IsFacedToWall { get; private set; }
        public bool IsFacingRight { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            IsFacingRight = true;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            HandleGravity();
            HandleMovement();
            HandleDashing();
            HandleRotation();
            _player.RB.linearVelocity = new(GroundVelocity + DashVelocity, FallVelocity);
        }

        private void HandleGravity()
        {
            _groundCollider = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, ObstacleMask);
            _ceilingCollider = Physics2D.OverlapCircle(_ceilingCheckPoint.position, _ceilingCheckRadius, ObstacleMask);
            if (_jumpTime > 0f)
            {
                if (_groundedTime > 0f)
                {
                    ApplyJumpForce();
                }
                _jumpTime -= Time.fixedDeltaTime;
            }
            if (IsDashing)
            {
                return;
            }
            if (IsGrounded)
            {
                if (_groundCollider == null || FallVelocity > 0f)
                {
                    IsGrounded = false;
                    if (FallVelocity <= 0f)
                    {
                        _player.Animator.PlayAction("Fall");
                    }
                    //...
                }
            }
            else
            {
                if (_groundedTime > 0f)
                {
                    _groundedTime -= Time.fixedDeltaTime;
                }
                if (FallVelocity > 0f)
                {
                    if (_ceilingCollider != null)
                    {
                        FallVelocity = 0f;
                    }
                    else
                    {
                        FallVelocity += Gravity * GravityJumpMultiplier * Time.fixedDeltaTime;
                    }
                }
                else if (FallVelocity > -MaxFallSpeed)
                {
                    FallVelocity += Gravity * GravityFallMultiplier * Time.fixedDeltaTime;
                }
                if (_groundCollider != null && FallVelocity <= 0f)
                {
                    IsGrounded = true;
                    FallVelocity = Gravity / 10f;
                    _groundedTime = TimeToFall;
                    _jumpCount = 0;
                    //...
                    OnLand?.Invoke();
                    _player.Animator.PlayAction("Idle");
                }
            }
        }

        private void HandleMovement()
        {
            _wallCollider = Physics2D.OverlapCircle(_wallCheckPoint.position, _wallCheckRadius, ObstacleMask);
            if (IsFacedToWall)
            {
                if (_wallCollider == null)
                {
                    IsFacedToWall = false;
                    //...
                }
            }
            else
            {
                if (_wallCollider != null)
                {
                    IsFacedToWall = true;
                    //...
                }
            }
            if (IsDashing)
            {
                if (IsFacedToWall)
                {
                    DashVelocity = 0f;
                }
                else
                {
                    return;
                }
            }
            if (_player.Input.MoveInput.x != 0f)
            {
                GroundVelocity = _player.Input.MoveInput.x * MaxMoveSpeed;
                if (!IsMoving)
                {
                    IsMoving = true;
                    //...
                    OnStartMove?.Invoke();
                }
            }
            else
            {
                GroundVelocity = 0f;
                if (IsMoving)
                {
                    IsMoving = false;
                    //...
                    OnStopMove?.Invoke();
                }
            }
        }

        private void HandleDashing()
        {
            if (IsDashing)
            {
                DashVelocity = Mathf.MoveTowards(DashVelocity, 0f, DashForce / TimeToDash * Time.fixedDeltaTime);
                if (DashVelocity == 0f)
                {
                    IsDashing = false;
                    _player.Animator.PlayAction("Idle");
                    //...
                }
            }
            else
            {
                //...
            }
        }

        private void HandleRotation()
        {
            if (IsFacingRight)
            {
                if (GroundVelocity < 0f)
                {
                    IsFacingRight = false;
                    transform.localScale = new(-1f, 1f, 1f);
                    //...
                }
            }
            else
            {
                if (GroundVelocity > 0f)
                {
                    IsFacingRight = true;
                    transform.localScale = new(1f, 1f, 1f);
                    //...
                }
            }
        }

        public void PerformDash()
        {
            if (IsDashing)
            {
                return;
            }
            if(!_player.Status.EnoughStamina(DashStaminaCost))
            {
                return;
            }
            if (_player.Input.MoveInput.x != 0f)
            {
                DashVelocity = _player.Input.MoveInput.normalized.x * DashForce;
            }
            else if (GroundVelocity != 0f)
            {
                DashVelocity = Mathf.Clamp(GroundVelocity, -1f, 1f) * DashForce;
            }
            else if (IsFacingRight)
            {
                DashVelocity = DashForce;
            }
            else
            {
                DashVelocity = -DashForce;
            }
            //...
            IsDashing = true;
            IsMoving = false;
            IsGrounded = false;
            GroundVelocity = 0f;
            FallVelocity = 0f;
            _jumpTime = 0f;
            _groundedTime = 0f;
            _player.Animator.PlayAction("Dash");
            _player.Status.ReduceStamina(DashStaminaCost);
            OnDash?.Invoke();
        }

        public void PerformJump()
        {
            if (CanVault())
            {
                PerformVault();
            }
            else if (_jumpCount < JumpCount)
            {
                ApplyJumpForce();
            }
            else
            {
                _jumpTime = TimeToJump;
            }
        }

        public void CancelJump()
        {
            if (FallVelocity > 0f)
            {
                FallVelocity /= 2f;
            }
        }

        private bool CanVault()
        {
            _vaultCollider = Physics2D.OverlapCircle(_vaultCheckPoint.position, _vaultCheckRadius, ObstacleMask);
            if (_vaultCollider == null)
            {
                return false;
            }
            //Grabbable grabbable = _vaultCollider.GetComponent<IGrabbable>();
            //if (grabbable == null)
            //{
            //    return false;
            //}
            return false;// while W.I.P.
            //return true;
        }

        private void PerformVault()
        {
            //...
            //Grabbable = null;
        }

        private void ApplyJumpForce()
        {
            if (!_player.Status.EnoughStamina(JumpStaminaCost))
            {
                return;
            }
            _jumpTime = 0f;
            if (_ceilingCollider != null || IsDashing)
            {
                return;
            }
            _groundedTime = 0f;
            _jumpCount++;
            FallVelocity = JumpForce;
            _player.Animator.PlayAction("Jump");
            _player.Status.ReduceStamina(JumpStaminaCost);
            OnJump?.Invoke();
        }

        private void OnDrawGizmos()
        {
            if (_groundCheckPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
            }
            if (_wallCheckPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_wallCheckPoint.position, _wallCheckRadius);
            }
            if (_ceilingCheckPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_ceilingCheckPoint.position, _ceilingCheckRadius);
            }
            if (_vaultCheckPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_vaultCheckPoint.position, _vaultCheckRadius);
            }
        }
    }
}