using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageTest : MonoBehaviour
{
    public List<Material> replacementMats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            StartCoroutine(AfterImage());
        }
    }

    IEnumerator AfterImage() {
        // List<GameObject> afterImages = new List<GameObject>();
        // Instantiate(gameObject, transform.parent);
        // for (int i = 0; i < 4; i++) {
        //     afterImages.Add(Instantiate(gameObject, transform.parent));
        //     ReplaceMaterials(afterImages[i], replacementMats[i]);
        // }

        for (int i = 1; i < 5; i++) {
            transform.GetPositionAndRotation(out Vector3 oldPos, out Quaternion oldRot);
            yield return new WaitForSeconds(0.1f * i);
            GameObject o = Instantiate(gameObject, transform.parent);
            o.transform.SetPositionAndRotation(oldPos, oldRot);
            ReplaceMaterials(o, replacementMats[i - 1]);
            yield return new WaitForSeconds(0.1f);
        }
        //gameObject.SetActive(false);
    }

    IEnumerator AfterImageOld() {
        List<GameObject> afterImages = new List<GameObject>();
        Instantiate(gameObject, transform.parent);
        for (int i = 0; i < 4; i++) {
            afterImages.Add(Instantiate(gameObject, transform.parent));
            ReplaceMaterials(afterImages[i], replacementMats[i]);
        }

        for (int i = 1; i < 5; i++) {
            transform.GetPositionAndRotation(out Vector3 oldPos, out Quaternion oldRot);
            yield return new WaitForSeconds(0.1f * i);
            afterImages[i - 1].transform.SetPositionAndRotation(oldPos, oldRot);
            yield return new WaitForSeconds(0.5f);
        }
        gameObject.SetActive(false);
    }

    // GPT Function to replace materials of all children with the given material
    public void ReplaceMaterials(GameObject parentObject, Material newMaterial)
    {
        // Get all renderers in the children of the parentObject
        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();

        // Iterate through each renderer and replace its materials with the newMaterial
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = newMaterial;
            }
            renderer.sharedMaterials = materials;
        }
    }
}
