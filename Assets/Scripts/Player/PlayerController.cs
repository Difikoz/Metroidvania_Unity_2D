using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerAnimatorComponent))]
    [RequireComponent(typeof(PlayerCombatComponent))]
    [RequireComponent(typeof(PlayerEquipmentComponent))]
    [RequireComponent(typeof(PlayerInputComponent))]
    [RequireComponent(typeof(PlayerLocomotionComponent))]
    [RequireComponent(typeof(PlayerStatusComponent))]
    public class PlayerController : BasicComponent
    {
        public Rigidbody2D RB { get; private set; }
        public PlayerAnimatorComponent Animator { get; private set; }
        public PlayerCombatComponent Combat { get; private set; }
        public PlayerEquipmentComponent Equipment { get; private set; }
        public PlayerInputComponent Input { get; private set; }
        public PlayerLocomotionComponent Locomotion { get; private set; }
        public PlayerStatusComponent Status { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            RB = GetComponent<Rigidbody2D>();
            Animator = GetComponent<PlayerAnimatorComponent>();
            Combat = GetComponent<PlayerCombatComponent>();
            Equipment = GetComponent<PlayerEquipmentComponent>();
            Input = GetComponent<PlayerInputComponent>();
            Locomotion = GetComponent<PlayerLocomotionComponent>();
            Status = GetComponent<PlayerStatusComponent>();
            Animator.Initialize();
            Combat.Initialize();
            Equipment.Initialize();
            Input.Initialize();
            Locomotion.Initialize();
            Status.Initialize();
        }

        public override void Destroy()
        {
            Animator.Destroy();
            Combat.Destroy();
            Equipment.Destroy();
            Input.Destroy();
            Locomotion.Destroy();
            Status.Destroy();
            base.Destroy();
        }

        public override void Enable()
        {
            base.Enable();
            Animator.Enable();
            Combat.Enable();
            Equipment.Enable();
            Input.Enable();
            Locomotion.Enable();
            Status.Enable();
        }

        public override void Disable()
        {
            Animator.Disable();
            Combat.Disable();
            Equipment.Disable();
            Input.Disable();
            Locomotion.Disable();
            Status.Disable();
            base.Disable();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Animator.OnFixedUpdate();
            Combat.OnFixedUpdate();
            Equipment.OnFixedUpdate();
            Input.OnFixedUpdate();
            Locomotion.OnFixedUpdate();
            Status.OnFixedUpdate();
        }
    }
}