using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounterScript : MonoBehaviour
{
    [SerializeField]
    Text coinCounter;



    

    // Start is called before the first frame update
    void Start()
    {
    

    }

    // Update is called once per frame
    void Update()
    {
        coinCounter.text = GameManager.Instance.gold.ToString();
    }


}
