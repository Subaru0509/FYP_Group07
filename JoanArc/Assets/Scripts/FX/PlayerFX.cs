using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    private bool canScreenShake = true;

    [Header("Screen Shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeDirection_light;
    public Vector3 shakeDirection_medium;
    public Vector3 shakeDirection_heavy;
    private CinemachineImpulseSource screenShake;

    [Header("Afterimage FX")]
    [SerializeField] private GameObject afterimagePrefab;
    [SerializeField] private float afterimageColorLosingSpeed;
    [SerializeField] private float afterimageCooldown;
    private float afterimageCooldownTimer;

    [Space]
    [SerializeField] private ParticleSystem dustFX;
    [SerializeField] private ParticleSystem downStrikeDustFX;

    protected override void Awake()
    {
        base.Awake();

        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    protected override void Start()
    {
        base.Start();

        afterimageCooldownTimer = 0;
    }

    protected override void Update()
    {
        base.Update();

        afterimageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakeDirection)
    {
        //to prevent gigantic screenshake when hitting multiple enemies at the same time
        if (canScreenShake)
        {
            screenShake.m_DefaultVelocity = new Vector3(_shakeDirection.x * player.facingDirection, _shakeDirection.y) * shakeMultiplier;
            screenShake.GenerateImpulse();
            canScreenShake = false;
            Invoke("EnableScreenShake", 0.05f);
        }
    }

    private void EnableScreenShake()
    {
        canScreenShake = true;
    }

    public void CreateAfterimage()
    {
        if (afterimageCooldownTimer < 0)
        {
            //need to pass transform.rotation here to make afterimage flip together with player when dashing to the left
            GameObject newAfterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            newAfterimage.GetComponent<AfterimageFX>()?.SetupAfterImage(sr.sprite, afterimageColorLosingSpeed);

            afterimageCooldownTimer = afterimageCooldown;
        }
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
        {
            dustFX.Play();
        }
    }

    public void PlayDownStrikeDustFX()
    {
        if (downStrikeDustFX != null)
        {
            downStrikeDustFX.Play();
        }
    }
}
