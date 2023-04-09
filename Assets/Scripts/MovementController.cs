using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//This Class handles moving objects with forces and impulses
public class MovementController : MonoBehaviour
{
    //Public Variables
    public enum MovementStatus
    {
        OnGround,
        Asending,
        Desending
    }
    [Header("Movement")]
    public float MaxSpeed = 10;
    public float maxCurrentSpeed = 10;
    public EnumerableTypes.Direction currentDirection;

    [Header("Jumping")] 
    public MovementStatus currentStatus = MovementStatus.OnGround;
    public float jumpPower;
    public float coyoteTime;
    public float bufferTime;
    public bool jumpBuffered;
    private float jumpCutLimit;

    //Private Variables
    private Rigidbody2D _rb;
    private float _coyoteTimer;
    private float _bufferTimer;
    private void Start()
    {
        //Check if there is a Rigidbody Attached to the game object, if there is none, display a warning.
        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("There is no RigidBody Attached to this Game Object");
        }

        jumpCutLimit = _rb.gravityScale / 2;
    }

    private void FixedUpdate()
    {

        //If we have a jump buffered when we land, we jump
        if (jumpBuffered && currentStatus == MovementStatus.OnGround)
        {
            Jump();
            jumpBuffered = false;
        }

        //If we buffered to early, we don't jump
        //Turn off the buffer effect
        if (_bufferTimer < 0)
        {
            jumpBuffered = false;
        }

        //Handle Status
        //If the rays determine the creature is on the ground the creature is indeed on the ground
        if (OnGround())
        {
            //Update status
            currentStatus = MovementStatus.OnGround;
        }
        //If the creature is not on the ground, it is either asending or desending
        else
        {
            if (_rb.velocity.y > 0)
            {
                currentStatus = MovementStatus.Asending;
            }

            if (_rb.velocity.y < 0)
            {
                currentStatus = MovementStatus.Desending;
            }
        }
        
        //Apply coyote time
        if (currentStatus == MovementStatus.OnGround)
        {
            _coyoteTimer = coyoteTime;
        }
        
        //Considering variable jump Height
        if (gameObject.CompareTag("Player"))
        {
            EndJump();
        }
        
        //Update the current direction
        if (_rb.velocity.x > 0)
        {
            currentDirection = EnumerableTypes.Direction.Right;
            transform.localScale = new Vector3(-2, transform.localScale.y);
        }

        if (_rb.velocity.x < 0)
        {
            currentDirection = EnumerableTypes.Direction.Left;
            transform.localScale = new Vector3(2, transform.localScale.y);
        }
        
        
        //Update Timers
        _coyoteTimer -= Time.deltaTime;
        _bufferTimer -= Time.deltaTime;
    }

    //Handles the creature's ability to jump
    public void HandleJump()
    {
        //If we are on the ground, jump
        if (currentStatus == MovementStatus.OnGround)
        {
            Jump();
        }
        //Otherwise, start a buffer timer
        else
        {
            _bufferTimer = bufferTime;
            jumpBuffered = true;
        }

        //If the creature is not on the ground, but has coyote time, jump
        if (currentStatus == MovementStatus.Desending)
        {
            if (_coyoteTimer > 0)
            {
                Jump();
            }
        }
        //Coyote Time
        //If the creature walks of a ledge, they have a limited amount of time for the jump call still works

        //Jump Buffering
        //If a creature presses the jump function before they hit the ground, the
        
    }

    //This function does necessary math and actually cases the creature to jump; this functions is generally not called directly unless a specific instance does so
    public void Jump()
    {
        float jumpForce = _rb.mass * (jumpPower - Mathf.Abs(_rb.velocity.y)) / (Time.deltaTime);
        //_rb.AddForce(jumpForce * transform.up, ForceMode2D.Force);
        _rb.velocity = new Vector2(_rb.velocity.x, jumpPower + (0.2f * _rb.velocity.x));
        _coyoteTimer = 0;
    }

    //Handles the creature's ability to move and any direction.
    public void Move(Vector2 dir)
    {
        if (currentStatus == MovementStatus.OnGround)
        {
            //Calculate the necessary force
            float force = _rb.mass * (maxCurrentSpeed - Mathf.Abs(_rb.velocity.x)) / (Time.deltaTime * 1) + 1;

            //Apply the force in the correct direction
            _rb.AddForce(force * dir);
        }
        else
        {
            //This isn't perfect, but it'll do for  not
            //TODO: Update with better air controls
            _rb.AddForce(50 * dir);
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -maxCurrentSpeed, maxCurrentSpeed), _rb.velocity.y);
        }

    }

    //Checks whether the creature is on the ground
    //Returns true if a creature is on the ground
    public bool OnGround()
    {
        //Two ray casts are created, if either detects the ground, the creature is on the ground
        var collider = gameObject.GetComponent<BoxCollider2D>();
        //The creature's left foot
        Vector2 leftHit = new Vector2(gameObject.transform.position.x - collider.size.x / 2,
            gameObject.transform.position.y - gameObject.transform.localScale.y / 2 - 0.1f);
        //The creature's right foot
        Vector2 rightHit = new Vector2(gameObject.transform.position.x + collider.size.x / 2,
            gameObject.transform.position.y - gameObject.transform.localScale.y / 2 - 0.1f);
        
        //Cast Left Ray
        RaycastHit2D left = Physics2D.Linecast(gameObject.transform.position, leftHit, 1<<8);
        Debug.DrawLine(gameObject.transform.position, leftHit, Color.red);

        //Cast Right Ray
        RaycastHit2D right = Physics2D.Linecast(gameObject.transform.position, rightHit, 1<<8);
        Debug.DrawLine(gameObject.transform.position, rightHit, Color.yellow);
        
        //If either ray collides with the ground, return true
        if (right || left)
        {
            return true;
        }
        
        return false;
    }

    //Method only available to players
    public void EndJump()
    {
        if (currentStatus == MovementStatus.Asending && !Input.GetKey("space") && _rb.velocity.y > jumpCutLimit)
        {
            //_rb.velocity = new Vector2(_rb.velocity.x, jumpCutLimit);
            _rb.AddForce(-transform.up * _rb.gravityScale * 5);
        }
    }
}
