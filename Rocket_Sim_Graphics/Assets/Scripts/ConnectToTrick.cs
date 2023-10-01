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

    public string ipAddress;
    public ushort portNum;
    public Vector3 rocketPos;
    public Vector3 planetPos;

    // Start is called before the first frame update
    void Start()
    {
        IPAddress ip = new IPAddress(Encoding.ASCII.GetBytes(ipAddress));

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
    }

    // Update is called once per frame
    async void Update()
    {
        var buffer = new byte[1024];
        var received = sock.Receive(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        Debug.Log("Response: " + response);
    }
}
