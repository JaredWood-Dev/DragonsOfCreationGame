using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour
{
    public GameObject target;
    
    void Update()
    {
        if (target != null)
        {
            gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y,
                gameObject.transform.position.z);
        }
    }
}
