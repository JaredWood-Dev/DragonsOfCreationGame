using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumerableTypes : MonoBehaviour
{
    //Damage Types
    //Used for resistances and vulnerabilities
    public enum DamageTypes
    {
        Slashing,
        Bludgeoning,
        Piercing,
        Lightning,
        Fire,
        Cold,
        Acid,
        Poison,
        Thunder,
        Psychic,
        Force,
        Radiant,
        Necrotic
    }
    
    //Directions
    //Used for finding what directions things are facing
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
