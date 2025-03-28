using System;
using UnityEngine;

namespace WinterUniverse
{
    public class PlayerStatusComponent : PlayerComponent
    {
        public Action<float, float> OnHealthChanged;
        public Action<float, float> OnStaminaChanged;

        [SerializeField] private float _regenerationTickTime = 0.25f;
        [SerializeField] private float _regenerationDelayTime = 5f;
        [Header("Test")]
        [SerializeField] private float testHealthMax = 100f;
        [SerializeField] private float testStaminaMax = 100f;
        [SerializeField] private float testHealthRegeneration = 1f;
        [SerializeField] private float testStaminaRegeneration = 10f;

        private float _healthRegenerationCurrentTickTime;
        private float _healthRegenerationCurrentDelayTime;
        private float _staminaRegenerationCurrentTickTime;
        private float _staminaRegenerationCurrentDelayTime;

        public float HealthCurrent { get; private set; }
        public float HealthMax { get; private set; }
        public float HealthRegeneration { get; private set; }
        public float StaminaCurrent { get; private set; }
        public float StaminaMax { get; private set; }
        public float StaminaRegeneration { get; private set; }
        public bool IsDead { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            HealthMax = testHealthMax;// test
            StaminaMax = testStaminaMax;// test
            HealthRegeneration = testHealthRegeneration;// test
            StaminaRegeneration = testStaminaRegeneration;// test
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (_healthRegenerationCurrentDelayTime >= _regenerationDelayTime)
            {
                if (_healthRegenerationCurrentTickTime >= _regenerationTickTime)
                {
                    _healthRegenerationCurrentTickTime = 0f;
                    RestoreHealth(HealthRegeneration * _regenerationTickTime);
                }
                else
                {
                    _healthRegenerationCurrentTickTime += Time.fixedDeltaTime;
                }
            }
            else
            {
                _healthRegenerationCurrentDelayTime += Time.fixedDeltaTime;
            }
            if (_staminaRegenerationCurrentDelayTime >= _regenerationDelayTime)
            {
                if (_staminaRegenerationCurrentTickTime >= _regenerationTickTime)
                {
                    _staminaRegenerationCurrentTickTime = 0f;
                    RestoreStamina(StaminaRegeneration * _regenerationTickTime);
                }
                else
                {
                    _staminaRegenerationCurrentTickTime += Time.fixedDeltaTime;
                }
            }
            else
            {
                _staminaRegenerationCurrentDelayTime += Time.fixedDeltaTime;
            }
        }

        public void ReduceHealth(float value)
        {
            if (IsDead)
            {
                return;
            }
            _healthRegenerationCurrentDelayTime = 0f;
            HealthCurrent = Mathf.Clamp(HealthCurrent - value, 0f, HealthMax);
            OnHealthChanged?.Invoke(HealthCurrent, HealthMax);
            if (HealthCurrent == 0f)
            {
                Die();
            }
        }

        public void RestoreHealth(float value)
        {
            if (IsDead)
            {
                return;
            }
            HealthCurrent = Mathf.Clamp(HealthCurrent + value, 0f, HealthMax);
            OnHealthChanged?.Invoke(HealthCurrent, HealthMax);
        }

        public bool EnoughStamina(float value)
        {
            return value >= 0f && StaminaCurrent >= value;
        }

        public void ReduceStamina(float value)
        {
            if (IsDead)
            {
                return;
            }
            _staminaRegenerationCurrentDelayTime = 0f;
            StaminaCurrent = Mathf.Clamp(StaminaCurrent - value, 0f, StaminaMax);
            OnStaminaChanged?.Invoke(StaminaCurrent, StaminaMax);
        }

        public void RestoreStamina(float value)
        {
            if (IsDead)
            {
                return;
            }
            StaminaCurrent = Mathf.Clamp(StaminaCurrent + value, 0f, StaminaMax);
            OnStaminaChanged?.Invoke(StaminaCurrent, StaminaMax);
        }

        private void Die()
        {
            if (IsDead)
            {
                return;
            }
            IsDead = true;
        }

        public void Revive(float health = 10f, float stamina = 10f)
        {
            IsDead = false;
            RestoreHealth(HealthMax * health / 100f);
            RestoreStamina(StaminaMax * stamina / 100f);
        }
    }
}