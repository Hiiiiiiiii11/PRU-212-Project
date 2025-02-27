using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffect;
    private float previousCamY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        previousCamY = cam.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffect));
        float distX = (cam.transform.position.x * parallaxEffect);
        float verticalMovement = cam.transform.position.y - previousCamY;
        transform.position = new Vector3(startPosX + distX, startPosY + verticalMovement, transform.position.z);
        if (tempX > startPosX + length) startPosX += length;
        else if (tempX < startPosX - length) startPosX -= length;
    }
}
