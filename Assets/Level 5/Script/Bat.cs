using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
namespace level5
{
    public class Bat : MonoBehaviour
    {
        public float flightSpeed = 3f;
        public float waypointReachedDistance = 0.1f;
        public DetectionZone biteDetectionZone;
        public List<Transform> waypoints;

        Animator animator;
        Rigidbody2D rb;
        Damageable damageable;

        Transform nextWaypoint;
        int waypointNum = 0;

        public bool _hasTarget = false;
        public bool HasTarget
        {
            get { return _hasTarget; }
            private set
            {
                _hasTarget = value;
                animator.SetBool(AnimationStrings.hasTarget, value);
            }
        }

        public bool CanMove
        {
            get
            {
                return animator.GetBool(AnimationStrings.canMove);
            }
        }

        public float AttackCooldown
        {
            get
            {
                return animator.GetFloat(AnimationStrings.attackCooldown);
            }
            private set
            {
                animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            damageable = GetComponent<Damageable>();
        }

        private void Start()
        {
            nextWaypoint = waypoints[waypointNum];
        }

        void Update()
        {
            HasTarget = biteDetectionZone.detectedColliders.Count > 0;
        }

        private void FixedUpdate()
        {
            if (damageable.IsAlive)
            {
                if (CanMove)
                {
                    Flight();
                }
                else
                {
                    rb.linearVelocity = Vector3.zero;
                }
            }
            else
            {
                rb.gravityScale = 2f;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }

        private void Flight()
        {
            Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

            float distance = Vector2.Distance(nextWaypoint.position, transform.position);

            rb.linearVelocity = directionToWaypoint * flightSpeed;
            UpdateDiraction();
            if (distance <= waypointReachedDistance)
            {
                waypointNum++;
                if (waypointNum >= waypoints.Count)
                {
                    waypointNum = 0;
                }
                nextWaypoint = waypoints[waypointNum];
            }
        }

        private void UpdateDiraction()
        {
            Vector3 localScale = transform.localScale;
            if (transform.localScale.x > 0)
            {
                if (rb.linearVelocity.x < 0)
                {
                    transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
                }
            }
            else
            {
                if (rb.linearVelocity.x > 0)
                {
                    transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
                }
            }
        }
    }
}