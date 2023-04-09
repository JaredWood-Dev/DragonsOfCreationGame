using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //Handles the weapons selection and use
    public Weapon primary;
    private IAttack CreaturePrimaryAttack;
    public Weapon secondary;
    private IAttack CreatureSecondaryAttack;
    
    //Enumerable Types
    //Weapon has a type for every type of weapon
    public enum Weapon
    {
        GreatAxe,
        IceScimitars
    }

    private void Start()
    {
        //Set up the selected weapons
        HandleWeapons();
    }

    private void HandleWeapons()
    {
        //The creature's primary weapon, this switch statement handles adding the appropriate component
        switch (primary)
        {
            case Weapon.GreatAxe:
                CreaturePrimaryAttack = gameObject.AddComponent<GreatAxe>();
                break;
            case Weapon.IceScimitars:
                CreaturePrimaryAttack = gameObject.AddComponent<IceScimitars>();
                break;
        }
        
        //The creature's secondary weapon, this switch statement handles addiing the appropritate component
        switch (secondary)
        {
            case Weapon.GreatAxe:
                CreatureSecondaryAttack = gameObject.AddComponent<GreatAxe>();
                break;
            case Weapon.IceScimitars:
                CreatureSecondaryAttack = gameObject.AddComponent<IceScimitars>();
                break;
        }
    }
    
    //The primary attack used by players and monsters
    public void PrimaryAttack()
    {
        CreaturePrimaryAttack.MakeAttack();
    }

    //The secondary attack primarily used by players, only a few monsters utilize the secondary attack
    public void SecondaryAttack()
    {
        CreatureSecondaryAttack.MakeAttack();
    }
}
