using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 15;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 throwDirection;
    public Rigidbody playerRb;
    private PowerScript powerScriptReference;
    private RagDoll ragDollReference;
    public GameManager gameManagerReference;
    private Vector3 haltPlayer = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        powerScriptReference = GetComponent<PowerScript>();
        powerScriptReference.SetPlayerReference(this);
        powerScriptReference.SetHealth(3);
        ragDollReference = GetComponent<RagDoll>();
        ragDollReference.SetPlayerReference(this);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        if(horizontalInput == 0 && verticalInput == 0)
        {
            HaltPlayer();
        }
    }

    void PlayerMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        throwDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        playerRb.AddForce(Vector3.right * horizontalInput * movementSpeed);
        playerRb.AddForce(Vector3.forward * verticalInput * movementSpeed);
    }

    public Vector3 GetThrowDirection()
    {
        return throwDirection;
    }

    public void HaltPlayer()
    {
        playerRb.velocity = haltPlayer;
        playerRb.angularVelocity = haltPlayer;
    }

    public void GetGameManager(GameManager tempGameManagerReference)
    {
        gameManagerReference = tempGameManagerReference;
    }
}
