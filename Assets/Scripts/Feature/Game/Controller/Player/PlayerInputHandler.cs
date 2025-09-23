using System;
using Feature.Common;
using Feature.Game.Controller.Scenes.Feature.Game.Events;
using UnityEngine;

namespace Scenes.Feature.Game.Controller
{
    /// <summary>
    /// 플레이어 입력을 처리하는 컴포넌트
    /// </summary>
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
            _inputManager?.Disable();
        }
    }
}