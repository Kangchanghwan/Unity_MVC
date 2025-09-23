using Feature.Common;
using UnityEngine;

namespace Feature.Game.Controller
{
    namespace Scenes.Feature.Game.Events
    {
        // === 플레이어 관련 이벤트 ===
        public struct PlayerJumpEvent : IEvent
        {
        }

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
            public string PlayerName { get; }
            public Vector3 Position { get; }

            public PlayerDiedEvent(string playerName, Vector3 position)
            {
                PlayerName = playerName;
                Position = position;
            }
        }

        public struct ObjectDamagedEvent : IEvent
        {
            public int Damage { get; }
            public string TagName { get; }

            public ObjectDamagedEvent(int damage, string tagName = "")
            {
                Damage = damage;
                TagName = tagName;
            }
        }

        // === 총알 관련 이벤트 ===
        public struct BulletHitEvent : IEvent
        {
            public Vector3 HitPosition { get; }
            public string TargetTag { get; }

            public BulletHitEvent(Vector3 hitPosition, string targetTag)
            {
                HitPosition = hitPosition;
                TargetTag = targetTag;
            }
        }
    }
}