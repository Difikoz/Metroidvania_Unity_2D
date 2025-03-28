using UnityEngine;

namespace WinterUniverse
{
    public class PlayerHUD : BasicComponent
    {
        [SerializeField] private VitalityBarUI _healthBar;
        [SerializeField] private VitalityBarUI _staminaBar;

        public override void Enable()
        {
            base.Enable();
            GameManager.StaticInstance.Player.Status.OnHealthChanged += _healthBar.SetValues;
            GameManager.StaticInstance.Player.Status.OnStaminaChanged += _staminaBar.SetValues;
        }

        public override void Disable()
        {
            GameManager.StaticInstance.Player.Status.OnHealthChanged -= _healthBar.SetValues;
            GameManager.StaticInstance.Player.Status.OnStaminaChanged -= _staminaBar.SetValues;
            base.Disable();
        }
    }
}