using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPassif : MonoBehaviour
{
    public TriggerAction[] actions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Serializable]
    public struct TriggerAction
    {
        public string name;
        public bool trigger;
        public float delay;
    }
}
