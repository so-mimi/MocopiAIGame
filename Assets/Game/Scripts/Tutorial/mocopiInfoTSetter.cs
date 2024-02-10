using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class mocopiInfoTSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mocopiIPAndPortText;
    
    private void Start()
    {
        mocopiIPAndPortText.text = "IP: " + GetLocalIPAddress() + " \nPort: 12351";
    }
    
    private string GetLocalIPAddress()
    {
        string localIP = "";
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
            }
        }
        return localIP;
    }
}
