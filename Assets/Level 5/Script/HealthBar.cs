using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace level5
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthSlider;
        public TMP_Text healthBarText;

        Damageable playerDamageable;

        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerDamageable = player.GetComponent<Damageable>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            healthSlider.value = CalculatedSilderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
            healthBarText.text = "HP: " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
        }

        private void OnEnable()
        {
            playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
        }

        private void OnDisable()
        {
            playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
        }
        private float CalculatedSilderPercentage(float currentHealth, float maxHealth)
        {
            return currentHealth / maxHealth;
        }

        private void OnPlayerHealthChanged(int newHealth, int maxHealth)
        {
            healthSlider.value = CalculatedSilderPercentage(newHealth, maxHealth);
            healthBarText.text = "HP: " + newHealth + " / " + maxHealth;
        }
    }
}