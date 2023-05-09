using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASymbol : MonoBehaviour
{
    //initial data for symbol
    public int id;

    /* for names, assign a-j for 1-10
     * A - 1
     * B - 2
     * C - 3
     * D - 4
     * E - 5
     * F - 6
     * G - 7
     * H - 8
     * I - 9
     * J - 10
    */
    public string name;

    public int[] payouts;

    //owner reel
    [SerializeField] private GameObject ownerReel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
