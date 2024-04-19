using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float maxMoveSpeed;

    [Header("Operational Settings")]
    bool ignoreSpeedCap;
    public bool ignorePlayerInput;

    [Header("References")]
    public Rigidbody rb;
    [SerializeField] Transform facingDirection;
    private float horizontalInput;
    private float verticalInput;
    public Vector3 moveDirection;
    public PlayerInfo pInfo;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pInfo.stunDuration > 0) {
            pInfo.stunDuration -= Time.deltaTime;
            moveDirection = Vector3.zero;
            ignorePlayerInput = true;
        } else {
            //ignorePlayerInput = false;
        }
        GetInput();
    }

    // do physics here
    void FixedUpdate() {
        MovePlayer();
        MoveSpeedCap();
    }

    void GetInput() {
        if (ignorePlayerInput) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
        moveDirection = moveDirection.normalized;

        // prevent facing direction from reseting when standing still.
        if (moveDirection != Vector3.zero) {
            facingDirection.forward = moveDirection;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Dash();
        }
    }

    // Moves player based on input
    void MovePlayer() {
        if (ignorePlayerInput) return;
        rb.AddForce(baseMoveSpeed * 10f * moveDirection, ForceMode.Force);
    }

    // ensure that player doesnt move faster than supposed to
    void MoveSpeedCap() {
        if (ignoreSpeedCap) return;

        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVelocity.magnitude > maxMoveSpeed) {
            float vertSpeed = rb.velocity.y; // maintain vertical speed
            rb.velocity = maxMoveSpeed * flatVelocity.normalized; 
            rb.velocity = new Vector3(rb.velocity.x, vertSpeed, rb.velocity.z);
        }
    }

    void Dash() {

        Debug.Log("Dash");

        //StartCoroutine(DashHandler());
        //StartCoroutine(DashHandlerB());
    }

    IEnumerator DashHandler() {
        ignoreSpeedCap = true;
        ignorePlayerInput = true;
        pInfo.hitboxState = CharacterInfo.HitboxState.dodging;

        rb.velocity = Vector3.zero;

        float elapsedTime = 0.0f;
        float duration = 0.1f;
        while (elapsedTime < duration) {
            rb.AddForce(2f * facingDirection.forward, ForceMode.Impulse);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        pInfo.hitboxState = CharacterInfo.HitboxState.vulnerable;

        ignoreSpeedCap = false;
        ignorePlayerInput = false;
    }
    
    public IEnumerator DashHandlerB() {
        //ignoreSpeedCap = true;
        ignorePlayerInput = true;
        pInfo.hitboxState = CharacterInfo.HitboxState.dodging;

        rb.velocity = Vector3.zero;

        float elapsedTime = 0.0f;
        float duration = 0.7f;

        bool clicked = false;
        float oldMax = maxMoveSpeed;
        maxMoveSpeed += 5;
        //ignoreSpeedCap = true;
        //rb.AddForce(60f * facingDirection.forward, ForceMode.Impulse);

        while (elapsedTime < duration) {
            if (elapsedTime < 0.5) {
                rb.AddForce(baseMoveSpeed * 16f * facingDirection.forward, ForceMode.Force);
            }
            yield return new WaitForFixedUpdate();
            elapsedTime += Time.deltaTime;
            if (Input.GetMouseButton(0)) {
                clicked = true;
            }
        }
        maxMoveSpeed = oldMax;
        if (clicked) {
            yield return new WaitForSeconds(0.75f);
        }
        pInfo.hitboxState = CharacterInfo.HitboxState.vulnerable;
        ignoreSpeedCap = false;
        ignorePlayerInput = false;
        
    }
}
