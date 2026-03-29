using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen_UI : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("FadeIn");
    }
}
