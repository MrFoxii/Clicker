using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
    static GameManager instance;
    public float gold = 0;
    public float goldMax = 0;
    public int dmg = 1;
    public float cost = 10f;
    public Text DmgCost;
    public int vieEnemy = 3;
    public int vieMax = 3;



    // ici tout commence mon garcon.

    public double money;
    public double dpc;
    public double dps;
    public double health;
    public double healthCap
    {
        get
        {
            return 10 * System.Math.Pow(2, stage - 1) * isBoss;
        }
    }
  

    public int stage;
    public int stageMax;
    public int kills;
    public int killsMax;
    public int isBoss;
    public int timerCap;

    public float timer;



    public Text moneyText;
    public Text DPCText;
    public Text DPSText;
    public Text StageText;
    public Text killsText;
    public Text healthText;
    public Text timerText;


    public GameObject back;
    public GameObject forward;


    public Image healthBar;
    public Image timerBar;


    public Animator coinExplosion;


    // offline
    public DateTime currentDate;
    public DateTime oldTime;
    public int OfflineProgressCheck;
    public float idleTime;
    public Text offlineTimeText;
    public float saveTime;
    public GameObject offlineBox;
    public int offlineLoadCount;

    //upgrades

    public Text pCostText;
    public Text pLevelText;
    public Text pPowerText;
    public double pCost
    {
        get
        {
            return 10 * Math.Pow(1.07, pLevel);
        }
    }
    public int pLevel;
    public double pPower
    {
        get
        {
            return 5 * pLevel;
        }
    }

    public Text cCostText;
    public Text cLevelText;
    public Text cPowerText;
    public double cCost
    {
        get
        {
            return 10 * Math.Pow(1.07, cLevel);
        }
    }
    public int cLevel;
    public double cPower
    {
        get
        {
            return 2 * cLevel;
        }
    }

    





    public void Start()
    {
        offlineBox.gameObject.SetActive(false);
        Load();
        IsBoss();
        health = healthCap;
        timerCap = 30;
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(FindObjectOfType<GameManager>());
        //DontDestroyOnLoad(music);

    }


     void Update()
    {



        // ici tout commence mon garcon.

        if (health <= 0)
        {
            Kill();
        }
        else health -= dps * Time.deltaTime;
       

        moneyText.text = "$" + money.ToString("F2");
        StageText.text = "Stage - " + stage;
        killsText.text = kills + "/" + killsMax + "Kills";
        healthText.text = health + "/" + healthCap + "HP";
        DPCText.text =  dpc + "dmg/c";
        DPSText.text =  dps + "dmg/s";

        healthBar.fillAmount = (float)(health / healthCap);

        if(stage > 1)
        {
            back.gameObject.SetActive(true);
        }
        else
        {
            back.gameObject.SetActive(false);
        }

        if (stage != stageMax)
        {
            forward.gameObject.SetActive(true);
        }
        else
        {
            forward.gameObject.SetActive(false);

        }

        IsBoss();
        Upgrades();

        saveTime += Time.deltaTime;
        if(saveTime >= 5)
        {
            saveTime = 0;
            Save();
        }

    }

    public void Upgrades()
    {
        cCostText.text = "Cost : $" + cCost.ToString("F2");
        cLevelText.text = "Level : " + cLevel;
        cPowerText.text = "+2/c";

        pCostText.text = "Cost : $" + pCost.ToString("F2");
        pLevelText.text = "Level : " + pLevel;
        pPowerText.text = "+5/s";
        dps = pPower;
        dpc = 1 + cPower;
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public void IsBoss()
    {
        if (stage % 5 == 0)
        {
            isBoss = 10;
            StageText.text = "BOSS ! (Stage - " + stage + " )";
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Back();
            }
            timerText.text = timer + "/" + timerCap;
            timerBar.gameObject.SetActive(true);
            timerBar.fillAmount = timer / timerCap;
            killsMax = 1;


        }
        else
        {
            isBoss = 1;
            StageText.text = "Stage - " + stage;
            timerText.text = "";
            timerBar.gameObject.SetActive(false);
            timer = 30;
            killsMax = 10;

        }
    }

    public void Hit()
    {
        health -= dpc;
        if(health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        money += Math.Ceiling(healthCap / 14);
        coinExplosion.Play("CoinExplosion", 0, 0);
        if (stage == stageMax)
        {
            kills += 1;
            if (kills >= killsMax)
            {
                kills = 0;
                stage += 1;
                stageMax += 1;
            }
        }
        IsBoss();
        health = healthCap;
        if (isBoss > 1)
        {
            timer = timerCap;
        }
        killsMax = 10;
    }

    public void Back()
    {
        stage -= 1;
        IsBoss();
        health = healthCap;
    }

    public void Forward()
    {
        stage += 1;
        IsBoss();
        health = healthCap;
    }



    public void BuyUpgrades(string id)
    {
        switch (id)
        {
            case "p1":
                if (money >= pCost)
                {
                    UpgradeDefault(ref pLevel,  pCost);
                }
                break;

            case "c1":
                if (money >= cCost)
                {
                    UpgradeDefault(ref cLevel,  cCost);
                }
                break;
        }
    }

    public void UpgradeDefault(ref int level,  double cost)
    {
        money -= cost;
        level++;
        
    }

    public void Save()
    {
        OfflineProgressCheck = 1;

        PlayerPrefs.SetString("money", money.ToString());
        PlayerPrefs.SetString("dpc", dpc.ToString());
        PlayerPrefs.SetString("dps", dps.ToString());
        PlayerPrefs.SetInt("stage", stage);
        PlayerPrefs.SetInt("stageMax", stageMax);
        PlayerPrefs.SetInt("kills", kills);
        PlayerPrefs.SetInt("killsMax", killsMax);
        PlayerPrefs.SetInt("isBoss", isBoss);
        PlayerPrefs.SetInt("cLevel", cLevel);
        PlayerPrefs.SetInt("pLevel", pLevel);
        PlayerPrefs.SetInt("OfflineProgressCheck", OfflineProgressCheck);


        PlayerPrefs.SetString("OfflineTime", DateTime.Now.ToBinary().ToString());
        
    }

    public void Load()
    {
        money = double.Parse(PlayerPrefs.GetString("money", "0"));
        dpc = double.Parse(PlayerPrefs.GetString("dpc", "1"));
        dps = double.Parse(PlayerPrefs.GetString("dps", "0"));
        stage = PlayerPrefs.GetInt("stage", 1);
        stageMax = PlayerPrefs.GetInt("stageMax", 1);
        kills = PlayerPrefs.GetInt("kills", 0);
        killsMax = PlayerPrefs.GetInt("killsMax", 10);
        isBoss = PlayerPrefs.GetInt("isBoss", 1);
        pLevel = PlayerPrefs.GetInt("pLevel", 0);
        cLevel = PlayerPrefs.GetInt("cLevel", 0);
        OfflineProgressCheck = PlayerPrefs.GetInt("OfflineProgressCheck", 0);
        LoadOffline();
    }

    public void LoadOffline()
    {
        if(OfflineProgressCheck == 1)
        {
            offlineBox.gameObject.SetActive(true);
            long previousTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            oldTime = DateTime.FromBinary(previousTime);
            currentDate = DateTime.Now;
            TimeSpan difference = currentDate.Subtract(oldTime);
            idleTime = (float)difference.TotalSeconds;

            var moneyToEarn = Math.Ceiling(healthCap / 14 / healthCap) * (dps / 5) * idleTime;
            money += moneyToEarn;

            TimeSpan timer = TimeSpan.FromSeconds(idleTime);
            offlineTimeText.text = "You were gone for " + timer.ToString(@"hh\:mm\:ss") + "\n\nYou earned : $" + moneyToEarn.ToString("F2");
        }
    }

    public void CloseOfflineBox()
    {
        offlineBox.gameObject.SetActive(false);
    }
}
