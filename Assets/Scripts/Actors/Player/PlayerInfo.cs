using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : CharacterInfo
{
    public PlayerManager pManager;
    public override void OnTakeDamage(CharacterInfo dealerInfo)
    {

        Vector3 senderPos = dealerInfo.transform.position;
        senderPos.y = 0;
        Vector3 receiverPos = pManager.tpm.transform.position;
        receiverPos.y = 0;
        Vector3 launchDirection = (receiverPos - senderPos).normalized; //direction from damage dealer, to us the receiver.

        pManager.tpm.rb.velocity = Vector3.zero;
        pManager.tpm.rb.AddForce(launchDirection * 15f, ForceMode.Impulse);
    }

    void Setup() {
        currentHealth = maxHealth;
        if (pManager == null) {
            pManager = FindObjectOfType<PlayerManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
