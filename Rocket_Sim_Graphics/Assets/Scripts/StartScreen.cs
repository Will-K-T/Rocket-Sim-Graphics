using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StartScreen : MonoBehaviour
{
    public GameObject trickConnector;
    public TextMeshProUGUI ipAddr;
    public TextMeshProUGUI portNum;
    public TextMeshProUGUI errorMessage;
    public GameObject rocket;

    private ConnectToTrick trick;
    // Start is called before the first frame update
    void Start()
    {
        trick = trickConnector.GetComponent<ConnectToTrick>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSim()
    {
        if (ipAddr.text.Length <= 1)
        {
            errorMessage.text = "ERROR: invalid IP Address";
            
            return; 
        }

        string text_ = portNum.text.Remove(portNum.text.Length - 1);
     
        if(!int.TryParse(text_, out int port))
        {
            errorMessage.text = "ERROR: invalid Port Number";

            return;

        }
        trick.ipAddress = ipAddr.text;
        trick.portNum = port;
        errorMessage.text = "";
        gameObject.SetActive(false);

        trick.ConnectToServer();
        rocket.SetActive(true);
    }
    
}
