using System.Collections.Generic;
using Ioni;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GodSpeed
{
    
    public class TestingGodSpeed : MonoBehaviour
    {
        [SerializeField] private DataObject dataobject;

        [SerializeField] private List<int> numbers = new ();
        public void Start()
        {
            D.Info("ddd", dataobject);
            D.Err("Boom: ", dataobject.ddd);
            "Warning".Warn();
        }

        public void EventTest(int i)
        {
            if (numbers.Contains(i))
            {
                numbers.Remove(i);
            }
            else
            {
                numbers.Add(i);
            }
        }
    }

    [System.Serializable]
    public struct DataObject
    {
        public string name;
        public string ddd;

        public override string ToString()
        {
            return $"({name} : {ddd})";
        }
    }
}

