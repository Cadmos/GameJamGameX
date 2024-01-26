using System;
using System.Collections;
using System.Collections.Generic;
using FGJ24.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FGJ24
{
    public class GameSceneUI : MonoBehaviour
    {
        [SerializeField] private bool menuVisible;
        [SerializeField] private RectTransform menuLayout;

        private void Start()
        {
            PlayerControls.Instance.SubscribeMenu(this);
        }

        public void ToggleMenu()
        {
            menuVisible = !menuVisible;
            menuLayout.gameObject.SetActive(menuVisible);
        }

        public void ToggleMenu(InputAction.CallbackContext ctx)
        {
            ToggleMenu();
        }
    }
}
