using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float length;

    public float offsety;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float BGPositionOffset = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, cam.transform.position.y + offsety);

        //make endless background image
        if (BGPositionOffset > xPosition + length)
        {
            //if BGPositionOffset > background_length
            //(xPosition + length = coordinate(зјБъ) of the right edge of the original BG),
            //move BG to the new position
            xPosition += length;
        }
        else if (BGPositionOffset < xPosition - length)
        {
            //same as above but move BG to the left position
            xPosition -= length;
        }
    }
}
