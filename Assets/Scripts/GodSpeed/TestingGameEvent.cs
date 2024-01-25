using System.Collections;
using System.Collections.Generic;
using Ioni.GameEvent;
using UnityEngine;

public class TestingGameEvent : MonoBehaviour
{
    [SerializeField] private GameEvent e;
    void Start()
    {
        e.Invoke();
    }
}
