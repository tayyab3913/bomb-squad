using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 5;
    public float playerDistance;
    public GameObject playerReference;
    private float attackRange = 8;
    private Vector3 previousPosition;
    private Vector3 throwDirection;
    private Rigidbody enemyRb;
    private Vector3 movementDirection;
    public GameManager gameManagerReference;
    private PowerScript powerScriptReference;
    private Vector3 haltEnemy = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        powerScriptReference = GetComponent<PowerScript>();
        powerScriptReference.SetEnemyReference(this);
        powerScriptReference.SetHealth(Random.Range(1, 3));
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerDistance();
        //throwDirection = (transform.position - previousPosition).normalized;
        //Debug.Log("Throw Direction- x:" + throwDirection.x + " y:" + throwDirection.y + " z:" + throwDirection.z);
        //previousPosition = transform.position;

        MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        throwDirection = (transform.position - previousPosition).normalized;
        //Debug.Log("Throw Direction- x:" + throwDirection.x + " y:" + throwDirection.y + " z:" + throwDirection.z);
        previousPosition = transform.position;
        movementDirection = (GetPlayerDirection() - transform.position).normalized;
        if(playerDistance >= attackRange)
        {
            enemyRb.AddForce(movementDirection * movementSpeed);
        } else if( playerDistance < attackRange)
        {
            HaltEnemy();
        }
    }

    public Vector3 GetPlayerDirection()
    {
        if (playerReference != null)
        {
            return playerReference.transform.position;
        }
        else return new Vector3(0, 0, 0);
    }

    public void SetPlayerReference(GameObject tempPlayerReference)
    {
        playerReference = tempPlayerReference;
    }

    public void GetGameManager(GameManager tempGameManagerReference)
    {
        gameManagerReference = tempGameManagerReference;
    }

    void UpdatePlayerDistance()
    {
        playerDistance = Vector3.Distance(GetPlayerDirection(), transform.position);
    }

    public void HaltEnemy()
    {
        enemyRb.velocity = haltEnemy;
        enemyRb.angularVelocity = haltEnemy;
    }
}
