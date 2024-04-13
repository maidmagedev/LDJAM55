using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        if (targetTransform == null) {
            targetTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null) return;

        transform.rotation = Quaternion.Euler(new Vector3(targetTransform.localRotation.x, 0, 0));
    }
}
