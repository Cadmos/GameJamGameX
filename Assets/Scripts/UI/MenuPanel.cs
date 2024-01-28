using System;
using FGJ24.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGJ24.UI
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text youWonText;

        private void Start()
        {
            if (WinSceneParams.Won == true)
            {
                youWonText.gameObject.SetActive(true);
            }
        }

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
