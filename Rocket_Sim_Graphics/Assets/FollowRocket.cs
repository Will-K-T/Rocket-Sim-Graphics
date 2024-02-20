using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRocket : MonoBehaviour
{
    public GameObject camTarget = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (camTarget != null)
        {
            Vector3 obj = camTarget.transform.position;
            transform.position = new Vector3(obj.x, obj.y, obj.z-200);
        }
    }
}
