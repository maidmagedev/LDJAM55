using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxCollider : MonoBehaviour
{
    public int damageToDeal;
    public float stunDuration;
    public CharacterInfo.Team dmgAffiliation;
    public CharacterInfo senderInfo;
    public List<CharacterInfo> targetsHitRecently; // can be cleared at whatever interval...

    void OnTriggerEnter(Collider other) {
        //Debug.Log("TEST!");
        if (other.TryGetComponent<CharacterInfo>(out var OtherCharInfo)) {
            //if (targetsHitRecently.Contains(OtherCharInfo)) return; // dont repeat hit
            //Debug.Log("Change HP");
            OtherCharInfo.ChangeHealth(damageToDeal, dmgAffiliation, senderInfo, 0.5f);
            // targetsHitRecently.Add(OtherCharInfo);
        } 
    }

    public void ClearHitTargetList() {
        targetsHitRecently.Clear();
    }
}
