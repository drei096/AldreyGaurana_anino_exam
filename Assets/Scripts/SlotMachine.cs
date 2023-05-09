using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{

    //reels count
    [HideInInspector] public int reels = 5;
    //rows count
    [HideInInspector] public int rows = 3;

    //2D array holder for slot machine result
    [HideInInspector] public string[,] slotResult = new string[3,5];

    //line combination list
    private List<int[]> lineCombinationList = new List<int[]>();

    //symbol position list
    private List<Vector3> symbolPositionList= new List<Vector3>();

    //bool var to check if spin button is pressed
    [HideInInspector] public bool isSpinning = false;

    //game object reference to spin/stop buttons
    [SerializeField] private GameObject spinButton;
    [SerializeField] private GameObject stopSpinButton;

    //game object references to reels
    [SerializeField] private GameObject[] reelList;

    //bet amount
    private int totalBetAmount;
    [SerializeField] private GameObject betText;
    //bet interval
    private int betInterval = 250;

    //main player reference
    private MainPlayer mainPlayerRef= null;

    //payouts dictionary
    Dictionary<string, int[]> payoutsDictionary = new Dictionary<string, int[]>();

    //total payout
    int totalPayout = 0;

    //payout data info text object
    public GameObject payoutDataInfo;

    public void startSpin()
    {
        if(isSpinning == false)
        {
            //reduce the total coins acc to bet amount
            mainPlayerRef.totalCoins -= totalBetAmount;
            mainPlayerRef.setTotalCoinsText();

            //reset total winnings to 0 for every new spin
            mainPlayerRef.totalWinnings = 0;
            mainPlayerRef.setTotalWinningsText();

            isSpinning = true;
            spinButton.SetActive(false);
            stopSpinButton.SetActive(true);

            foreach(var reel in reelList)
            {
                StartCoroutine(reel.GetComponent<AReel>().spinReel());
            }

        }
    }

    public void stopSpin()
    {
        if(isSpinning == true)
        {
            isSpinning = false;
            spinButton.SetActive(true);
            stopSpinButton.SetActive(false);

            foreach (var reel in reelList)
            {
                StopCoroutine(reel.GetComponent<AReel>().spinReel());
            }

            getSlotResult();
            checkLineCombinations();
            calculatePayout();

            totalPayout = 0;
        }
    }

    private void InitLineCombinationList()
    {
        lineCombinationList.Add(new[] { 0, 0, 0, 0, 0 });
        lineCombinationList.Add(new[] { 1, 1, 1, 1, 1 });
        lineCombinationList.Add(new[] { 2, 2, 2, 2, 2 });
        lineCombinationList.Add(new[] { 0, 1, 2, 1, 0 });
        lineCombinationList.Add(new[] { 2, 1, 0, 1, 2 });
        lineCombinationList.Add(new[] { 2, 1, 0, 1, 0 });
        lineCombinationList.Add(new[] { 0, 1, 0, 1, 2 });
        lineCombinationList.Add(new[] { 2, 0, 0, 0, 2 });
        lineCombinationList.Add(new[] { 1, 0, 0, 0, 1 });
        lineCombinationList.Add(new[] { 1, 0, 0, 0, 0 });
        lineCombinationList.Add(new[] { 2, 0, 0, 0, 0 });
        lineCombinationList.Add(new[] { 0, 1, 0, 0, 0 });
        lineCombinationList.Add(new[] { 0, 2, 0, 0, 0 });
        lineCombinationList.Add(new[] { 0, 0, 1, 0, 0 });
        lineCombinationList.Add(new[] { 0, 0, 2, 0, 0 });
        lineCombinationList.Add(new[] { 0, 0, 0, 1, 0 });
        lineCombinationList.Add(new[] { 0, 0, 0, 2, 0 });
        lineCombinationList.Add(new[] { 0, 0, 0, 0, 1 });
        lineCombinationList.Add(new[] { 0, 0, 0, 0, 2 });
        lineCombinationList.Add(new[] { 2, 0, 0, 0, 0 });
    }

    private void InitPositionsList()
    {
        symbolPositionList.Add(new Vector3(-4, 15, 0));
        symbolPositionList.Add(new Vector3(-4, 13.5f, 0));
        symbolPositionList.Add(new Vector3(-4, -1.5f, 0));
        symbolPositionList.Add(new Vector3(-4, 0, 0));
        symbolPositionList.Add(new Vector3(-4, 1.5f, 0));
        symbolPositionList.Add(new Vector3(-4, 3, 0));
        symbolPositionList.Add(new Vector3(-4, 4.5f, 0));
        symbolPositionList.Add(new Vector3(-4, 6, 0));
        symbolPositionList.Add(new Vector3(-4, 7.5f, 0));
        symbolPositionList.Add(new Vector3(-4, 9, 0));
        symbolPositionList.Add(new Vector3(-4, 10.5f, 0));
        symbolPositionList.Add(new Vector3(-4, 12, 0));
    }

    private void randomizePositions()
    {
        int reelNumber = 0;

        //iterate for each reel
        foreach(var reel in reelList)
        {
            //loop on the reel's symbol list
            for (int i = 0; i < reel.GetComponent<AReel>().symbolList.Count; i++)
            {
                //get a random number within the indices of the position list
                int randomPosIndex = UnityEngine.Random.Range(0, symbolPositionList.Count);

                //assign that random number index to the position of the symbol
                if(reelNumber > 0)
                    reel.GetComponent<AReel>().symbolList[i].transform.position = new Vector3(symbolPositionList[randomPosIndex].x + (2.0f * reelNumber), symbolPositionList[randomPosIndex].y, symbolPositionList[randomPosIndex].z);
                symbolPositionList.RemoveAt(randomPosIndex);
            }


            //refill positions list when all is used at a reel
            reelNumber++;
            InitPositionsList();
        }
    }

    private void getSlotResult()
    {
        foreach(var reel in reelList)
        {
            reel.GetComponent<AReel>().setResultColumn();
        }

        
        for (int i = 0; i < slotResult.GetLength(0); i++)
        {
            string rowString = "";

            // Iterate over the columns of the array
            for (int j = 0; j < slotResult.GetLength(1); j++)
            {
                // Add the current element to the row string
                rowString += slotResult[i, j] + " ";
            }

            // Print out the row string
            Debug.Log(rowString);
        }
        
    }

    private void checkLineCombinations()
    {
        foreach(var lineCombination in lineCombinationList)
        {
            //holder for current row sequence and most common symbol counts
            List<string> curr_sequence = new List<string>();
            Dictionary<string, int> counts = new Dictionary<string, int>();

            //iterate per line combination element of the slot result then add it to the curr_sequence 
            for (int i = 0; i < lineCombination.Length; i++)
            {
                curr_sequence.Add(slotResult[lineCombination[i], i]);
            }

            //iterate each element in sequence to get their occurrence count
            foreach (string item in curr_sequence)
            {
                if (counts.ContainsKey(item))
                {
                    counts[item]++;
                }
                else
                {
                    counts[item] = 1;
                }
            }
            // find the element with the highest count
            string mostRepeatedElement = "";
            int maxCount = 0;

            foreach (KeyValuePair<string, int> pair in counts)
            {
                if (pair.Value > maxCount)
                {
                    mostRepeatedElement = pair.Key;
                    maxCount = pair.Value;
                }
            }

            //get most common symbol
            string mostCommonSymbol = mostRepeatedElement;
            totalPayout += payoutsDictionary[mostCommonSymbol][maxCount - 1];

        }
    }

    private void calculatePayout()
    {
        mainPlayerRef.totalWinnings = totalPayout * totalBetAmount;
        mainPlayerRef.setTotalWinningsText();
        mainPlayerRef.totalCoins += mainPlayerRef.totalWinnings;
        mainPlayerRef.setTotalCoinsText();
    }

    public void addBet()
    {
        if((totalBetAmount + betInterval) < mainPlayerRef.totalCoins)
        {
            totalBetAmount += betInterval;
            betText.GetComponent<TextMeshProUGUI>().text = totalBetAmount.ToString();
        }
    }

    public void decreaseBet()
    {
        if(totalBetAmount - betInterval > 0)
        {
            totalBetAmount -= betInterval;
            betText.GetComponent<TextMeshProUGUI>().text = totalBetAmount.ToString();
        }
    }

    private void InitializePayoutsDictionary()
    {
        foreach (var symbol in reelList[0].GetComponent<AReel>().symbolList)
        {
            payoutsDictionary[symbol.GetComponent<ASymbol>().name] = symbol.GetComponent<ASymbol>().payouts;
        }
    }

    private void InitInfoPayoutData()
    {
        string text = "Payout Lines/Rewards: \n\n";
        // iterate each of the symbols and add payout data text
        // add text
        text += $"For Symbol 1 (3-5 streak): " + payoutsDictionary["A"][2].ToString() + "," + payoutsDictionary["A"][3].ToString() + "," + payoutsDictionary["A"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 2 (3-5 streak): " + payoutsDictionary["B"][2].ToString() + "," + payoutsDictionary["B"][3].ToString() + "," + payoutsDictionary["B"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 3 (3-5 streak): " + payoutsDictionary["C"][2].ToString() + "," + payoutsDictionary["C"][3].ToString() + "," + payoutsDictionary["C"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 4 (3-5 streak): " + payoutsDictionary["D"][2].ToString() + "," + payoutsDictionary["D"][3].ToString() + "," + payoutsDictionary["D"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 5 (3-5 streak): " + payoutsDictionary["E"][2].ToString() + "," + payoutsDictionary["E"][3].ToString() + "," + payoutsDictionary["E"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 6 (3-5 streak): " + payoutsDictionary["F"][2].ToString() + "," + payoutsDictionary["F"][3].ToString() + "," + payoutsDictionary["F"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 7 (3-5 streak): " + payoutsDictionary["G"][2].ToString() + "," + payoutsDictionary["G"][3].ToString() + "," + payoutsDictionary["G"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 8 (3-5 streak): " + payoutsDictionary["H"][2].ToString() + "," + payoutsDictionary["H"][3].ToString() + "," + payoutsDictionary["H"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 9 (3-5 streak): " + payoutsDictionary["I"][2].ToString() + "," + payoutsDictionary["I"][3].ToString() + "," + payoutsDictionary["I"][4].ToString();
        text += "\n\n";
        // add text
        text += $"For Symbol 10 (3-5 streak): " + payoutsDictionary["J"][2].ToString() + "," + payoutsDictionary["J"][3].ToString() + "," + payoutsDictionary["J"][4].ToString();
        text += "\n\n";

        payoutDataInfo.GetComponent<TextMeshProUGUI>().text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainPlayerRef = FindObjectOfType<MainPlayer>();

       
        InitLineCombinationList();
        InitPositionsList();

        //BUGGY, NEED TO FIX RANDOMIZE POSITIONS FUNCTION SUCH THAT THE POSITION ORDER OF SYMBOLS WILL NOT BE AFFECTED
        //randomizePositions();

        InitializePayoutsDictionary();
        InitInfoPayoutData();

        //initial bet amount
        betInterval = betInterval * lineCombinationList.Count;
        totalBetAmount = 1000 * lineCombinationList.Count;
        betText.GetComponent<TextMeshProUGUI>().text = totalBetAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
