using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : MonoBehaviour
{
    public float bombTimer = 3;
    public float blastForce = 1000;
    public float blastRadius = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BombTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(bombTimer);
        BombBlast();
        Destroy(gameObject);
    }

    void BombBlast()
    {
        Vector3 blastPoint = transform.position;
        Collider[] colliders = Physics.OverlapSphere(blastPoint, blastRadius);
        foreach (Collider inRangeObject in colliders)
        {
            Rigidbody tempRb = inRangeObject.GetComponent<Rigidbody>();
            if (tempRb != null)
            {
                tempRb.AddExplosionForce(blastForce, blastPoint, blastRadius);
            }
            PowerScript tempPowerScript = inRangeObject.GetComponent<PowerScript>();
            if (tempPowerScript != null)
            {
                tempPowerScript.GetDamage();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = collision.gameObject.transform;
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    GetComponent<Rigidbody>().isKinematic = true;
        //    transform.parent = collision.gameObject.transform;
        //}
    }
}
