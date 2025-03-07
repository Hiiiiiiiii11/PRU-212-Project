using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace level4
{
	public class HealthBar : MonoBehaviour
	{
		public Slider healthSlider;
		public TMP_Text healthBarTex;
		Damageable playerDamageable;

		private void Awake()
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			playerDamageable = player.GetComponent<Damageable>();
		}
		void Start()
		{
			healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
			healthBarTex.text = "HP " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
		}

		private void OnEnable()
		{
			playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
		}

		private void OnDisable()
		{
			playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
		}

		private float CalculateSliderPercentage(float currentHealth, float maxHealth)
		{
			return currentHealth / maxHealth;
		}

		void Update()
		{

		}

		private void OnPlayerHealthChanged(int mewHealth, int maxHealth)
		{
			healthSlider.value = CalculateSliderPercentage(mewHealth, maxHealth);
			healthBarTex.text = "HP " + mewHealth + " / " + maxHealth;
		}
	}
}