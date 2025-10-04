using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;

    public void Move(float distanceToMove)
    {
        background.position = background.position + new Vector3(distanceToMove * parallaxMultiplier, 0);
    }
}
