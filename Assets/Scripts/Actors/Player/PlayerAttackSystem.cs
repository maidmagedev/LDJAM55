using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [Header("Basic Attack Combo")]
    public int currentCombo;
    public bool allowAttack;
    public bool bufferedAttack;
    public float idleTime = 0.0f;

    [Header("Wrench Projectile")]
    float mouseHeldTime = 0.0f;
    [SerializeField] Transform wrenchSpawnOrigin;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LineRenderer lineRendererMain;
    [SerializeField] LineRenderer lineRendererTip;
    RaycastHit hit;
    float throwDistMultiplier = 0.0f;
    public float tipLength;
    public AnimationCurve wrenchCurve;


    [Header("References")]
    [SerializeField] Transform cursor;
    [SerializeField] PlayerAnimations pAnimations;


    // Start is called before the first frame update
    void Start()
    {
        lineRendererMain.positionCount = 2;
        lineRendererMain.startWidth = 4;
        lineRendererMain.endWidth = 4;

        lineRendererTip.positionCount = 2;
        lineRendererTip.startWidth = 4;
        lineRendererTip.endWidth = 4;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Resets combo after threshhold.
        if (!pAnimations.CurrAnimState.animTags.Contains(PlayerAnimations.AnimTag.AttackString)) {
            idleTime += Time.deltaTime;
        } else {
            idleTime = 0.0f;
        }
        if (idleTime > 1.0f) {
            currentCombo = 0;
        }
        AttackComboHandler();
        WrenchThrownHandler();
    }

    void AttackComboHandler() {
        if (Input.GetMouseButtonDown(0)) {
            currentCombo++;
            switch(currentCombo) {
                case 1:
                    pAnimations.AnimQueueEnqueue(pAnimations.animStates["atkA1"]);
                    break;
                case 2:
                    pAnimations.AnimQueueEnqueue(pAnimations.animStates["atkA2"]);
                    break;
                case 3:
                    pAnimations.AnimQueueEnqueue(pAnimations.animStates["atkA3"]);
                    currentCombo = 0;
                    break;
                default:
                    currentCombo = 0;
                    break;
            }
        }
    }


    void WrenchThrownHandler() {
        throwDistMultiplier = Mathf.Clamp(mouseHeldTime, 0.5f, 1.5f);
        //Debug.DrawRay(wrenchSpawnOrigin.position + new Vector3(0, 2, 0), throwDistMultiplier * 10f *  wrenchSpawnOrigin.forward, Color.red);

        Vector3 directionToCursor = new Vector3(cursor.position.x, 0, cursor.position.z) - new Vector3(wrenchSpawnOrigin.position.x, 0, wrenchSpawnOrigin.position.z);

        Vector3 startPos = wrenchSpawnOrigin.position + new Vector3(0, 0.25f, 0);
        Vector3 endPos = wrenchSpawnOrigin.position + 10f * throwDistMultiplier * directionToCursor.normalized + new Vector3(0, 0.25f, 0);

        lineRendererMain.SetPosition(0, startPos);
        lineRendererMain.SetPosition(1, endPos - (directionToCursor.normalized * tipLength));
        lineRendererTip.SetPosition(0, lineRendererMain.GetPosition(1));
        lineRendererTip.SetPosition(1, endPos);

        if (mouseHeldTime < 0.5) {
            lineRendererMain.enabled = false;
            lineRendererTip.enabled = false;

        } else {
            lineRendererMain.enabled = true;
            lineRendererTip.enabled = true;
        }

        if (Input.GetMouseButton(0)) {
            mouseHeldTime += Time.deltaTime;
        }
        if (mouseHeldTime > 1.5f) {
            // MAX THROW! flash effect.
        }
        if (Input.GetMouseButtonUp(0)) {
            if (mouseHeldTime > 0.5f) { // initial threshhold
                Debug.Log("Throw");
                GameObject projectile = Instantiate(Resources.Load<GameObject>("CalledByScript/pf_player_projectile_wrench"), wrenchSpawnOrigin.position, wrenchSpawnOrigin.rotation);
                StartCoroutine(ProjectileHandler(projectile, endPos));
                mouseHeldTime = 0.0f;
            } else {
                mouseHeldTime = 0.0f;
                Debug.Log("Strike,");
            }
        }
    }

    IEnumerator ProjectileHandler(GameObject obj, Vector3 endPos) {
        float elapsedTime = 0.0f;
        float duration = 3.0f;

        // lerping to throw the projectile outwards
        while(elapsedTime < duration) {

            float t = wrenchCurve.Evaluate(elapsedTime / duration); // using a custom sendout speed, one that eases out
            
            obj.transform.position = Vector3.Lerp(wrenchSpawnOrigin.position, endPos, t);
            obj.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one, t);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        obj.transform.position = endPos;

        yield return new WaitForSeconds(1.5f); // time projectile is at apex

        // lerping to bring the projectile back
        elapsedTime = 0.0f;
        duration = 1.5f;
        while(elapsedTime < duration) {

            float t = elapsedTime / duration; // using a linear return
            
            obj.transform.position = Vector3.Lerp(endPos, wrenchSpawnOrigin.position, t);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        obj.transform.position = wrenchSpawnOrigin.position;
        
        // Lerping the scale away...
        elapsedTime = 0.0f;
        duration = 0.5f;
        Vector3 pos = obj.transform.position;
        while(elapsedTime < duration) {

            float t = elapsedTime / duration; // using a linear return
            obj.transform.position = Vector3.Lerp(pos, wrenchSpawnOrigin.position, t);
            obj.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        Destroy(obj);
    }



}
