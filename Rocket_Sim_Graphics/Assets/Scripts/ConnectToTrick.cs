using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

public class ConnectToTrick : MonoBehaviour
{
    private IPEndPoint endpoint;
    private Socket sock;
    private bool hasStarted;

    public string ipAddress = "";
    public Int32 portNum = 0;
    public Vector3 rocketPos;
    public Vector3 planetPos;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;   
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            var buffer = new byte[1024];
            var received = sock.Receive(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            Debug.Log("Response: " + response);
        }

        
    }

    public void ConnectToServer()
    {
        IPAddress ip = IPAddress.Parse(ipAddress);

        endpoint = new IPEndPoint(ip, portNum);

        sock = new(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        sock.Connect(endpoint);

        string trickVar = "trick.var_set_client_tag(\"rocketclient\")\n" +
            "trick.var_pause()\n" +
            "trick.var_ascii()\n" +
            "trick.var_add(\"dyn.rocket.pos[0]\")\n" +
            "trick.var_add(\"dyn.rocket.pos[1]\") \n" +
            "trick.var_add(\"trick_sys.sched.mode\") \n" +
            "trick.var_unpause()\n";

        sock.Send(Encoding.ASCII.GetBytes(trickVar), SocketFlags.None);
        Debug.Log("--------- Connected to trick Variable Server ---------");
        Debug.Log("IP: " + ipAddress);
        Debug.Log("port: " + portNum);

        hasStarted = true;
    }
}
