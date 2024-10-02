using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cam;
    private float xPosition;
    private float length;
    [SerializeField] public float parallaxEffect;
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }

    void Update()
    {
        float distanceMoved = (1-parallaxEffect) * cam.position.x;
        float distanceToMove =  parallaxEffect * cam.position.x;
        transform.position = new Vector2(xPosition + distanceToMove, transform.position.y);

        if (xPosition + length < distanceMoved)
            xPosition = distanceMoved + length;
        else if (xPosition - length > distanceMoved)
            xPosition = distanceMoved - length;
    }
}
