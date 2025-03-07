using UnityEngine;

namespace level3
{
    public class Parallax : MonoBehaviour
    {
        Transform cam; // Main Camera
        Vector3 camStartPos;
        float distance; // Distance between the start camera position and current position

        GameObject[] backgrounds;
        Material[] mat;
        float[] backSpeed;

        float farthestBack;

        [Range(0.01f, 0.5f)] // Reduce max range for better control
        public float parallaxSpeed = 0.1f; // Lower default value
        public float yOffset = -1.5f; // Adjust this value to move background lower

        void Start()
        {
            cam = Camera.main.transform;
            camStartPos = cam.position;

            int backCount = transform.childCount;
            mat = new Material[backCount];
            backSpeed = new float[backCount];
            backgrounds = new GameObject[backCount];

            for (int i = 0; i < backCount; i++)
            {
                backgrounds[i] = transform.GetChild(i).gameObject;
                mat[i] = backgrounds[i].GetComponent<Renderer>().material;
            }

            BackSpeedCalculate(backCount);
        }

        void BackSpeedCalculate(int backCount)
        {
            farthestBack = float.MinValue;
            float nearestBack = float.MaxValue;

            for (int i = 0; i < backCount; i++) // Find the farthest and nearest background
            {
                float depth = backgrounds[i].transform.position.z - cam.position.z;
                if (depth > farthestBack)
                {
                    farthestBack = depth;
                }
                if (depth < nearestBack)
                {
                    nearestBack = depth;
                }
            }

            for (int i = 0; i < backCount; i++) // Set the speed of background
            {
                float depth = backgrounds[i].transform.position.z - cam.position.z;
                backSpeed[i] = (farthestBack == nearestBack) ? 0.01f : (farthestBack - depth) / (farthestBack - nearestBack) * 0.2f;
            }
        }

        private void FixedUpdate()
        {
            distance = cam.position.x - camStartPos.x;
            transform.position = new Vector3(cam.position.x, cam.position.y + yOffset, transform.position.z);

            for (int i = 0; i < backgrounds.Length; i++)
            {
                float speed = Mathf.Max(backSpeed[i] * parallaxSpeed, 0.005f); // Lower the minimum movement speed
                mat[i].SetTextureOffset("_MainTex", new Vector2(distance * speed, 0));
            }
        }
    }
}