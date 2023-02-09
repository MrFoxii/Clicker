using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject coinBank;
    
   

    private float timer = 0f;




    // Start is called before the first frame update
    void Start()
    {
        coinBank = GameObject.Find("CoinBank");
    }

    // Update is called once per frame
    void Update()
    {

            transform.position = Vector2.Lerp(transform.position, coinBank.transform.position, timer/6);
        

        if (timer < 1)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CoinBank"))
        {
            GameManager.Instance.gold += Random.Range(2, 3);
            Destroy(gameObject);

        }
    }


}
