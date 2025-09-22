using UnityEngine;

namespace Scenes.Feature.Game.View
{
    public class PlayerView : MonoBehaviour
    {
        public void PlayJumpAnimation()
        {
            Debug.Log("플레이어 점프 애니메이션 재생");
        }

        public void PlayIdleAnimation()
        {
            Debug.Log("플레이어 걷기 애니메이션 재생");
        }
    }
}