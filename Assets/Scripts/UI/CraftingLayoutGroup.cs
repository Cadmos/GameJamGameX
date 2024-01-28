using System.Collections.Generic;
using FGJ24.ScriptableObjects.UICrafting;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FGJ24.UI
{
    public class CraftingLayoutGroup : MonoBehaviour
    {
        [SerializeField] private GameObject layoutElementPrefab;
        
        public void UpdateUI(List<RecipeBlueprint> recipes)
        {
            transform.DestroyChildren();
            
            recipes.ForEach(r =>
            {
                var newLayoutElement = Instantiate(layoutElementPrefab, transform).GetComponent<CraftingLayoutElement>();
                if (newLayoutElement != null)
                {
                    newLayoutElement.SetContents(r);
                }
            });
        }
    }
}
