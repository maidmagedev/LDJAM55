using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAwayDirection : MonoBehaviour
{
    public List<Transform> enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 average = Vector3.zero;
        foreach (Transform t in enemies) {
            Vector3 dirTowardsEnemy = (t.position - transform.position).normalized;
            Debug.DrawRay(transform.position, dirTowardsEnemy * 10f, Color.red, 0.1f);
            average += dirTowardsEnemy * Vector3.Distance(transform.position, t.position);
        }
        average /= 3;
        Debug.DrawRay(transform.position, average.normalized * 10f, Color.yellow, 0.1f);
        Debug.DrawRay(transform.position, average.normalized * -10f, Color.green, 0.1f);
    }
}
