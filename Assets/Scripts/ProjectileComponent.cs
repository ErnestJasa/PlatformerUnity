using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    public int damage = 8;
    public Vector2 moveSpeed = new(3f, 0);
    public Vector2 knockback = new(0, 0);

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageble damageable = collision.GetComponent<Damageble>();

        if (damageable != null)
        {
            // If parent is facing hte left by localscale, our knockback x flips its value to face the left as well
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // hit the target
            bool gotHit = damageable.Hit(damage, deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + damage);
                Destroy(gameObject);
            }
        }
    }
}
