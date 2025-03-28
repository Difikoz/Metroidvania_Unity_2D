using UnityEngine;

namespace WinterUniverse
{
    public class PlayerInputComponent : PlayerComponent
    {
        private PlayerInputActions _inputActions;
        private bool _dashPerfomed;
        private bool _jumpPerfomed;
        private bool _jumpCanceled;
        private bool _interactPerfomed;
        private bool _primaryAttackPerfomed;
        private bool _primaryAttackCanceled;
        private bool _secondaryAttackPerfomed;
        private bool _secondaryAttackCanceled;

        public Vector2 MoveInput { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            _inputActions = new();
        }

        public override void Enable()
        {
            base.Enable();
            _inputActions.Enable();
            _inputActions.Player.Dash.performed += ctx => _dashPerfomed = true;
            _inputActions.Player.Jump.performed += ctx => _jumpPerfomed = true;
            _inputActions.Player.Jump.canceled += ctx => _jumpCanceled = true;
            _inputActions.Player.Interact.performed += ctx => _interactPerfomed = true;
            _inputActions.Player.PrimaryAttack.performed += ctx => _primaryAttackPerfomed = true;
            _inputActions.Player.PrimaryAttack.canceled += ctx => _primaryAttackCanceled = true;
            _inputActions.Player.SecondaryAttack.performed += ctx => _secondaryAttackPerfomed = true;
            _inputActions.Player.SecondaryAttack.canceled += ctx => _secondaryAttackCanceled = true;
        }

        public override void Disable()
        {
            _inputActions.Player.Dash.performed -= ctx => _dashPerfomed = true;
            _inputActions.Player.Jump.performed -= ctx => _jumpPerfomed = true;
            _inputActions.Player.Jump.canceled -= ctx => _jumpCanceled = true;
            _inputActions.Player.Interact.performed -= ctx => _interactPerfomed = true;
            _inputActions.Player.PrimaryAttack.performed -= ctx => _primaryAttackPerfomed = true;
            _inputActions.Player.PrimaryAttack.canceled -= ctx => _primaryAttackCanceled = true;
            _inputActions.Player.SecondaryAttack.performed -= ctx => _secondaryAttackPerfomed = true;
            _inputActions.Player.SecondaryAttack.canceled -= ctx => _secondaryAttackCanceled = true;
            _inputActions.Disable();
            base.Disable();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void OnFixedUpdate()
        {
            if (GameManager.StaticInstance.InputMode == InputMode.UI)
            {
                MoveInput = Vector2.zero;
                return;
            }
            MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            if(_dashPerfomed)
            {
                _dashPerfomed = false;
                _player.Locomotion.PerformDash();
            }
            if (_jumpPerfomed)
            {
                _jumpPerfomed = false;
                _player.Locomotion.PerformJump();
            }
            if (_jumpCanceled)
            {
                _jumpCanceled = false;
                _player.Locomotion.CancelJump();
            }
            if (_interactPerfomed)
            {
                _interactPerfomed = false;
            }
            if (_primaryAttackPerfomed)
            {
                _primaryAttackPerfomed = false;
                _player.Combat.PerformPrimaryAttack();
            }
            if (_primaryAttackCanceled)
            {
                _primaryAttackCanceled = false;
                _player.Combat.CancelPrimaryAttack();
            }
            if (_secondaryAttackPerfomed)
            {
                _secondaryAttackPerfomed = false;
                _player.Combat.PerformSecondaryAttack();
            }
            if (_secondaryAttackCanceled)
            {
                _secondaryAttackCanceled = false;
                _player.Combat.CancelSecondaryAttack();
            }
            base.OnFixedUpdate();
        }
    }
}