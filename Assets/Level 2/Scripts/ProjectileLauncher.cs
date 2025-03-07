using UnityEngine;
namespace level2
{
    public class ProjectileLauncher : MonoBehaviour
    {
        public GameObject projectile;
        public Transform spawnLocation;
        public Quaternion spawnRotation;

        public float spawnTime = 0.5f;

        private float timeSinceSpawn = 0f;

        void Start()
        {

        }
        void Update()
        {
            timeSinceSpawn += Time.deltaTime;
            if (timeSinceSpawn >= spawnTime)
            {
                Instantiate(projectile, spawnLocation.position, spawnRotation);
                timeSinceSpawn = 0;
            }


        }
    }
}