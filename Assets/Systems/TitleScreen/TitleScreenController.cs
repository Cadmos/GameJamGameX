using UnityEngine;

namespace FGJ24
{
    public class TitleScreenController : MonoBehaviour
    {
            public static TitleScreenController Instance { get; private set; }
        
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
                MusicPlayer.Instance.PlayMenuMusic();
            }
    }
}
