using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration;
    public float max_velocity,multiplier; 
    [Header("Dash")]
    public float dash_force;
    public float dash_multiplier, dash_cooldown;
    [Header("Trail")]
    public TrailRenderer trail;
    public Color trail_color,dash_trail_color;

    //privates
    private Rigidbody2D rb;
    private bool can_dash;
    private Vector2 force = new Vector2();
    private float dash_timer = 0f;
    private float initialDrag;

    void Start()
    {
        // Get attached rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        initialDrag = rb.drag;

    }

    void Update(){
        //update dash timer
        if(!can_dash)dash_timer -= Time.deltaTime;
        if(dash_timer < 0f) can_dash = true;

        //change trail colors
        if(can_dash)trail.startColor = trail_color;
        else trail.startColor = dash_trail_color;

        // Get up/down/left/right inputs
        if(Input.GetAxis("Horizontal") != 0){
            force.x = Input.GetAxis("Horizontal");
        }
        if(Input.GetAxis("Vertical") != 0){
            force.y = Input.GetAxis("Vertical");
        }
        //dashing
        if(Input.GetButtonDown("Jump") && can_dash) {
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * dash_force * dash_multiplier, ForceMode2D.Impulse);
            can_dash = false;
            dash_timer = dash_cooldown;
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


    private void OnCollisionEnter2D( Collision2D collision )
     {  
        
        GameObject other = collision.gameObject;
        if (other.CompareTag("Terrain"))
        {
            var terrain = other.GetComponent<Terrain>();
            rb.drag = terrain.getDrag();
            terrain.Activate();
        }
    
    
        
     }

     private void OnCollisionExit2D( Collision2D collision )
     {
         GameObject other = collision.gameObject;
         if (other.CompareTag("Terrain"))
         {  
            rb.drag = initialDrag;
            other.GetComponent<Terrain>().Deactivate();
         }
     }
}
