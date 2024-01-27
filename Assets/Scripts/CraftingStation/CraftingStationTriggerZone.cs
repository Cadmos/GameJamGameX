using FGJ24.Interactions;
using Ioni.Extensions;
using UnityEngine;

namespace FGJ24.CraftingStation
{
    public class CraftingStationTriggerZone : Interactable
    {
        [SerializeField] private CraftingStation craftingStation;

        public override void Interact()
        {
            "Interacting With Crafting Station".Info();
            craftingStation.StartInteractWithCraftStation();
        }

        public CraftingStation CraftingStation => craftingStation;
    }
}
