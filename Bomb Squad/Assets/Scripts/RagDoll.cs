using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    private Enemy enemyReference;
    private PlayerController playerControllerReference;
    private PowerScript powerScriptReference;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerReference(PlayerController tempParticipantReference)
    {
        playerControllerReference = tempParticipantReference;
    }

    public void SetEnemyReference(Enemy tempParticipantReference)
    {
        enemyReference = tempParticipantReference;
    }

    public void SetPowerScriptReference(PowerScript tempPowerScriptReference)
    {
        powerScriptReference = tempPowerScriptReference;
    }

    public void DeathHasHappened()
    {
        GetComponent<Rigidbody>().AddExplosionForce(200, transform.position, 0.5f);
        powerScriptReference.enabled = false;
        if (gameObject.CompareTag("Player"))
        {
            playerControllerReference.enabled = false;
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            enemyReference.enabled = false;
        }
        StartCoroutine(DeathCountDown());
    }

    IEnumerator DeathCountDown()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
