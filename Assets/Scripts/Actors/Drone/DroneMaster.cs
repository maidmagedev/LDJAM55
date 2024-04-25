using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DroneMaster : MonoBehaviour
{
    public List<DroneBrain> droneUnits;
    [SerializeField] GameObject pfDrone;
    [SerializeField] Transform target;
    public float radius; // used for positioning the drones.

    // Start is called before the first frame update
    void Start()
    {
        AddNewDrone();
        PositionDrones();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            AddNewDrone();
            PositionDrones();
        }
    }

    void PositionDrones() {
        float angleIncrement = 360f / droneUnits.Count; // relative to the center, what angle should each drone be placed at?

        for (int i = 0; i < droneUnits.Count; i++) {
            float angle = i * angleIncrement;
            
            //droneUnits[i].transform.localRotation = Quaternion.Euler(0, angle, 0);
            //droneUnits[i].body.transform.localPosition = new Vector3(0, 0, 5.5f);
            //Debug.Log("Placing unit [" + i + "] at " + Quaternion.Euler(0, angle, 0) + ", " + new Vector3(0, 0, 5.5f));

            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 pos = new Vector3(x, 0, y) + transform.position;
            droneUnits[i].transform.position = pos;
        }
    }

    void AddNewDrone() {
        GameObject newDrone = Instantiate(pfDrone, transform);
        DroneBrain newBrain = newDrone.GetComponent<DroneBrain>();
        newBrain.stare = target;

        droneUnits.Add(newBrain);
    }
}
