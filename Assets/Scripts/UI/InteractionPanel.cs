using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FGJ24.UI
{
    public class InteractionPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text interactionText;
        [SerializeField] private Button actionButton;
        [SerializeField] private TMP_Text actionButtonText;

        private void Start()
        {
            interactionText.text = "";
            HideInteraction();
        }

        public void HideInteraction()
        {
            interactionText.gameObject.SetActive(false);
            actionButton.gameObject.SetActive(false);
        }

        public void ShowInteraction(string actionText, string keybind)
        {
            interactionText.gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);
            interactionText.text = actionText;
            actionButtonText.text = keybind;
        }
    }
}
