using UnityEngine;

namespace FGJ24.Inventory
{
    [System.Serializable]
    public enum ResourceType
    {
        Crystal, 
        Stone,
        Mushroom,
    }
    
    [System.Serializable]
    public class Resource
    {
        [SerializeField] private string name;
        [SerializeField] private ResourceType resource;

        public Resource(ResourceType resourceType)
        {
            name = NameForResource(resourceType);
            resource = resourceType;
        }

        private string NameForResource(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Crystal:
                    return "Crystal";
                case ResourceType.Stone:
                    return "Stone";
                case ResourceType.Mushroom:
                    return "Mushroom";
                default:
                    return "NaN";
            }
        }

        public ResourceType ResourceType => resource;
    }
}
