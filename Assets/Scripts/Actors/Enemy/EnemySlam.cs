using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySlam : CharacterInfo
{
    [Header("Additional Stats")]
    [SerializeField] float speed;

    [Header("References")]
    [SerializeField] Transform target;
    [SerializeField] Animator anim;
    [SerializeField] MatchTransforms targetLockOn; // used to seek the target 
    [SerializeField] Transform attackPosition; // position relative to the target to attack from.

    [Header("States")]
    [SerializeField] bool aimAtTarget;  // just looking at them
    [SerializeField] bool huntingTarget = true; // moving towards them
    [SerializeField] bool isAttacking;


    // Start is called before the first frame update
    void Start()
    {
        ObtainTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunDuration > 0) {
            stunDuration -= Time.deltaTime;
            return;
        }
        if (target == null) return;
        if (aimAtTarget) transform.LookAt(target);
        targetLockOn.transform.LookAt(this.transform);

        //if (huntingTarget) transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, speed * Time.fixedDeltaTime);
    }

    void FixedUpdate() {
        if (huntingTarget && stunDuration <= 0) {
            transform.position = Vector3.MoveTowards(transform.position, attackPosition.position, speed * Time.fixedDeltaTime);

            if (!isAttacking && Vector3.Distance(transform.position, attackPosition.position) <= 0.1f) {
                anim.SetTrigger("StartAttack");
                isAttacking = true;
            }
        }
    }

    public void ObtainTarget() {
        // player
        target = FindObjectOfType<PlayerInfo>().transform;
        targetLockOn.enabled = true;
        targetLockOn.targetTransform = target;
    }

}
