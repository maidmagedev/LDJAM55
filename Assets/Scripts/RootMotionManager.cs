using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] MatchTransforms artAsChild;
    [SerializeField] MatchTransforms rigidbodyAsChild;
    [SerializeField] Transform root;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RootMotionEnable() {
        animator.applyRootMotion = true;
        Debug.Log("RM ENABLE");
        artAsChild.enabled = false;
        rigidbodyAsChild.enabled = true;
    }

    public void RootMotionDisable() {
        Debug.Log("RM DISABLE");
        
        rigidbodyAsChild.enabled = false; // disconnect rigidbody
        animator.applyRootMotion = false; // disable root motion
        transform.localPosition = Vector3.zero; // reset animator position
        artAsChild.transform.position = rigidbodyAsChild.transform.position; // drag art
        artAsChild.enabled = true; // connect art 
        
        root.position = transform.position; // drag root

        
        // rigidbodyAsChild.enabled = false; // disconnect rigidbody
        // animator.applyRootMotion = false; // disable root motion
        // transform.localPosition = Vector3.zero; // reset animator position
        // artAsChild.transform.position = rigidbodyAsChild.transform.position; // drag art
        // artAsChild.enabled = true; // connect art 
        
        // root.position = transform.position; // drag root
        //StartCoroutine(Delay());
        // manual update
        //artAsChild.transform.position = rigidbodyAsChild.transform.position;
        // transform.localRotation = Quaternion.identity;
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(0.5f);
        rigidbodyAsChild.enabled = false; // disconnect rigidbody
        animator.applyRootMotion = false; // disable root motion
        transform.localPosition = Vector3.zero; // reset animator position
        artAsChild.transform.position = rigidbodyAsChild.transform.position; // drag art
        artAsChild.enabled = true; // connect art 
        
        root.position = transform.position; // drag root
    }
    IEnumerator SFDAIFHJPA() {
        float elapsedTime = 0.0f;
        float totalTime = 3.0f;
        while (elapsedTime < totalTime) {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;

            // artAsChild.transform.position = rigidbodyAsChild.transform.position;
            // artAsChild.enabled = true;

            // artAsChild.transform.position = rigidbodyAsChild.transform.position;

        rigidbodyAsChild.enabled = false; // disconnect rigidbody
        animator.applyRootMotion = false; // disable root motion
        transform.localPosition = Vector3.zero; // reset animator position
        artAsChild.transform.position = rigidbodyAsChild.transform.position; // drag art
        artAsChild.enabled = true; // connect art 
        
        root.position = transform.position; // drag root
        }
    }
}
