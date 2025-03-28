using UnityEngine;

namespace WinterUniverse
{
    public class PlayerCombatComponent : PlayerComponent
    {
        public bool IsPerfomingPrimaryAttack { get; private set; }
        public bool IsPerfomingSecondaryAttack { get; private set; }

        public void PerformPrimaryAttack()
        {
            if (IsPerfomingPrimaryAttack || IsPerfomingSecondaryAttack)
            {
                return;
            }
            IsPerfomingPrimaryAttack = true;
            Debug.Log($"Primary: {IsPerfomingPrimaryAttack}\nSecondary: {IsPerfomingSecondaryAttack}");
        }

        public void CancelPrimaryAttack()
        {
            if (!IsPerfomingPrimaryAttack)
            {
                return;
            }
            IsPerfomingPrimaryAttack = false;
            Debug.Log($"Primary: {IsPerfomingPrimaryAttack}\nSecondary: {IsPerfomingSecondaryAttack}");
        }

        public void PerformSecondaryAttack()
        {
            if (IsPerfomingPrimaryAttack || IsPerfomingSecondaryAttack)
            {
                return;
            }
            IsPerfomingSecondaryAttack = true;
            Debug.Log($"Primary: {IsPerfomingPrimaryAttack}\nSecondary: {IsPerfomingSecondaryAttack}");
        }

        public void CancelSecondaryAttack()
        {
            if (!IsPerfomingSecondaryAttack)
            {
                return;
            }
            IsPerfomingSecondaryAttack = false;
            Debug.Log($"Primary: {IsPerfomingPrimaryAttack}\nSecondary: {IsPerfomingSecondaryAttack}");
        }
    }
}