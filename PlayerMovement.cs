using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;


    //var for IsGround class
    public LayerMask jumpableGround;

    //var for X axis direction
    float dirX = 0f;

    //speed player moves
    public float moveSpeed = 7f;

    //speed player jumps
    public float jumpForce = 14f;



    //deals with animation
    private enum MovementState { idle, running, jumping, falling}

    public AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
     private void Update()
     {
         //X axis direction (remove Raw if movement slide is wanted)
         dirX = Input.GetAxis("Horizontal");

         //left and right movement
         rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);



        //checks if space is pressed
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {

            //plays jump sound
            jumpSoundEffect.Play();

            // Jump Height
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

         //calls animation class
         UpdateAnimationState();
     }

    

    //whole method for checking animations
    private void UpdateAnimationState()
    {
        MovementState state;

        //running animation code
        
        //forward
        if (dirX > 0f)
        {
            //changes enum value
            state = MovementState.running;

            //flips charater the right way when running
            sprite.flipX = false;
        }
        
        //backward
        else if (dirX < 0f)
        {
            //makes parameter true
            state = MovementState.running;

            //flips charater the right way when running
            sprite.flipX = true;
        }

        //not moving
        else
        {
            //makes parameter true
            state = MovementState.idle;
            
        }
        
        //jumping animation code

        //jumping
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }

        //falling
        else if (rb.velocity.y < -0.1f)
        {
            state= MovementState.falling;
        }
        
        //the actual inter change in enum
        anim.SetInteger("state", (int) state);
    }

    //checks if player is touching ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}
