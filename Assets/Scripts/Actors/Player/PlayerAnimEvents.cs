using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    public PlayerAttackSystem pAttackSystem;
    public PlayerAnimations pAnimations;


    // Start is called before the first frame update
    void Start()
    {
        if (pAttackSystem == null) pAttackSystem = FindObjectOfType<PlayerAttackSystem>();
        
        if (pAnimations == null) pAnimations = FindObjectOfType<PlayerAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // used for the basic attack combo
    void SetAnimCancel() {
        
    }

    public void RemoveFromQueue(string removedState) {
        pAnimations.animQueue.Remove(pAnimations.animStates[removedState]);
    }
}
