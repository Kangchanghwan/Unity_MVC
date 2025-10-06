using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.ScoreCounterV2
{
    public class ScoreController : MonoBehaviour, IController
    {
        [SerializeField]
        private Button addButton;
        [SerializeField]
        private Button subButton;
        [SerializeField]
        private TextMeshProUGUI numBoard;

        private IScoreModel _model;
        
        private void Start()
        {
            _model = this.GetModel<IScoreModel>();
            
            addButton.onClick.AddListener( this.SendCommand<AddScoreCommand>);
            subButton.onClick.AddListener( this.SendCommand<SubScoreCommand>);

            _model.Score.RegisterWithInitValue(score =>
                {
                    UpdateDisplay();
                })
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        public IArchitecture GetArchitecture()
        {
            return ScoreApp.Interface;
        }

        private void UpdateDisplay()
        {
            numBoard.text = _model.Score.ToString();
        }
       
        private void OnDestroy()
        {
            _model = null;
        }
    }  
}
