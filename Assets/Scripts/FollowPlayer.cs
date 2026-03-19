using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 50);
    void Update()
    {
        transform.position = Camera.main.transform.position + offset;
    }
}
