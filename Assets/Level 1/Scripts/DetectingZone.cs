using System.Collections.Generic;
using UnityEngine;
namespace level1
{
    public class DetectingZone : MonoBehaviour
    {
        public List<Collider2D> detectedColliders = new List<Collider2D>();
        Collider2D col;
        private void Awake()
        {
            col = GetComponent<Collider2D>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnTriggerEnter2D(Collider2D collision)
        {
            detectedColliders.Add(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            detectedColliders.Remove(collision);
        }

    }
}