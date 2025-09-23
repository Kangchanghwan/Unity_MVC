using System;

namespace Feature.Game.Model
{
    public class BulletModel
    {
        public float Speed { get; private set; }
        public int Damage { get; private set; }
        public bool IsDestroyed { get; private set; }

        // C# 이벤트로 Controller에게 알림
        public event Action OnBulletDestroyed;

        public BulletModel(float speed = 3f, int damage = 1)
        {
            Speed = speed;
            Damage = damage;
            IsDestroyed = false;
        }

        public void DestroyBullet()
        {
            if (IsDestroyed) return;
            
            IsDestroyed = true;
            OnBulletDestroyed?.Invoke();
        }
    }
}