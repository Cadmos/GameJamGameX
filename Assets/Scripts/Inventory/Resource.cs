using UnityEngine;

namespace FGJ24.Inventory
{
    [System.Serializable]
    public enum ResourceType
    {
        Crystal, 
        Stone
    }
    
    [System.Serializable]
    public class Resource
    {
        [SerializeField] private string name;
        [SerializeField] private ResourceType resource;

        public Resource(ResourceType resourceType)
        {
            this.name = NameForResource(resourceType);
            resource = resourceType;
        }

        private string NameForResource(ResourceType resourceType)
        {
            switch (resource)
            {
                case ResourceType.Crystal:
                    return "Crystal";
                case ResourceType.Stone:
                    return "Crystal";
                default:
                    return "NaN";
            }
        }

        public ResourceType ResourceType => resource;
    }
}
