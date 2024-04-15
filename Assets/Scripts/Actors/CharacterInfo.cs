using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class CharacterInfo : MonoBehaviour
{   
    public string actorName;
    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public HitboxState hitboxState = HitboxState.vulnerable;

    [Header("Extra Info")]
    public bool isMainPlayer; // actually the player, not a player summon.
    public Team teamAffiliation;
    
    [Header("Debuffs")]
    public float stunDuration = 0.0f;

    [Header("References")]
    public CharacterManager characterManager;

    public enum Team {
        player, // the player themselves, or anything they summon.
        enemy,
        neutral // used by damage. can damage both player and enemy.
    }
    public enum HitboxState {
        vulnerable,
        invulnerable,
        dodging
    }
    public enum DamageResponse {
        dmgTaken,
        dmgResisted,
        dmgDodged,
        targetInvuln, // 
        targetHealed, // special case if the healing went through.
        killed, // target was killed by this damage instance.
        invalid // ignore bc of team affiliation etc.
    }
    
    // Used for healing and dealing damage!
    // damageValue being positive deals damage.
    // damageValue being negative heals.
    //
    // dmgAffiliation means Intended Target. So a dmgAffil of Player is intended to hurt the player.
    public DamageResponse ChangeHealth(int damageValue, Team dmgAffiliation, CharacterInfo dealerInfo, float stunDuration) {
        if (dmgAffiliation == Team.player && this.teamAffiliation == Team.enemy) return DamageResponse.invalid;
        if (dmgAffiliation == Team.enemy && this.teamAffiliation == Team.player) return DamageResponse.invalid;

        // Ignore damage, but allow healing.
        if (hitboxState == HitboxState.invulnerable && !(damageValue < 0)) {
            GameObject popupDodge = Instantiate(Resources.Load<GameObject>("CalledByScript/pf_DMGUI"), transform);
            popupDodge.GetComponent<DamagePopup>().textMesh.text = "INVULN";

            // add stun here maybe...
            return DamageResponse.targetInvuln;
        }
        if (hitboxState == HitboxState.dodging && !(damageValue < 0)) {
            GameObject popupDodge = Instantiate(Resources.Load<GameObject>("CalledByScript/pf_DMGUI"), transform);
            popupDodge.GetComponent<DamagePopup>().textMesh.text = "DODGED";
            return DamageResponse.dmgDodged;
        }
        //Healing
        if (damageValue < 0) {
             Debug.Log(actorName + " has been healed " + damageValue + " by " + dealerInfo.actorName);
            return DamageResponse.targetHealed;
        }

        // Damaging
        currentHealth -= damageValue;
        GameObject dmgPopUp = Instantiate(Resources.Load<GameObject>("CalledByScript/pf_DMGUI"), transform.position, quaternion.identity);
        dmgPopUp.GetComponent<DamagePopup>().textMesh.text = damageValue.ToString();
        OnTakeDamage(dealerInfo);

        if (stunDuration > this.stunDuration) {
            this.stunDuration = stunDuration; // stuns don't stack hehe
        } 

        Debug.Log(actorName + " has taken " + damageValue + " damage from " + dealerInfo.actorName);

        // Death
        if (currentHealth <= 0) {
            Die();
            return DamageResponse.killed;
        }
        return DamageResponse.dmgTaken;
    }

    public abstract void OnTakeDamage(CharacterInfo dealerInfo);

    public void Die() {
        Debug.Log(actorName + " has died");
    }
}
