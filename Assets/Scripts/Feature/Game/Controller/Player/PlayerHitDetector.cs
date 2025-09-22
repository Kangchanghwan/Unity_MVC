using Feature.Common;
using UnityEngine;

namespace Scenes.Feature.Game.Controller
{
    public struct PlayerDamagedEvent : IEvent
    {
        public int Damage { get; }

        public PlayerDamagedEvent(int damage)
        {
            Damage = damage;
        }
    }

    public class PlayerHitDetector : MonoBehaviour, IMonoEventDispatcher
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                this.Emit(new PlayerDamagedEvent(damage: 1));
            }
        }
    }
}
