using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    [HideInInspector] public int totalCoins;
    [HideInInspector] public int totalWinnings;

    [SerializeField] private GameObject totalCoinsText;
    [SerializeField] private GameObject totalWinningsText;

    // Start is called before the first frame update
    void Start()
    {
        totalCoins = 1000000;
        totalWinnings = 0;

        totalCoinsText.GetComponent<TextMeshProUGUI>().text = "Coins: " + totalCoins.ToString(); 
        totalWinningsText.GetComponent<TextMeshProUGUI>().text = totalWinnings.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTotalCoinsText()
    {
        totalCoinsText.GetComponent<TextMeshProUGUI>().text = "Coins: " + totalCoins.ToString();
    }
    public void setTotalWinningsText()
    {
        totalWinningsText.GetComponent<TextMeshProUGUI>().text = totalWinnings.ToString();
    }
}
