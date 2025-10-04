using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float imageWidthOffset = 10;

    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalulateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }


    public void Move(float distanceToMove)
    {
        background.position = background.position + new Vector3(distanceToMove * parallaxMultiplier, 0);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageLeftEdge = (background.position.x - imageHalfWidth) - imageWidthOffset;
        float imageRightEdge = (background.position.x + imageHalfWidth) + imageWidthOffset;

        if (cameraRightEdge < imageLeftEdge)
            background.position += Vector3.right * imageFullWidth;
        else if (cameraRightEdge > imageRightEdge)
            background.position += Vector3.right * -imageFullWidth;




    }
}
