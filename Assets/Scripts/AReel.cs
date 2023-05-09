using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class AReel : MonoBehaviour
{
    //holder for symbol objects
    public List<GameObject> symbolList = new List<GameObject>();

    //holder for reel slot objects
    [SerializeField] private List<GameObject> slotObjects = new List<GameObject>();

    //holder for vector3 locations of slots of reel
    private List<Vector3> slotLocations = new List<Vector3>();

    //symbol list index
    private int symbolListIndex = 0;

    //list for slot contents
    private List<string> symbolInSlotList= new List<string>();

    //reference to the slot machine class
    private SlotMachine slotMachineRef = null;

    //column indicator for reel
    [SerializeField] private int reelColumn;

    //time interval of spin
    private float timeInterval = 0.025f;
    //reference to reel symbol set
    [SerializeField] private GameObject reelSymbol;

    //reference to reel y pos
    float reelYPos;



    // Start is called before the first frame update
    void Start()
    {
        //get the reference of the slot machine
        slotMachineRef = FindObjectOfType<SlotMachine>();
        if (slotMachineRef == null)
            Debug.LogError("No SlotMachine class reference found!");


        //initially clear the slot location list
        slotLocations.Clear();

        //populate the slot location list with transform positions from the slot objects list 
        for(int i = 0; i < slotObjects.Count; i++)
        {
            slotLocations.Add(slotObjects[i].transform.position);
        }
    }





    // Update is called once per frame
    void Update()
    {



    }

    public void setResultColumn()
    {
        //BUGGY
        //TO DO: FIX REEL SYMBOL LOCAL Y POSITION ASSIGNMENT, DOES NOT MATCH TRANSFORM POSITION OF "REEL1SYMBOLS" IN HIERARCHY
        float reelYPos = reelSymbol.transform.localPosition.y;
        float startingIndex = Mathf.Abs(reelYPos/1.5f);
        int i_startingIndex = (int)startingIndex;

        Debug.Log($"{reelSymbol.name} starting index:" + reelSymbol.transform.localPosition.y);

        slotMachineRef.slotResult[0, reelColumn] = symbolList[i_startingIndex - 2].GetComponent<ASymbol>().name;
        slotMachineRef.slotResult[1, reelColumn] = symbolList[i_startingIndex - 1].GetComponent<ASymbol>().name;
        slotMachineRef.slotResult[2, reelColumn] = symbolList[i_startingIndex].GetComponent<ASymbol>().name;
    }


    public IEnumerator spinReel()
    {
        while (slotMachineRef.isSpinning == true)
        {
            for(int i = 0; i < 30; i++)
            {
                //if the symbol list position nearly reaches the end
                if(reelSymbol.transform.localPosition.y < -12.0f)
                {
                    //reset back to the starting location
                    reelSymbol.transform.localPosition = new Vector3(reelSymbol.transform.localPosition.x, 0.0f, reelSymbol.transform.localPosition.z);
                }

                //decrement and make symbol list go down
                reelSymbol.transform.localPosition = new Vector3(reelSymbol.transform.localPosition.x, reelSymbol.transform.localPosition.y - 1.5f, reelSymbol.transform.localPosition.z);

                yield return new WaitForSeconds(timeInterval);
            }

        }
    }
}
