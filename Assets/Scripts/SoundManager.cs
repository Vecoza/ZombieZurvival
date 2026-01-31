using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingSoundM1911;

    public AudioSource reloadingSound;
    public AudioSource emptyMagizineSound;
    
    //kalas
    public AudioSource reloadingSoundAK74;
    public AudioSource shootingSoundAK47;
    //uzii
    public AudioSource reloadingSoundM1A1;
    public AudioSource shootingSoundM1A1;

    public AudioSource throwablesChannel;
    public AudioClip grenade;
    public AudioClip grenadeImpact;
    public AudioClip grenadeThrow;
    
    //ammo
    public AudioSource AmmoPickUpChannel;
    public AudioClip ammoBoxPickUp;
    
    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    public AudioClip gameOverMusic;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponScript.WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponScript.WeaponModel.HandgunM1911:
                shootingSoundM1911.Play();
                break;
            case WeaponScript.WeaponModel.ThompsonM1A1:
                shootingSoundM1A1.Play();
                break;
            case WeaponScript.WeaponModel.RifleAK74M:
                shootingSoundAK47.Play();
                break;
        }
        
        
    }

    public void PlayReloadSound(WeaponScript.WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponScript.WeaponModel.HandgunM1911:
                reloadingSound.Play();
                break;
            case WeaponScript.WeaponModel.ThompsonM1A1:
                reloadingSoundM1A1.Play();
                break;
            case WeaponScript.WeaponModel.RifleAK74M:
                reloadingSoundAK74.Play();
                break;
        }
    }
    
    
}
