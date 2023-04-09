using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestControllerScript : MonoBehaviour
{
    private Creature _creature;
    private MovementController _movement;
    private WeaponController _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _creature = gameObject.GetComponent<Creature>();
        _movement = gameObject.GetComponent<MovementController>();
        _weapon = gameObject.GetComponent<WeaponController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _movement.HandleJump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            _weapon.PrimaryAttack();
        }
        if (Input.GetKeyDown("s"))
        {
            _weapon.SecondaryAttack();
        }

    }

    void FixedUpdate()
    {
        
        if (Input.GetKey("a"))
        {
            _movement.Move(-transform.right);
        }
        
        if (Input.GetKey("d"))
        {
            _movement.Move(transform.right);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movement.maxCurrentSpeed = _movement.MaxSpeed + 5;
        }
        else
        {
            _movement.maxCurrentSpeed = _movement.MaxSpeed;
        }
        
    }

    private void OnDrawGizmos()
    {
        Vector2 pos = gameObject.transform.position;
        Vector2 attackOrigin = new Vector2(pos.x - gameObject.transform.localScale.x / 2, pos.y);
        //Gizmos.DrawWireCube(attackOrigin, new Vector2(2, gameObject.transform.localScale.y));
    }
}
