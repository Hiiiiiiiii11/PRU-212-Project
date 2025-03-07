using UnityEngine;
namespace level2
{
    public class Projectile : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public float lifeTime = 5f;
        private float timeSinceSpawn = 0f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position += moveSpeed * transform.right * Time.deltaTime;

            timeSinceSpawn += Time.deltaTime;
            if (timeSinceSpawn > lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}