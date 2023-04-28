using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float speed;
    private GameObject player;

    void Start(){
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,player.transform.position + offset, speed * Time.deltaTime );
    }
}
