using UnityEngine;

namespace WinterUniverse
{
    public class UIManager : BasicComponent
    {
        public PlayerHUD PlayerHUD { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            PlayerHUD = GetComponentInChildren<PlayerHUD>();
            PlayerHUD.Initialize();
        }

        public override void Destroy()
        {
            PlayerHUD.Destroy();
            base.Destroy();
        }

        public override void Enable()
        {
            base.Enable();
            PlayerHUD.Enable();
        }

        public override void Disable()
        {
            PlayerHUD.Disable();
            base.Disable();
        }
    }
}