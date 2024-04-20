using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIManager : MonoBehaviour
{
    [SerializeField] List<AIMovement> bots;
    public int selectedBot;

    [Header("player bot")]
    [SerializeField] MatchTransforms matchTransforms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            CycleMode();
        }
        if (Input.GetMouseButtonDown(1)) {
            bots[selectedBot].MoveAI();
        }
    }

    void CycleMode() {
        selectedBot++;
        if (selectedBot + 1 > bots.Capacity) selectedBot = 0;

        if (selectedBot == 0) {
            matchTransforms.enabled = false;
            bots[0].agent.isStopped = false;
            //bots[0].agent.ResetPath();

        } else {
            bots[0].agent.isStopped = true;
            matchTransforms.enabled = true;
            bots[0].agent.ResetPath();
        }
    }
}
