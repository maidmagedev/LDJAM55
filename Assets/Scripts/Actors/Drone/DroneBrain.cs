using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBrain : MonoBehaviour
{
    public Transform stare;
    public Transform body;
    public Transform cursor;

    public bool cursorLock = false;
    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<MousePosition>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            cursorLock = !cursorLock;
        }
        if (cursorLock) {
            body.transform.LookAt(cursor);
        } else {
            body.transform.LookAt(stare);
        }
    }
}
