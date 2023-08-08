using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	Collider2D attackCollider;
	public int attackDamage;
	public Vector2 knockback = Vector2.zero;

	private void Awake()	
	{
		attackCollider = GetComponent<Collider2D>();
	}	
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// see if it can be hit
		Damageble damageable = collision.GetComponent<Damageble>();
		
		if(damageable is not null)
		{
			// If parent is facing hte left by localscale, our knockback x flips its value to face the left as well
			Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
			// hit the target
			bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
			if (gotHit)
			{				
				Debug.Log(collision.name + " hit for " + attackDamage);
			}
		}
	}
}
