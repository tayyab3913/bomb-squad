using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerScript : MonoBehaviour
{
    public GameObject bombPrefab;
    public GameObject minePrefab;
    public GameObject stickyBombPrefab;
    public Vector3 throwDirection;
    public float horizontalInput;
    public float verticalInput;
    public float throwForce = 50;
    public int health = 3;
    public bool canThrowBomb = true;
    public bool hasMultiShootPowerup = false;
    public bool hasStickyBombPowerup = false;
    public bool hasMinesPowerup = false;
    public bool hasHealthPowerup = false;
    public bool hasNoAbility = true;
    private Enemy enemyReference;
    private PlayerController playerControllerReference;
    public RagDoll ragDollReference;
    private bool isEnemyCouroutineCalled;
    private int specialBombsInHand = 3;
    private Vector3 halt = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        ragDollReference = GetComponent<RagDoll>();
        ragDollReference.SetPowerScriptReference(this);
        if (gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(EnemyBombThrow());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.CompareTag("Player") && health > 0)
        {
            UserControllerBombThrow();
        } else if (gameObject.CompareTag("Enemy") && isEnemyCouroutineCalled && health > 0)
        {
            StartCoroutine(EnemyBombThrow());
            isEnemyCouroutineCalled = false;
        }
    }

    void InstantiatePlayerBomb()
    {
        GameObject tempBomb = Instantiate(bombPrefab, new Vector3(transform.position.x, 3, transform.position.z), bombPrefab.transform.rotation);

        if (playerControllerReference != null)
        {
            throwDirection = playerControllerReference.GetThrowDirection();
        }
        Rigidbody tempRb = tempBomb.GetComponent<Rigidbody>();
        //Debug.Log("Throw Direction- x:" + throwDirection.x + " y:" + throwDirection.y + " z:" + throwDirection.z);
        tempRb.AddForce((throwDirection * throwForce), ForceMode.Impulse);

    }

    void InstantiateEnemyBomb()
    {
        GameObject tempBomb = Instantiate(bombPrefab, new Vector3(transform.position.x, 3, transform.position.z), bombPrefab.transform.rotation);

        if (enemyReference != null)
        {
            throwDirection = (enemyReference.GetPlayerDirection() - transform.position).normalized;
        }
        Rigidbody tempRb = tempBomb.GetComponent<Rigidbody>();
        //Debug.Log("Throw Direction- x:" + throwDirection.x + " y:" + throwDirection.y + " z:" + throwDirection.z);
        tempRb.AddForce((throwDirection * throwForce), ForceMode.Impulse);

    }

    public void SetPlayerReference(PlayerController tempParticipantReference)
    {
        playerControllerReference = tempParticipantReference;
    }

    public void SetEnemyReference(Enemy tempParticipantReference)
    {
        enemyReference = tempParticipantReference;
    }

    void UserControllerBombThrow()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canThrowBomb && hasNoAbility)
        {
            HaltPlayer();
            InstantiatePlayerBomb();
            canThrowBomb = false;
            StartCoroutine(PlayerBombCoolDown());
        } else if(Input.GetKeyDown(KeyCode.Space) && hasStickyBombPowerup)
        {
            HaltPlayer();
            StickyBombPowerUpUsed();
        } else if (Input.GetKeyDown(KeyCode.Space) && hasMinesPowerup)
        {
            HaltPlayer();
            MinesPowerUpUsed();
        }
    }

    IEnumerator EnemyBombThrow()
    {
        yield return new WaitForSeconds(4);
        if(enemyReference.playerReference != null && health > 0)
        {
            enemyReference.HaltEnemy();
            InstantiateEnemyBomb();
            isEnemyCouroutineCalled = true;
        }     
    }

    public void SetHealth(int tempHealth)
    {
        health = tempHealth;
    }

    public void GetDamage()
    {
        health--;
        if (gameObject.CompareTag("Player"))
        {
            CheckDeathPlayer();
        } else
        {
            CheckDeathEnemy();
        }
    }

    void CheckDeathEnemy()
    {
        if(health<1)
        {
            enemyReference.gameManagerReference.RemoveEnemyFromEnemiesAlive(gameObject);
            enemyReference.enemyRb.constraints = RigidbodyConstraints.None;
            if (ragDollReference != null)
            {
                ragDollReference.DeathHasHappened();
            }
            //Destroy(gameObject);
        }
    }

    void CheckDeathPlayer()
    {
        if (health < 1)
        {
            playerControllerReference.playerRb.constraints = RigidbodyConstraints.None;
            if (ragDollReference != null)
            {
                ragDollReference.DeathHasHappened();
            }
            //Destroy(gameObject);
        }
    }
    
    IEnumerator PlayerBombCoolDown()
    {
        if(hasMultiShootPowerup)
        {
            yield return new WaitForSeconds(0f);
        } else
        {
            yield return new WaitForSeconds(1.5f);
        } 
        canThrowBomb = true;
    }

    // Below
    // is
    // PowerUps
    // Logic
    // Only

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("MultiShootPowerup"))
        {
            Destroy(other.gameObject);
            hasMultiShootPowerup = true;
            StartCoroutine(MultiShootCoolDown());
        } else if (other.gameObject.CompareTag("StickyBombPowerup"))
        {
            Destroy(other.gameObject);
            hasNoAbility = false;
            hasMinesPowerup = false;
            hasStickyBombPowerup = true;
            specialBombsInHand = 3;
        } else if (other.gameObject.CompareTag("MinesPowerup"))
        {
            Destroy(other.gameObject);
            hasNoAbility = false;
            hasStickyBombPowerup = false;
            hasMinesPowerup = true;
            specialBombsInHand = 3;
        } else if (other.gameObject.CompareTag("HealthPowerup"))
        {
            Destroy(other.gameObject);
            hasHealthPowerup = true;
        }
    }

    IEnumerator MultiShootCoolDown()
    {
        yield return new WaitForSeconds(10);
        hasMultiShootPowerup = false;
    }

    void HealthPowerUpUsed()
    {
        health++;
        hasHealthPowerup = false;
    }

    void MinesPowerUpUsed()
    {
        GameObject tempBomb = Instantiate(minePrefab, new Vector3(transform.position.x, 3, transform.position.z), minePrefab.transform.rotation);

        if (playerControllerReference != null)
        {
            throwDirection = playerControllerReference.GetThrowDirection();
        }
        Rigidbody tempRb = tempBomb.GetComponent<Rigidbody>();
        //Debug.Log("Throw Direction- x:" + throwDirection.x + " y:" + throwDirection.y + " z:" + throwDirection.z);
        tempRb.AddForce((throwDirection * throwForce), ForceMode.Impulse);
        specialBombsInHand--;
            if(specialBombsInHand < 1)
            {
                hasMinesPowerup = false;
                hasNoAbility = true;
            }
    }

    void StickyBombPowerUpUsed()
    {
        GameObject tempBomb = Instantiate(stickyBombPrefab, new Vector3(transform.position.x, 3, transform.position.z), stickyBombPrefab.transform.rotation);

        if (playerControllerReference != null)
        {
            throwDirection = playerControllerReference.GetThrowDirection();
        }
        Rigidbody tempRb = tempBomb.GetComponent<Rigidbody>();
        //Debug.Log("Throw Direction- x:" + throwDirection.x + " y:" + throwDirection.y + " z:" + throwDirection.z);
        tempRb.AddForce((throwDirection * throwForce), ForceMode.Impulse);
        specialBombsInHand--;
            if (specialBombsInHand < 1)
            {
                hasStickyBombPowerup = false;
                hasNoAbility = true;
            }
    }

    void ImplementedPowerUpSystem()
    {
        if(hasHealthPowerup)
        {
            HealthPowerUpUsed();
        }
        if(hasMinesPowerup)
        {
            MinesPowerUpUsed();
        }
        if(hasStickyBombPowerup)
        {
            StickyBombPowerUpUsed();
        }
        if(hasNoAbility)
        {
            UserControllerBombThrow();
        }
    }

    void HaltPlayer()
    {
        if(gameObject.CompareTag("Player"))
        {
            playerControllerReference.HaltPlayer();
        } else if(gameObject.CompareTag("Enemy"))
        {
            enemyReference.HaltEnemy();
        }
    }

}
