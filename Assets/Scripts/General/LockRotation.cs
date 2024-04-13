using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;


    // Update is called once per frame
    void Update()
    {
        float x = 1;
        float y = 1;
        float z = 1;
        if (lockX) {
            x = 0;
        }
        if (lockY) {
            y = 0;
        }
        if (lockZ) {
            z = 0;
        }
        
        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x * x, transform.localRotation.y * y, transform.localRotation.z * z));
        
    }
}
