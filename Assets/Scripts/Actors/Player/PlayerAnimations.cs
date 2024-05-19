using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator dashOverride;
    [SerializeField] ThirdPersonMovement movement;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    [SerializeField] Transform trackOverride;

    // priority list
    public List<AnimState> animQueue;
    public AnimState CurrAnimState {get; set;}
    
    public readonly Dictionary<string, AnimState> animStates = new Dictionary<string, AnimState>{
        //new animClipInfo{ priority = 0, stateName = "", normalizedTransitionDuration = 0.0f, layer = 0},
        {"idle",new AnimState{ priority = 010, stateName = "idle", normalizedTransitionDuration = 0.00f, layer = 0, 
            animTags = new List<AnimTag>(){AnimTag.DashCancellable, AnimTag.AttackCancellable}
        }},
        {"run", new AnimState{ priority = 050, stateName = "run", normalizedTransitionDuration = 0.00f, layer = 0,
            animTags = new List<AnimTag>(){AnimTag.DashCancellable, AnimTag.AttackCancellable}
        }},

        {"atkA1", new AnimState{ priority = 300, stateName = "atkA1", normalizedTransitionDuration = 0.00f, layer = 0,
            animTags = new List<AnimTag>(){AnimTag.DashCancellable, AnimTag.AttackString, AnimTag.DenyMovementInputs}

        }},
        {"atkA2", new AnimState{ priority = 310, stateName = "atkA2", normalizedTransitionDuration = 0.00f, layer = 0,
            animTags = new List<AnimTag>(){AnimTag.DashCancellable, AnimTag.AttackString, AnimTag.DenyMovementInputs}

        }},
        {"atkA3", new AnimState{ priority = 320, stateName = "atkA3", normalizedTransitionDuration = 0.00f, layer = 0,
            animTags = new List<AnimTag>(){AnimTag.DashCancellable, AnimTag.AttackString, AnimTag.DenyMovementInputs}
        }},

        {"dashATK", new AnimState{ priority = 700, stateName = "dashATK", normalizedTransitionDuration = 0.00f, layer = 0,
            animTags = new List<AnimTag>(){AnimTag.DashCancellable, AnimTag.AttackCancellable, AnimTag.DenyMovementInputs}
        }},
        {"stunned", new AnimState{ priority = 900, stateName = "stunned", normalizedTransitionDuration = 0.0f, layer = 0}},
    };

    public struct AnimState {
        public int priority;
        public string stateName;
        public float normalizedTransitionDuration;
        public int layer;
        public List<AnimTag> animTags;
    }

    public enum AnimTag {
        DashCancellable, // a move that can be interrupted into a dash.
        AttackCancellable, // a move that can be canceled into an attack.
        AttackString, // a move that is itself an attack.
        AcceptAttackInputs, // a move that will accept inputs to buffer into the next attack.
        DenyMovementInputs, // denies movement input recieving
        Armored, // a move that supercedes stuns.
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
        AnimState statePreSort = CurrAnimState;

        animQueue.Sort((a, b) => b.priority.CompareTo(a.priority));
        if (animQueue[0].stateName != statePreSort.stateName)
            animator.CrossFade(animQueue[0].stateName, animQueue[0].normalizedTransitionDuration, animQueue[0].layer);

        CurrAnimState = animQueue[0];
        //print("playing: " + currAnimState.stateName);
    }

    // Adds an item to the queue if it isnt already there, and then refreshes the queue.
    public void AnimQueueEnqueue(AnimState state) {
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
        if (/* movement.moveDirection != Vector3.zero && */ movement.rb.velocity.magnitude > 2f) {
            AnimQueueEnqueue(animStates["run"]);
        } else {
            animQueue.Remove(animStates["run"]);
            //print("removed run");
            AnimQueueRefresh();
        }
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(DashSmarter());
        
    }

    IEnumerator DashStupid() {
        if (animQueue.Contains(animStates["dashATK"])) yield break;
        //movement.ignorePlayerInput = true;

        Transform oldLook = cinemachine.LookAt;
        Transform oldFollow = cinemachine.Follow;

        //cinemachine.Follow = trackOverride;
        cinemachine.LookAt = trackOverride;
        
        AnimQueueEnqueue(animStates["dashATK"]);
        dashOverride.CrossFade("dashATK", 0, 0);
        yield return new WaitForSeconds(1.65f);
        animQueue.Remove(animStates["dashATK"]);
        dashOverride.CrossFade("initial", 0, 0);
        AnimQueueRefresh();
        FindObjectOfType<RootMotionManager>().RootMotionDisable();
        //movement.ignorePlayerInput = false;

        //cinemachine.Follow = oldFollow;
        cinemachine.LookAt = oldLook;
    }

    public IEnumerator DashSmarter() {
        if (animQueue.Contains(animStates["dashATK"])) yield break;
        AnimQueueEnqueue(animStates["dashATK"]);
        dashOverride.CrossFade("dashATK REWORK", 0, 0);
        float elapsedTime = 0.0f;
        float duration = 0.7f;
        bool clicked = false;
        while (elapsedTime < duration) {

            yield return new WaitForFixedUpdate(); // line up with the dash coroutine in TPM.
            elapsedTime += Time.deltaTime;
            if (Input.GetMouseButton(0)) {
                clicked = true;
            }
        }
        if (!clicked) {
            dashOverride.CrossFade("initial", 0, 0);
            animQueue.Remove(animStates["dashATK"]);
            AnimQueueRefresh();
            yield break;

        } else {
            yield return new WaitForSeconds(0.75f);

            animQueue.Remove(animStates["dashATK"]);
            dashOverride.CrossFade("initial", 0, 0);

            //movement.ignorePlayerInput = false;
            AnimQueueRefresh();
        }
    }
}
