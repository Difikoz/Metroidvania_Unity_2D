using UnityEngine;

namespace WinterUniverse
{
    public abstract class PlayerComponent : BasicComponent
    {
        protected PlayerController _player;

        public override void Initialize()
        {
            base.Initialize();
            _player = GetComponent<PlayerController>();
        }
    }
}