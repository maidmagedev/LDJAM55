using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] LayerMask cursorLayer;
    [Header("Point N Click")] 
    [SerializeField] Transform flagPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, cursorLayer)) {
            transform.position = raycastHit.point;
        }

        if (Input.GetMouseButtonDown(1)) {
            flagPlayer.position = transform.position;
        }
    }
}
