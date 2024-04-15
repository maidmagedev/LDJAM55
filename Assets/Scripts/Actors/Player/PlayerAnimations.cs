using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ThirdPersonMovement movement;

    // priority list
    private List<AnimState> animQueue;
    private AnimState currAnimState;
    
    private readonly Dictionary<string, AnimState> animStates = new Dictionary<string, AnimState>{
        //new animClipInfo{ priority = 0, stateName = "", normalizedTransitionDuration = 0.0f, layer = 0},
        {"idle",new AnimState{ priority = 010, stateName = "idle", normalizedTransitionDuration = 0.00f, layer = 0}},
        {"run", new AnimState{ priority = 050, stateName = "run", normalizedTransitionDuration = 0.00f, layer = 0}},
        {"dash", new AnimState{ priority = 400, stateName = "dash", normalizedTransitionDuration = 0.00f, layer = 0}},
        {"stunned", new AnimState{ priority = 500, stateName = "stunned", normalizedTransitionDuration = 0.0f, layer = 0}},
    };

    private struct AnimState {
        public int priority;
        public string stateName;
        public float normalizedTransitionDuration;
        public int layer;
        
    }

    void AnimQueueSetup() {
        animQueue = new List<AnimState>(){
            animStates["idle"],
            // animStates["run"],
            // animStates["stunned"]
        };
    }

    // Sorts the queue, and if the top value was updated, play that new animation.
    void AnimQueueRefresh() {
        AnimState statePreSort = currAnimState;

        animQueue.Sort((a, b) => b.priority.CompareTo(a.priority));
        if (animQueue[0].stateName != statePreSort.stateName)
            animator.CrossFade(animQueue[0].stateName, animQueue[0].normalizedTransitionDuration, animQueue[0].layer);

        currAnimState = animQueue[0];
        //print("playing: " + currAnimState.stateName);
    }

    // Adds an item to the queue if it isnt already there, and then refreshes the queue.
    void AnimQueueEnqueue(AnimState state) {
        if (animQueue.Contains(state)) return;
        animQueue.Add(state);
        AnimQueueRefresh();
    }

    // Start is called before the first frame update
    void Start()
    {
        AnimQueueSetup();
        AnimQueueRefresh();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (movement.pInfo.stunDuration > 0) {
            AnimQueueEnqueue(animStates["stunned"]);
        } else {
            //print("removed stunned");
            animQueue.Remove(animStates["stunned"]);
            AnimQueueRefresh();
        }
        if (movement.moveDirection != Vector3.zero && movement.rb.velocity.magnitude > 0.1f) {
            AnimQueueEnqueue(animStates["run"]);
        } else {
            animQueue.Remove(animStates["run"]);
            //print("removed run");
            AnimQueueRefresh();
        }
        if (Input.GetKeyDown(KeyCode.J)) StartCoroutine(DashStupid());
        
    }

    IEnumerator DashStupid() {
        if (animQueue.Contains(animStates["dash"])) yield break;
        
        AnimQueueEnqueue(animStates["dash"]);
        yield return new WaitForSeconds(1.5f);
        animQueue.Remove(animStates["dash"]);
        AnimQueueRefresh();
        FindObjectOfType<RootMotionManager>().RootMotionDisable();
    }
}