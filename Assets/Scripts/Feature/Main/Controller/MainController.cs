using System;
using Feature.Common;
using Scenes.Feature.Main;
using TMPro;
using UnityEngine;

public class MainController : MonoBehaviour, IMonoEventListener
{
    private Score _score;
    [SerializeField] private TextMeshProUGUI textView;

    private void Awake()
    {
        Debug.Assert(textView != null);
        
        _score = new Score();
        
        UpdateDisplay();
    }

    public EventChain OnEventHandle(IEvent @event)
    {
        if (@event is OnClickAddPointGameEvent)
        {
            _score.AddData(10);
            UpdateDisplay();
        }
        return EventChain.Break;
    }

    private void UpdateDisplay()
    {
        textView.text = _score.Data.ToString();
    }
}