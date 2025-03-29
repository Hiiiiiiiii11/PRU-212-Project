using UnityEngine;
using UnityEngine.Events;
namespace level5
{
    public class Damageable : MonoBehaviour
    {
        public UnityEvent<int, Vector2> damagableHit;
        public UnityEvent<int, int> healthChanged;

        Animator animator;
        [SerializeField]
        private int _maxHealth = 100;
        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;

            }
        }

        [SerializeField]
        private int _health = 100;

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                //healthChanged?.Invoke(_health, MaxHealth);
                if (_health <= 0)
                {
                    IsAlive = false;
                }
            }
        }

        [SerializeField]
        private bool _isAlive = true;

        [SerializeField]
        private bool isInvincible = false;

        private float timeSinceHit = 0;
        public float invincibilityTime = 0.25f;

        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
                animator.SetBool(AnimationStrings.isAlive, value);
            }
        }

        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationStrings.lockVelocity);
            }
            set
            {
                animator.SetBool(AnimationStrings.lockVelocity, value);
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isInvincible)
            {
                if (timeSinceHit > invincibilityTime)
                {
                    isInvincible = false;
                    timeSinceHit = 0;
                }
                timeSinceHit += Time.deltaTime;
            }
        }

        public bool Hit(int damage, Vector2 knockback)
        {
            if (IsAlive && !isInvincible)
            {
                Health -= damage;
                isInvincible = true;
                animator.SetTrigger(AnimationStrings.hitTrigger);
                LockVelocity = true;
                damagableHit?.Invoke(damage, knockback);
                healthChanged?.Invoke(Health, MaxHealth);
                CharacterEvents.characterDamaged.Invoke(gameObject, damage);
                return true;
            }
            return false;
        }

        public bool Heal(int healthRestored)
        {
            if (IsAlive && Health < MaxHealth)
            {
                int maxHeal = Mathf.Max(MaxHealth - Health, 0);
                int actualHeal = Mathf.Min(maxHeal, healthRestored);
                Health += actualHeal;
                healthChanged?.Invoke(Health, MaxHealth);
                CharacterEvents.characterHealed(gameObject, actualHeal);
                return true;
            }
            return false;
        }
    }
}
