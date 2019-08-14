using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class CustomNetworkManager : NetworkManager
{
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        GameObject enemyPrefab = spawnPrefabs[0];
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 2, 0), Quaternion.identity);
        NetworkServer.Spawn(enemy);
        Destroy(enemy, 10);
    }
}
