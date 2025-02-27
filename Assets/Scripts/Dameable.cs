using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit = new();
    public UnityEvent<int, int> healthChanged = new();

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get => _maxHealth;
        private set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100; 
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;

            healthChanged?.Invoke(_health, MaxHealth);

            if (_health <= 0)
            {
                IsAlive = false;
			}
        }
    }

    private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    [SerializeField]
	private bool isInvincible = false;

	private float timeSinceHit = 0;
	public float invincibilityTime = 1f;

	Animator animator;

	public bool LockVelocity
	{
		get => animator.GetBool(AnimationStrings.lockVelocity);
		private set => animator.SetBool(AnimationStrings.lockVelocity, value);
	}

	private void Awake()
    {
        animator = GetComponent<Animator>();
    }

	void Update()
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

	public bool Hit (int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hit);

            LockVelocity = true;

            damageableHit.Invoke(damage, knockback);

            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        return false;
    }

    public bool Heal (int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int originalHealth = Health;
            Health = Mathf.Min(MaxHealth, Health + healthRestore);
            CharacterEvents.characterHealed.Invoke(gameObject, Health - originalHealth);
            return true;
        }
        else return false;
    }
}
