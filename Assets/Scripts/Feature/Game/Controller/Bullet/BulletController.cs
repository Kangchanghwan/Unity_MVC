using Feature.Common;
using Feature.Game.Controller.Scenes.Feature.Game.Events;
using Feature.Game.Model;
using Feature.Game.View;
using Scenes.Feature.Game.Controller;
using UnityEngine;

namespace Feature.Game.Controller.Bullet
{
    /// <summary>
    /// 총알의 Model과 View를 중재하는 Controller
    /// </summary>
    public class BulletController : EventListenerMono, IMonoEventDispatcher
    {
        [Header("Bullet Settings")] [SerializeField]
        private float speed = 3f;

        [SerializeField] private int damage = 1;

        private BulletModel _model;
        private BulletView _view;

        private void Awake()
        {
            _model = new BulletModel(speed, damage);
            _view = GetComponentInChildren<BulletView>();
        }

        private void Start()
        {
            // Model의 C# 이벤트 구독
            _model.OnBulletDestroyed += OnBulletDestroyed;

            // 글로벌 이벤트 구독 (자신의 히트 이벤트 감지)
            EventManager.Subscribe<BulletHitEvent>(this);
        }

        private void Update()
        {
            if (!_model.IsDestroyed)
            {
                // View에게 움직임 지시
                _view.MoveBullet(_model.Speed);
            }
        }

        // Model에서 오는 파괴 이벤트
        private void OnBulletDestroyed()
        {
            _view.PlayDestroyEffect();
            Destroy(gameObject, 0.5f); // 이펙트 재생 후 파괴
        }

        // 글로벌 이벤트 핸들러
        public override EventChain OnEventHandle(IEvent @event)
        {
            if (@event is BulletHitEvent hitEvent)
            {
                this.EmitGlobal(new ObjectDamagedEvent(_model.Damage, hitEvent.TargetTag));
                _model.DestroyBullet();
                return EventChain.Break;
            }

            return EventChain.Continue;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_model != null)
                _model.OnBulletDestroyed -= OnBulletDestroyed;
        }
    }
}