using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGJ24.UI
{
    public class MenuPanel : MonoBehaviour
    {
        public static void StartGame()
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
        
        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}
