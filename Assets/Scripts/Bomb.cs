using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
public class Bomb : NetworkBehaviour
{
    public float explosionDelay = 2;
    public float explosionRadius = 2;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode()); // Starts explosion timer
    }
    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay); // Short delay before exploding
        CmdExplode(transform.position, explosionRadius); // Explodes
        
    }

    [Command]
    void CmdExplode(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius); // Checks for objects in blast radius and destroys them
        foreach(var hit in hits)
        {
            if (hit.CompareTag("Player") == false) // Checks if object is player, if true don't destroy
            {
                NetworkServer.Destroy(hit.gameObject);
            }
            
        }
    }
}
