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
    [SerializeField] float scale = 1000000.0f;

    private IPEndPoint endpoint;
    private Socket sock;
    private bool hasStarted;

    public string ipAddress = "";
    public Int32 portNum = 0;
    public Vector3 rocketPos;
    public Vector3 planetPos;
    public Vector3 planetSize;
    public Vector3 rocketSize;

    public bool rocketDataRead = false;
    public bool planetDataRead = false;

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
            //Debug.Log("Response: " + response);
            string[] simulationStates = response.Split('\t');

            // Variable values
            if (Convert.ToInt64(simulationStates[0]) == 0)
            {
                rocketPos[0] = (float)Convert.ToDouble(simulationStates[1]) / scale;
                rocketPos[1] = (float)Convert.ToDouble(simulationStates[2]) / scale;
                rocketPos[2] = (float)Convert.ToDouble(simulationStates[3]) / scale;

                rocketSize = new Vector3(10 / scale, 100 / scale, 10 / scale);

                rocketDataRead = true;

                planetPos[0] = 0.0f;
                planetPos[1] = 0.0f;
                planetPos[2] = 0.0f;

                float planetRad = (float)Convert.ToDouble(simulationStates[4]) / scale;

                planetSize = new Vector3(2 * planetRad, 2 * planetRad, 2 * planetRad);

                planetDataRead = true;

                //Debug.Log("Planet radius: " + planetRad.ToString());

                //Debug.Log("Rocket pos: \nx: " + rocketPos[0].ToString() + "\ny: " + rocketPos[1].ToString() + "\nz: " + rocketPos[2].ToString());
            }
            // Var send once
            else if (Convert.ToInt64(simulationStates[0]) == 5)
            {
                
            }
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
            "trick.var_add(\"dyn.rocket.pos[2]\") \n" +
            "trick.var_add(\"dyn.body.radius\") \n" +
            "trick.var_add(\"trick_sys.sched.mode\") \n" +
            "trick.var_unpause()\n";

        sock.Send(Encoding.ASCII.GetBytes(trickVar), SocketFlags.None);
        Debug.Log("--------- Connected to trick Variable Server ---------");
        Debug.Log("IP: " + ipAddress);
        Debug.Log("port: " + portNum);

        hasStarted = true;
    }
}
