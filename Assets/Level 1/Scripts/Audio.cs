using UnityEngine;

namespace level1
{
    public class Audio : MonoBehaviour
    {
        [SerializeField] private AudioSource BackgroundAudioSource;
        [SerializeField] private AudioSource effectAudioSource;
        [SerializeField] private AudioClip BackgroundClip;
        [SerializeField] private AudioClip JumpClip;
        [SerializeField] private AudioClip AttackSword1;
        [SerializeField] private AudioClip AttackSword2;
        [SerializeField] private AudioClip AttackSword3;
        [SerializeField] private AudioClip Trap;
        [SerializeField] private AudioClip PlayerHurt;
        [SerializeField] private AudioClip MonsterBite;
        [SerializeField] private AudioClip MonsterDead;
        [SerializeField] private AudioClip MonsterPunch;
        [SerializeField] private AudioClip DashAudio;
        [SerializeField] private AudioClip TouchEnemy;

        [SerializeField] private AudioClip DeadSound;
        [SerializeField] private AudioClip Healing;
        [SerializeField] private AudioClip PickupClock;
        [SerializeField] private AudioClip EnterPortal;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PlayBackgroundMusic();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void PlayBackgroundMusic()
        {
            BackgroundAudioSource.clip = BackgroundClip;
            BackgroundAudioSource.Play();
        }
        public void PlayJumpSound()
        {
            effectAudioSource.PlayOneShot(JumpClip);
        }
        public void PlayAttackSword1()
        {
            effectAudioSource.PlayOneShot(AttackSword1);
        }
        public void PlayAttackSword2()
        {
            effectAudioSource.PlayOneShot(AttackSword2);
        }
        public void PlayAttackSword3()
        {
            effectAudioSource.PlayOneShot(AttackSword3);
        }
        public void PlayTrapClose()
        {
            effectAudioSource.PlayOneShot(Trap);
        }
        public void PlayPlayerHurt()
        {
            effectAudioSource.PlayOneShot(PlayerHurt);
        }
        public void PlayMonsterBite()
        {
            effectAudioSource.PlayOneShot(MonsterBite);
        }
        public void PlayMonsterDead()
        {
            effectAudioSource.PlayOneShot(MonsterDead);
        }
        public void PlayMonsterPunch()
        {
            effectAudioSource.PlayOneShot(MonsterPunch);
        }
        public void PlayDashAudio()
        {
            effectAudioSource.PlayOneShot(DashAudio);
        }
        public void PlayDeadSound()
        {
            effectAudioSource.PlayOneShot(DeadSound);
        }
        public void AttackOnEnemy()
        {
            effectAudioSource.PlayOneShot(TouchEnemy);
        }
        public void HealingItem()
        {
            effectAudioSource.PlayOneShot(Healing);
        }
        public void PickUpItem()
        {
            effectAudioSource.PlayOneShot(PickupClock);
        }
        public void PlayEnterPortal()
        {
            effectAudioSource.PlayOneShot(EnterPortal);
        }
    }
}