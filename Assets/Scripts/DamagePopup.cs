using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] float lifeDuration;
    public TextMeshProUGUI textMesh;
    [SerializeField] float vertSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Time.deltaTime * vertSpeed * transform.up;
    }


    IEnumerator LifeTime() {
        yield return new WaitForSeconds(lifeDuration);
        Destroy(gameObject);
    }
}