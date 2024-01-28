using System;
using System.Collections.Generic;
using System.Linq;
using FGJ24.Inventory;
using FGJ24.Player;
using FGJ24.ScriptableObjects.UICrafting;
using FGJ24.UI;
using Ioni;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace FGJ24.Interactions
{
    [Serializable]
    public struct InteractableData
    {
        public Vector3 interactableObjectPosition;
        public Interactable interactable;
    }

    public static class WinSceneParams
    {
        public static bool Won { get; set; }
    }
    
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private List<InteractableData> interactablesInRange;

        [SerializeField] private PlayerControls playerControls;
        [SerializeField] private CraftingMenu craftingMenu;
        [SerializeField] private InteractionPanel interactionPanel;
        [SerializeField] private Player.Player player;
        private void Start()
        {
            playerControls.SubscribeInteraction(this);
        }

        private void Update()
        {
            if (interactablesInRange.Count > 0)
            {
                var i = interactablesInRange.First();
                interactionPanel.ShowInteraction(i.interactable.transform.parent.name, "G");
            }
            else
            {
                interactionPanel.HideInteraction();
            }
        }

        public void RegisterInteractable(Interactable i, Vector3 position)
        {
            interactablesInRange.Add(new InteractableData { interactableObjectPosition = position, interactable = i });
            TrySetCraftingMenuVisible(true);
        }

        public void DeregisterInteractable(Interactable i)
        {
            var interactable = interactablesInRange.Find(interactable => interactable.interactable == i);
            TrySetCraftingMenuVisible(false);
            interactablesInRange.Remove(interactable);
        }


        private void TrySetCraftingMenuVisible(bool visibility)
        {
            interactablesInRange.ForEach(i =>
            {
                var craftingStation = i.interactable.GetComponent<CraftingStation.CraftingStationTriggerZone>();
                if (craftingStation != null)
                {
                    SetCraftingMenuVisible(visibility);
                }
            });
        }

        public void SetCraftingMenuVisible(bool visible)
        {
            craftingMenu.gameObject.SetActive(visible);
            craftingMenu.ShowCraftingMenu();
        }

        public void TryCraftRecipe(RecipeBlueprint recipe)
        {
            var canCraft = false; 
            CraftingStation.CraftingStation craftStation = null;
            interactablesInRange.ForEach(i =>
            {
                var craftingStation = i.interactable.GetComponent<CraftingStation.CraftingStationTriggerZone>();
                if (craftingStation != null)
                {
                    canCraft = craftingStation.CraftingStation.HasResourcesFor(recipe);
                    craftStation = craftingStation.CraftingStation;
                }
            });

            if (canCraft && craftStation != null)
            {
                CraftRecipe(recipe, craftStation);
            }
            else
            {
                "Player Trying Craft, but not enough resources".Info(); // TODO - Should be indicated for player
            }
        }

        public void CraftRecipe(RecipeBlueprint recipe, CraftingStation.CraftingStation craftStation)
        {
            // eat resources
            var resourcesToRemove = new List<Resource>();

            for (int i = 0; i < recipe.Crystals; i++)
            {
                resourcesToRemove.Add(new Resource(ResourceType.Crystal));
            }
            
            for (int i = 0; i < recipe.Stones; i++)
            {
                resourcesToRemove.Add(new Resource(ResourceType.Stone));
            }
            
            for (int i = 0; i < recipe.Mushrooms; i++)
            {
                resourcesToRemove.Add(new Resource(ResourceType.Mushroom));
            }
            
            craftStation.RemoveResources(resourcesToRemove);

            // giev penefit
            if (recipe.Id == 0)
            {
                // heal
                // player.Heal(1000);
            }
            if (recipe.Id == 1)
            {
                // Regen
                // player.AddRegen(10);
            }
            if (recipe.Id == 2)
            {
                // player.AddMaxHealth(100);
            }
            if (recipe.Id == 3)
            {
                // player.AddMoveSpeed(1);
            }
            if (recipe.Id == 4)
            {
                // Win The Game
                WinSceneParams.Won = true;
                SceneManager.LoadSceneAsync("TittelScene");
            }
        }

        private Interactable ClosestInteractable()
        {
            var ownPosition = transform.position;
            IEnumerable<InteractableData> orderedInteractables = interactablesInRange.OrderBy(interactableData => Vector3.Distance(interactableData.interactableObjectPosition, ownPosition));
            return orderedInteractables.First().interactable;
        }

        public void LaunchInteraction(InputAction.CallbackContext ctx)
        {
            interactablesInRange.ForEach(i => i.interactable.Interact());
            //ClosestInteractable().Interact();
        }
    }
}
