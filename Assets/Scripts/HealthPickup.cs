using UnityEngine;

public class HealthPickup : MonoBehaviour
{

	public int healthRestore = 20;
	public Vector3 spinRotationSpeed = new(0, 180, 0);

	AudioSource pickupSound;

	private void Awake()
	{
		pickupSound = GetComponent<AudioSource>();
	}
	void Start()
	{
	}

	void Update()
	{
		transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Damageable damageable = collision.GetComponent<Damageable>();

		if (damageable)
		{
			var wasHeal = damageable.Heal(healthRestore);

			if (wasHeal) 
			{
				if (pickupSound)
					AudioSource.PlayClipAtPoint(pickupSound.clip, gameObject.transform.position, pickupSound.volume);

				Destroy(gameObject); 
			}
		}
	}
}
