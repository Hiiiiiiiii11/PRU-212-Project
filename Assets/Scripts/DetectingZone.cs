using System.Collections.Generic;
using UnityEngine;

public class DetectingZone : MonoBehaviour
{
    public List<Collider2D> detetedColliders = new List<Collider2D>();
    Collider2D col;
    void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OniggerEnter2D(Collider2D collision)
    {
        detetedColliders.Add(collision);
    }
    private void OiggerExit2D(Collider2D collision)
    {
        detetedColliders.Remove(collision);
    }
}
