using System;
using System.Collections.Generic;
using UnityEngine;

namespace level4
{
    public class FlightEyeController : MonoBehaviour
	{
		public float flightSpeed = 3f;
		public List<Transform> waypoints;
		Transform nextWaypoint;
		int waypointNum = 0;	

		public DetectionZone biteDetection;
		private Animator animator;
		Rigidbody2D rb;
		Damageable damageable;

		private bool _hasTarget = false;
		public float waypointReachedDistance = 0.1f;

		public bool HasTarget
		{
			get => _hasTarget;
			private set
			{
				_hasTarget = value;
				animator.SetBool(AnimationStrings.hasTarget, value);
			}
		}

		public bool CanMove
		{
			get => animator.GetBool(AnimationStrings.canMove);
		}

		public float AttackCooldown
		{
			get => animator.GetFloat(AnimationStrings.attackCooldown);
			private set
			{
				animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
			}
		}

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();
			damageable = GetComponent<Damageable>();
		}

		void Start()
		{
			nextWaypoint = waypoints[waypointNum];
		}

		void Update()
		{
			HasTarget = biteDetection.detectionCollitions.Count > 0;

			if (AttackCooldown > 0)
				AttackCooldown -= Time.deltaTime;
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
					rb.linearVelocity = Vector2.zero;
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
			if (nextWaypoint == null) return;

			Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
			float distance = Vector2.Distance(nextWaypoint.position, transform.position);

			rb.linearVelocity = directionToWaypoint * flightSpeed;
			UpdateDirection();

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

		private void UpdateDirection()
		{
			if (transform.localScale.x > 0)
			{
				if (rb.linearVelocity.x < 0) transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
			else 
			{
				if (rb.linearVelocity.x > 0) transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}
}