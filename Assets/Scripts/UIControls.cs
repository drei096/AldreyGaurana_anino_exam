using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public GameObject infoPanel;

    public void OpenCloseInfoPanel()
    {
        if(infoPanel.activeInHierarchy == false)
            infoPanel.SetActive(true);
        else
            infoPanel.SetActive(false);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
