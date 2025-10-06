using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.ScoreCounterV2
{
    public interface IScoreModel : IModel
    {
        BindableProperty<int> Score { get; }
    }

    public class ScoreModel : AbstractModel, IScoreModel
    {
        public BindableProperty<int> Score { get; } = new BindableProperty<int>();

        protected override void OnInit()
        {
            var storage = this.GetUtility<IStorage>();
            
            Score.SetValueWithoutEvent(storage.LoadInt(nameof(Score)));
            
            Score.Register(score =>
                storage.SaveInt(nameof(Score), score)
            );
        }
    }

    public interface IStorage : IUtility
    {
        void SaveInt(string key, int data);
        int LoadInt(string key, int defaultScore = 0);
    }

    public class Storage : IStorage
    {
        public void SaveInt(string key, int data)
        {
            PlayerPrefs.SetInt(key, data);
        }

        public int LoadInt(string key, int defaultScore = 0)
        {
            return PlayerPrefs.GetInt(key, defaultScore);
        }
    }

    public interface IAchievementSystem : ISystem
    {
    }

    public class AchievementSystem : AbstractSystem, IAchievementSystem
    {
        protected override void OnInit()
        {
            var scoreModel = this.GetModel<IScoreModel>();

            scoreModel.Score.RegisterWithInitValue(score =>
            {
                if (score == 10)
                {
                    Debug.Log("업적 달성: 클릭 마스터");
                }
                else if (score == 20)
                {
                    Debug.Log("업적 달성: 클릭 전문가");
                }
                else if (score == -10)
                {
                    Debug.Log("업적 달성: 클릭 초심자");
                }
            });
        }
    }
    
    public class ScoreApp : Architecture<ScoreApp>
    {
        protected override void Init()
        {
            this.RegisterSystem<IAchievementSystem>(new AchievementSystem());
            this.RegisterModel<IScoreModel>(new ScoreModel());
            this.RegisterUtility<IStorage>(new Storage());
        }
        
        protected override void ExecuteCommand(ICommand command)
        {
            Debug.Log(command.GetType().Name + " 실행 전");
            base.ExecuteCommand(command);
            Debug.Log(command.GetType().Name + " 실행 후");
        }
    }

    public class AddScoreCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<IScoreModel>().Score.Value++;
        }
    }
    public class SubScoreCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<IScoreModel>().Score.Value--;
        }
    }
    
   
}
