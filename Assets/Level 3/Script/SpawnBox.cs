using UnityEngine;
namespace level3
{
    public class SpawnBox : MonoBehaviour
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] public GameObject keyPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform keySpawnPoint;
        private bool isTriggered = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"Collided with: {collision.gameObject.name}");

            if (!isTriggered && collision.gameObject.CompareTag("Player"))
            {
                isTriggered = true;
                Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                Debug.Log("Prefab spawned at " + spawnPoint.position);
            }
        }

        public void SpawnKey()
        {
            Instantiate(keyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}