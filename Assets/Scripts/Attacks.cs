using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    //Modifies the damage amount according to any modifiers
    public float ModifyAmount();
    //Makes the attack, varies from weapon to weapon
    public void MakeAttack();
    //Returns the damage type of the attack
    public EnumerableTypes.DamageTypes GetDamageType();
}

public class GreatAxe : MonoBehaviour, IAttack
{
    private EnumerableTypes.DamageTypes damageType;
    private float _coolDownTimer;
    private int _weaponDamage;
    GreatAxe()
    {
        damageType = EnumerableTypes.DamageTypes.Slashing;
        _coolDownTimer = 10f;
        _weaponDamage = 12;
    }
    
    public float ModifyAmount()
    {
        return -1;
    }

    public void MakeAttack()
    {
        //The Great Axe cleaves through many enemies within its hit area
        Vector2 pos = gameObject.transform.position;
        Vector2 attackOrigin = new Vector2(pos.x - gameObject.transform.localScale.x / 2, pos.y);
        RaycastHit2D[] attackArea = Physics2D.BoxCastAll(attackOrigin, new Vector2(1.5f, gameObject.transform.localScale.y), 0f, transform.forward);

        //Iterate through all of the enemies in the target area and damage the appropriate enemies
        for (int i = 0; i < attackArea.Length; i++)
        {
            if (attackArea[i].collider.gameObject.CompareTag("Enemy"))
            {
                attackArea[i].collider.gameObject.GetComponent<Creature>().HitCreature(_weaponDamage, damageType);
            }
        }
    }

    public EnumerableTypes.DamageTypes GetDamageType()
    {
        return damageType;
    }
}

public class IceScimitars : MonoBehaviour, IAttack
{
    private EnumerableTypes.DamageTypes damageType;
    IceScimitars()
    {
        damageType = EnumerableTypes.DamageTypes.Slashing;
    }
    
    public float ModifyAmount()
    {
        return -1;
    }

    public void MakeAttack()
    {
        Debug.Log("Ice Scimitar!");
        Debug.Log("Ice Scimitar!");
    }

    public EnumerableTypes.DamageTypes GetDamageType()
    {
        return damageType;
    }
}
