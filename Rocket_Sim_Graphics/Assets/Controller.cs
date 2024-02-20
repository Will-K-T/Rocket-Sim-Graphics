using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject rocketModel;
    [SerializeField] GameObject planetModel;

    [SerializeField] ConnectToTrick trickConnector;
    [SerializeField] FollowRocket cam;

    GameObject rocket = null;
    GameObject planet = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trickConnector.planetDataRead)
        {
            if (planet == null)
            {
                CreatePlanet(trickConnector.planetPos, trickConnector.planetSize);
            }
            else
            {
                planet.transform.position = trickConnector.planetPos;
            }
        }

        if (trickConnector.rocketDataRead)
        {
            if (rocket == null)
            {
                CreateRocket(trickConnector.rocketPos, trickConnector.rocketSize);
                cam.camTarget = rocket;
            }
            else
            {
                Vector3 currPos = trickConnector.rocketPos;
                rocket.transform.position = new Vector3(currPos.x, currPos.y + rocket.transform.localScale.y/2, currPos.z);
            }
        }
    }

    void CreateRocket(Vector3 pos, Vector3 size)
    {
        rocket = Instantiate(rocketModel, pos, new Quaternion());
        rocket.transform.localScale = size;
    }

    void CreatePlanet(Vector3 pos, Vector3 size)
    {
        planet = Instantiate(planetModel, pos, new Quaternion());
        planet.transform.localScale = size;
    }
}
