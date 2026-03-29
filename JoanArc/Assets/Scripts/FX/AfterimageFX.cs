using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float afterimageColorLosingSpeed;

    public void SetupAfterImage(Sprite _spriteImage, float _afterimageColorLosingSpeed)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _spriteImage;

        afterimageColorLosingSpeed = _afterimageColorLosingSpeed;
    }

    private void Update()
    {
        float alpha = sr.color.a - afterimageColorLosingSpeed * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
