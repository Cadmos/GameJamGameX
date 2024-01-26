using UnityEngine;

namespace FGJ24.Systems
{
    public class GameSceneController : MonoBehaviour
    {
        public static GameSceneController Instance { get; private set; }
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            MusicPlayer.Instance.PlayGameMusic();
        }
    }
}