using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This class contains all the information and methods that are used by every creature, including the player
public class Creature : MonoBehaviour
{
    //Public Variables for Inscpector Use
    public int health;
    public float speed;
    public EnumerableTypes.DamageTypes[] resistances;
    public EnumerableTypes.DamageTypes[] vulnerabilities;
    public EnumerableTypes.DamageTypes[] immunities;

    //Private Variables
    private Rigidbody2D _rb;
    

    private void Start()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("There is no RigidBody2D connected to this game object!");
        }
    }

    //Changes the health according to the number given
    public void ChangeHealth(int change)
    {
        //A negative number causes damage
        //A positive number heals
        health = health + change;
    }

    //Hurts the creature
    public void HitCreature(int amount, EnumerableTypes.DamageTypes type)
    {
        Color indication = Color.red;
        //Modify amount based on vulnerabilities, resistances, and immunities
        foreach (var t in vulnerabilities)
        {
            if (t == type)
            {
                amount = amount * 2;
                indication = Color.black;
            }
        }
        foreach (var t in resistances)
        {
            if (t == type)
            {
                amount = amount / 2;
                indication = Color.magenta;
            }
        }
        foreach (var t in immunities)
        {
            if (t == type)
            {
                amount = amount * 0;
                indication = Color.white;
            }
        }
        
        StartCoroutine(DamageIndication(indication));
        
        //Damage the creature
        ChangeHealth(-amount);
        
        //Check to see if the creature dies
        if (health < 1)
        {
            //The creature dies
            Destroy(gameObject);
        }
    }

    IEnumerator DamageIndication(Color indicationColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = indicationColor;

        yield return new WaitForSeconds(0.1f);
        
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
