using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    public NavMeshAgent agent;
    [SerializeField] LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 0;
    }

    void DrawPath() {
        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPosition(0, transform.position + new Vector3(0, 0.25f, 0));

        if (agent.path.corners.Length < 2) return;
        
        for (int i = 1; i < agent.path.corners.Length; i++) {
            Vector3 pointPos = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y + 0.25f, agent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPos);
        }
    }

    IEnumerator PathForum() {
        yield return new WaitForSeconds(0.5f);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        agent.SetDestination(target.position);

        yield return new WaitForEndOfFrame();

        //if (agent.path.corners.Length < 2) yield break;

        lineRenderer.positionCount = agent.path.corners.Length;
        for (int i = 0; i < agent.path.corners.Length; i++) {
            lineRenderer.SetPosition(i, agent.path.corners[i]);        
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(1)) {
        //     //StartCoroutine(PathForum());
        //     agent.SetDestination(target.position);
        // }

        if (agent.hasPath) DrawPath();

    }

    public void MoveAI() {
        agent.SetDestination(target.position);
    }

    public void ResetPath() {
        lineRenderer.positionCount = 0;
        agent.SetDestination(agent.transform.position);
    }

}
