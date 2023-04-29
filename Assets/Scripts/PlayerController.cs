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
    [Header("Animation")]
    public Animator animator;

    //privates
    private Rigidbody2D rb;
    private bool can_dash;
    private Vector2 force = new Vector2();
    private float dash_timer = 0f;
    private float initialDrag;
    private int state; //tracks elemental state of player

    void Start()
    {
        // Get attached rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        initialDrag = rb.drag;
        state = 0;
        // 0 is air
        // 1 is water
        // 2 is fire
        // 3 is earth
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
        if(Input.GetButtonDown("State1")) state = 0;
        else if(Input.GetButtonDown("State2")) state = 1;
        else if(Input.GetButtonDown("State3")) state = 2;
        else if(Input.GetButtonDown("State4")) state = 3;
        
        animator.SetInteger("state",state);
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


    private void OnTriggerEnter2D( Collider2D collision )
     {  
        GameObject other = collision.gameObject;
        if (other.CompareTag("Terrain"))
        {
            var terrain = other.GetComponent<Terrain>();
            rb.drag = terrain.getDrag();
            terrain.Activate();
        }
     }

     private void OnTriggerExit2D( Collider2D collision )
     {
         GameObject other = collision.gameObject;
         if (other.CompareTag("Terrain"))
         {  
            rb.drag = initialDrag;
            other.GetComponent<Terrain>().Deactivate();
         }
     }
}
