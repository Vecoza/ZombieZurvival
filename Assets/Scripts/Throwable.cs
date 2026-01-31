using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 25f;
    [SerializeField] private float force = 700f;
    [SerializeField] private int lethalDamage = 18;
    [SerializeField] private GameObject explosionEffect;
    private float countdown;
    private bool hasExploaded = false;
    private bool hasBeenThrown = false; 
    public enum ThrowableType
    {
        None,
        Grenade
    }

    public ThrowableType throwableType;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    public void setHasBeenThrown(bool value)
    {
        hasBeenThrown = value;
    }

    public bool getHasBeenThrown()
    {
        return hasBeenThrown;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploaded)
            {
                Expload();
                hasExploaded = true;
            }
        }
    }

    private void Expload()
    {
        GetThrowableEffect();
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hasBeenThrown)
        {
            SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeImpact);
            
        }
    }

    private void GrenadeEffect()
    {
        var effect = Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenade);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearByObj in colliders)
        {
            Rigidbody rb = nearByObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, damageRadius);
            }
        }
        Collider[] collidersArr = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearByObj in collidersArr)
        {
            var temp = nearByObj.GetComponent<Enemy>();
            if (temp != null)
            {
                nearByObj.GetComponent<Enemy>().TakeDamage(lethalDamage);
            }
        }

        CameraShaker.Instance.ShakeOnce(5f, 2.5f, .5f, 1f);
        Destroy(gameObject);
        Destroy(effect, 3f);
    }
}
