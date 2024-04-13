using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOut : MonoBehaviour
{
    public List<CharacterInfo> exclusionList;
    public List<Transform> overlap;
    [SerializeField] Transform myBody;

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<CharacterInfo>(out var OtherCharInfo)) {
            if (!exclusionList.Contains(OtherCharInfo)) overlap.Add(OtherCharInfo.transform);            
        } 
    }

    void OnTriggerExit(Collider other) {
        if (overlap.Contains(other.transform)) overlap.Remove(other.transform);
    }

    void Update() {
        foreach (Transform t in overlap) {
            myBody.position += -2.5f * Time.deltaTime * (t.position - transform.position);
        }
    }


}
