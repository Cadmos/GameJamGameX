using System.Collections.Generic;
using UnityEngine;


namespace FGJ24.CustomPhysics
{
    public static class CustomGravity
    {
        private static List<GravitySource> _sources = new List<GravitySource>();
        public static Vector3 GetGravity(Vector3 position)
        {
            Vector3 g = Vector3.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                g += _sources[i].GetGravity(position);
            }
            return g;
        }
        public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
        {
            Vector3 g = Vector3.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                g += _sources[i].GetGravity(position);
            }
            upAxis = -g.normalized;
            return g;
        }
        public static Vector3 GetUpAxis(Vector3 position)
        {
            Vector3 g = Vector3.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                g += _sources[i].GetGravity(position);
            }
            return -g.normalized;
        }

        public static void Register(GravitySource source)
        {
            _sources.Add(source);
        }

        public static void Unregister(GravitySource source)
        {
            _sources.Remove(source);
        }
        
    }
}