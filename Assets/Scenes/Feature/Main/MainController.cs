using System;
using Feature.Common;
using Scenes.Feature.Main;
using TMPro;
using UnityEngine;

public class MainController : MonoBehaviour, IMonoEventListener
{
    private Model _model;
    [SerializeField] private TextMeshProUGUI textView;

    private void Awake()
    {
        Debug.Assert(textView != null);
        _model = new Model();
        UpdateDisplay();
    }

    public EventChain OnEventHandle(IEvent @event)
    {
        if (@event is OnClickAddPointGameEvent)
        {
            Debug.Log("OnClickAddPointGameEvent");
            _model.AddData(10);
            UpdateDisplay();
        }
        return EventChain.Break;
    }

    private void UpdateDisplay()
    {
        textView.text = _model.Data.ToString();
    }
}