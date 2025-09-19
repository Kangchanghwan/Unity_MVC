using System;
using Feature.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Feature.Main
{
    public struct OnClickAddPointGameEvent : IEvent
    {
    }


    public class MainCanvas : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private Button addButton;

        private void Awake()
        {
            if (addButton != null)
            {
                addButton.onClick.AddListener(this.Emit<OnClickAddPointGameEvent>);
            }
        }
    }
}