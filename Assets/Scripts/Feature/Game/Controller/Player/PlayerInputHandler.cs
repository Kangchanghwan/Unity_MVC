using System;
using Feature.Common;
using UnityEngine;

namespace Scenes.Feature.Game.Controller
{
    public struct PlayerJumpEvent : IEvent
    {
    }

    // Input 을 처리하는 클래스
    public class PlayerInputHandler : MonoBehaviour, IMonoEventDispatcher
    {
        private InputManager _inputManager;
        private void Awake()
        {
            _inputManager = new InputManager();
        }

        private void OnEnable()
        {
            _inputManager.Enable();
            _inputManager.Player.Jump.performed += _ => this.Emit<PlayerJumpEvent>();
        }

        private void OnDisable()
        {
            _inputManager.Disable();
        }
        
    }
}