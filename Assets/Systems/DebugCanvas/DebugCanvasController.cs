using TMPro;
using UnityEngine;

namespace FGJ24
{
    public class DebugCanvasController : MonoBehaviour
    {
        
        public static DebugCanvasController Instance;
        
        [SerializeField] private GameObject _debugCanvas;

        [SerializeField] private TextMeshProUGUI _stateText;
        
        public void SetStateText(string text)
        {
            _stateText.text = text;
        }
        
        public void ToggleDebugCanvas()
        {
            _debugCanvas.SetActive(!_debugCanvas.activeSelf);
        }
        
        
        
        private void Awake()
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

    }
}
