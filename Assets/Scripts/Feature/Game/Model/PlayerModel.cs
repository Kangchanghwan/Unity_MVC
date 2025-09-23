using System;
using UnityEngine;

namespace Feature.Game.Model
{
    /// <summary>
    /// 플레이어의 데이터와 로직을 관리하는 순수 C# 클래스
    /// </summary>
    public class PlayerModel
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        // C# 이벤트로 Controller에게 알림
        public event Action<int, int> OnHealthChanged; // (current, max)
        public event Action OnPlayerDied;

        public PlayerModel(int maxHealth = 3)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            IsDead = false;
        }

        public void ApplyDamage(int damage)
        {
            if (IsDead || damage <= 0) return;

            var newHealth = Mathf.Max(0, CurrentHealth - damage);
            if (newHealth == CurrentHealth) return;

            CurrentHealth = newHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0) return;

            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        private void Die()
        {
            IsDead = true;
            OnPlayerDied?.Invoke();
        }
    }
}