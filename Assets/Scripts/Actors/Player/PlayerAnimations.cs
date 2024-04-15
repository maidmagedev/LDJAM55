using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] ThirdPersonMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.pInfo.stunDuration > 0) {
            anim.CrossFade("stunned", 0 , 0);
            return;
        } 
        if (movement.rb.velocity.magnitude > 0.4f) {
            anim.CrossFade("run", 0, 0);
        } else {
            anim.CrossFade("idle", 0, 0);
        }
        
    }
}
