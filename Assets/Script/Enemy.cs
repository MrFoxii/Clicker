using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //public int vie = 10;
    //public Player Player;
    public Animator anim;
    private float Min;
    private float Max;
    private int counter = 10;
    private Vector3 InitialPos;
    private float timer = 0.01f;
    private bool timerstart;
    public bool isDead = false;
    public GameObject Coin;
    public int NombreDeCoin = 3;
    public Vector2[] positions;




    public GameObject spawn;
    public GameObject PrefabEnemy;
    public bool isSpawned = false;

    private float timerRespawn = 0.5f;







    // Start is called before the first frame update
    void Start()
    {
        //Player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();

        InitialPos = transform.position;
        spawn = GameObject.Find("Spawn");
        isDead = false;


    }



    // Update is called once per frame
    void Update()
    {

        OnDestroy();

        isSpawned = true;
     
        if (timerstart)
        {
            timer -= Time.deltaTime;

        }

        if (timer <= 0)
        {

            gameObject.transform.position = gameObject.transform.position + new Vector3(Random.Range(Min, Max), Random.Range(Min, Max));
            counter--;
            timer = 0.01f;

        }

        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.vieEnemy -= GameManager.Instance.dmg;
            anim.SetTrigger("Hit");
            timerstart = true;

        }

        if (counter == 0)
        {
            timerstart = false;
            transform.position = InitialPos;
            counter = 10;
        }


        if (isDead == false)
        {
            checkEnemyisKilled();
            if (isDead == true)
            {
                EnemyDeath();
            }

        }
        print(timerRespawn);

    }

    void EnemyDeath()
    {
       
        for (int i = 0; i < positions.Length; i++)
        {
            Instantiate(Coin, positions[i], transform.rotation);
        }
        
    }


    void checkEnemyisKilled()
    { 
        if (GameManager.Instance.vieEnemy <= 0)
        {
            anim.SetTrigger("Death");
            isDead = true;
            isSpawned = false;
           
            Destroy(gameObject, 1f);
            

        }
    }




    public void OnDestroy()
    {

       
        if (GameManager.Instance.vieEnemy <= 0 && isSpawned == false)
        {
           
            

            
                Instantiate(PrefabEnemy, spawn.transform.position, transform.rotation);
                GameManager.Instance.vieMax += 3;
                GameManager.Instance.vieEnemy = GameManager.Instance.vieMax;
                isSpawned = true;
                isDead = false;

                if (isDead == true)
                {
                    isDead = false;
                }
            
        }

        
    }
}

