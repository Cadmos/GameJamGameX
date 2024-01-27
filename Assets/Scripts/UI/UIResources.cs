using System.Collections.Generic;
using FGJ24.Inventory;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FGJ24.UI
{
    public class UIResources : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [SerializeField] private GameObject resourcePrefab;
        public void SetResources(List<Resource> resources)
        {
            // First remove everything
            layoutGroup.transform.DestroyChildren();
            
            // Crystal
            var crystals = resources.FindAll(r => r.ResourceType == ResourceType.Crystal);
            if (crystals.Count > 0)
            {
                var uiResourceObject = GameObject.Instantiate(resourcePrefab, layoutGroup.transform);
                uiResourceObject.GetComponent<UIResource>().UpdateContent(ResourceType.Crystal, crystals.Count);
            }
            
            // Stone
            var stones = resources.FindAll(r => r.ResourceType == ResourceType.Stone);
            if (stones.Count > 0)
            {
                var uiResourceObjectStone = GameObject.Instantiate(resourcePrefab, layoutGroup.transform);
                uiResourceObjectStone.GetComponent<UIResource>().UpdateContent(ResourceType.Stone, stones.Count);
            }
        }
    }
}
