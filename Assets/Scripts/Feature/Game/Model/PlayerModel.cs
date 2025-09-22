using Feature.Common;
using UnityEngine;

namespace Scenes.Feature.Game.Model
{
    public struct PlayerHealthChangedEvent : IEvent
    {
        public int Current { get; }
        public int Max { get; }

        public PlayerHealthChangedEvent(int current, int max)
        {
            Current = current;
            Max = max;
        }
    }
    
    public struct PlayerDiedEvent : IEvent
    {
    }


    public class PlayerModel : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
            this.Emit(new PlayerHealthChangedEvent(currentHealth, maxHealth));
        }
        
        public void ApplyDamage(int amount)
        {
            var next = Mathf.Max(0, currentHealth - amount);
            
            if(next == currentHealth) return;
            
            this.Emit(new PlayerHealthChangedEvent(currentHealth, maxHealth));

            if (currentHealth == 0)
            {
                this.Emit<PlayerDiedEvent>();
            }
        }
    }
}