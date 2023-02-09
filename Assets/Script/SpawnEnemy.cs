using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject spawn;
    public GameObject Enemy;
    public bool canSpawn = false;
    public bool isSpawned = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Spawn();
    }

    void OnDestroy()
    {
        if(GameManager.Instance.vieEnemy <= 0 && isSpawned == false)
        {
            canSpawn = true;
            if(canSpawn == true)
            {
                
                Instantiate(Enemy, spawn.transform.position, transform.rotation);
                GameManager.Instance.vieEnemy += 3;
                canSpawn = false;
                isSpawned = true;

            }
        }
    }

    
        
    

    
}
