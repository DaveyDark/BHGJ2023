using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration, max_velocity,multiplier; 
    private Rigidbody2D rb;
    private Vector2 force = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        // Get attached rigidbody
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        // Get up/down/left/right inputs
        if(Input.GetAxis("Horizontal") != 0){
            force.x = Input.GetAxis("Horizontal");
        }
        if(Input.GetAxis("Vertical") != 0){
            force.y = Input.GetAxis("Vertical");
        }
    }

    void FixedUpdate(){
        // move the body by the inputs
        rb.AddForce(force * Time.deltaTime * multiplier * acceleration);

        // Limit max velocity of the Rigidbody
        Vector2 clampedVelocity = rb.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -max_velocity, max_velocity);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -max_velocity, max_velocity);
        rb.velocity = clampedVelocity;
    }
}
