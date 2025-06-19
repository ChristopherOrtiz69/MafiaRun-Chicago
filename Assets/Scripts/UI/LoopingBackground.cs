using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    private float spriteWidth;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float distanceToCam = cam.position.x - transform.position.x;

        if (Mathf.Abs(distanceToCam) > spriteWidth)
        {
            float direction = Mathf.Sign(distanceToCam);
            transform.position += Vector3.right * spriteWidth * 2f * direction;
        }
    }
}
