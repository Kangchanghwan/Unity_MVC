using System;
using Feature.Common;
using Scenes.Feature.Game.Model;
using Scenes.Feature.Game.View;
using UnityEngine;

namespace Scenes.Feature.Game.Controller
{
    public class BulletController : MonoBehaviour, IMonoEventListener
    {
        [SerializeField] private BulletModel bulletModel;
        [SerializeField] private BulletView bulletView;

        private void Update()
        {
            bulletView.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Time.deltaTime * -bulletModel.Speed, 0);
        }

        public EventChain OnEventHandle(IEvent @event)
        {
            switch (@event)
            {
                case BulletOnTriggerEvent:
                    Destroy(gameObject);
                    return EventChain.Break;
            }

            return EventChain.Continue;
        }
    }
}