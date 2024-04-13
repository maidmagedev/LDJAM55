using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTransforms : MonoBehaviour
{
    public Transform targetTransform;
    [SerializeField] bool matchPosition;
    [SerializeField] bool matchRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null) return;
        
        if (matchPosition) {
            transform.position = targetTransform.position;
        }
        if (matchRotation) {
            transform.rotation = targetTransform.rotation;
        }
    }
}
