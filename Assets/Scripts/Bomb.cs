using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
public class Bomb : MonoBehaviour
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);
        
    }

    [Command]
    void CmdExplode(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);
        foreach(var hit in hits)
        {
            NetworkServer.Destroy(hit.gameObject);
        }
    }
}
