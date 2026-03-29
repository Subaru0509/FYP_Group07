using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float appearingSpeed;
    [SerializeField] private float disappearingSpeed;
    [SerializeField] private float colorLosingSpeed;

    [SerializeField] private float lifeTime;
    private float textTimer;

    private void Awake()
    {
        myText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        textTimer = lifeTime;
    }

    private void Update()
    {
        textTimer -= Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), appearingSpeed * Time.deltaTime);


        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorLosingSpeed * Time.deltaTime;

            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), disappearingSpeed * Time.deltaTime);
            }

            if (myText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
