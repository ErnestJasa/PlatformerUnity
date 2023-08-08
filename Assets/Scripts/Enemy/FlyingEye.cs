using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2.5f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    Animator animator;
    Rigidbody2D rb;
    Damageble damageble;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageble = GetComponent<Damageble>();
    }


    private void OnEnable()
    {
        damageble.damageableDeath.AddListener(OnDeath);
    }

    // Start is called before the first frame update
    void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }




    private void FixedUpdate()
    {
        if (damageble.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        // Fly to the next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;

        UpdateDiraction();

        // See if we need to switch waypoints
        if (distance <= waypointReachedDistance)
        {
            // switch to next waypoint
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                // Loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }

    }

    private void UpdateDiraction()
    {
        Vector3 localScale = transform.localScale;

        if (localScale.x > 0)
        {
            // facing right
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }
        }
        else
        {
            // facing left
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.x);
            }
        }
    }

    public void OnDeath()
    {
        // dead, falls to the ground
        rb.gravityScale = 2f;
        rb.velocity = new Vector3(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
