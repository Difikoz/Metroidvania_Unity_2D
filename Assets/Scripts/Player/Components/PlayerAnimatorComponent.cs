using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimatorComponent : PlayerComponent
    {
        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Animator.SetBool("Is Moving", _player.Locomotion.IsMoving);
            Animator.SetFloat("Fall Velocity", _player.Locomotion.FallVelocity);
        }

        public void PlayAction(string name, float fadeTime = 0f)
        {
            Animator.CrossFade(name, fadeTime);
        }
    }
}