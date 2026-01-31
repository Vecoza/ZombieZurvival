// using System;
// using UnityEngine;
// using TMPro;
// using UnityEditor.Search;
// using Random = UnityEngine.Random;
//
// public class WeaponScript : MonoBehaviour
// {
//     //zombie
//     public int weaponDamage;
//     
//     //muzzel effect
//     public GameObject muzzleEffect;
//     //animacija
//     private Animator animator;
//     
//     //bullet 
//     public GameObject bullet;
//
//     //bullet force
//     public float shootForce, upwardForce;
//
//     //Gun stats
//     public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
//     public int magazineSize, bulletsPerTap;
//     public bool allowButtonHold;
//
//     public int bulletsLeft, bulletsShot;
//
//     //Recoil
//     public Rigidbody playerRb;
//     public float recoilForce;
//
//     //bools
//     bool shooting, readyToShoot, reloading;
//
//     //Reference
//     public Camera fpsCam;
//     public Transform attackPoint;
//     
//     
//     public bool allowInvoke = true;
//     private EquipWeapon _equipWeaponScript;
//     private bool isEquiped;
//     
//     public enum WeaponModel
//     {
//         HandgunM1911,
//         ThompsonM1A1,
//         RifleAK74M
//     }
//
//     public WeaponModel thisWeaponModel;
//     private void Awake()
//     {
//         //make sure magazine is full
//         bulletsLeft = magazineSize;
//         readyToShoot = true;
//         animator = GetComponent<Animator>();
//     }
//     public void SetEquiped(bool status)
//     {
//         isEquiped = status;
//     }
//     public bool GetEquiped()
//     {
//         return isEquiped;
//     }
//     private void Start()
//     {
//         SetEquiped(false);
//     }
//     
//     private void Update()
//     {
//         MyInput();
//     }
//
//     private void MyInput()
//     {
//         if (!isEquiped)
//         {
//             SetWeaponLayer("Default");
//             return;
//         }
//
//         SetWeaponLayer("WeaponRender");
//
//         // Handle shooting input
//         shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
//
//         // Handle reloading
//         if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > 0)
//         {
//             Reload();
//             return; // No need to continue if reloading
//         }
//
//         // Handle shooting and empty magazine sound
//         if (readyToShoot && shooting && !reloading)
//         {
//             if (bulletsLeft <= 0)
//             {
//                 SoundManager.Instance.emptyMagizineSound.Play();
//                 if (WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > 0)
//                 {
//                     Reload();
//                 }
//             }
//             else
//             {
//                 bulletsShot = 0;
//                 Shoot();
//             }
//         }
//     }
//
//     private void SetWeaponLayer(string layerName)
//     {
//         foreach (Transform child in transform)
//         {
//             child.gameObject.layer = LayerMask.NameToLayer(layerName);
//         }
//     }
//
//
//     private void Shoot()
//     {
//         muzzleEffect.GetComponent<ParticleSystem>().Play();
//         //animator.SetTrigger("RECOIL"); // uvijek koristimo ovo za amimaciju
//         //SoundManager.Instance.shootingSoundM1911.Play();
//         SoundManager.Instance.PlayShootingSound(thisWeaponModel);
//         
//         readyToShoot = false;
//
//         //Find the exact hit position using a raycast
//         Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
//         RaycastHit hit;
//
//         //check if ray hits something
//         Vector3 targetPoint;
//         if (Physics.Raycast(ray, out hit))
//             targetPoint = hit.point;
//         else
//             targetPoint = ray.GetPoint(75); //Just a point far away from the player
//         // Better for spread
//         // Vector3 targetPoint = ray.GetPoint(5f);
//         
//         Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
//         
//         float x = Random.Range(-spread, spread);
//         float y = Random.Range(-spread, spread);
//         // float z = Random.Range(-spread, spread);
//         Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
//         
//         GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); 
//         
//         //ZOMBIE
//         //Bullet bul = currentBullet.GetComponent<Bullet>();
//         //bul.bulletDamage = weaponDamage;
//         //
//         
//         currentBullet.transform.forward = directionWithSpread.normalized;
//         
//         currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
//
//         if (muzzleEffect != null)
//         {
//             var effect = Instantiate(muzzleEffect, attackPoint.position, Quaternion.identity);
//             Destroy(effect, 1f);
//         }
//
//         bulletsLeft--;
//         bulletsShot++;
//         
//         if (allowInvoke)
//         {
//             Invoke("ResetShot", timeBetweenShooting);
//             allowInvoke = false;
//             
//             playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
//         }
//         
//         if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
//             Invoke("Shoot", timeBetweenShots);
//         Destroy(currentBullet, 3f);
//         
//         
//     }
//     private void ResetShot()
//     {
//         readyToShoot = true;
//         allowInvoke = true;
//     }
//
//     private void Reload()
//     {
//         //SoundManager.Instance.reloadingSound.Play();
//         
//         
//         
//         reloading = true;
//         Invoke("ReloadFinished", reloadTime); 
//         
//         SoundManager.Instance.PlayReloadSound(thisWeaponModel);
//         
//     }
//     private void ReloadFinished()
//     {
//         // SoundManager.Instance.PlayReloadSound(thisWeaponModel); //test
//         
//         int reloadAmount = magazineSize - bulletsLeft; // Maximum bullets we can reload
//         int availableAmmo = WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel); // Ammo available in inventory
//
//         // Calculate the actual bullets to reload, which is the minimum of what we need and what we have
//         int bulletsToReload = Math.Min(reloadAmount, availableAmmo);
//
//         // Add the bullets to the magazine
//         bulletsLeft += bulletsToReload;
//
//         // Decrease the total ammo in the inventory by the amount reloaded
//         WeaponManager.Instance.DecreaseTotalAmmo(bulletsToReload, thisWeaponModel);
//         
//         reloading = false;
//     }
// }
using System;
using UnityEngine;
using TMPro;
using EZCameraShake;

public class WeaponScript : MonoBehaviour
{
    // Public Fields
    public int weaponDamage;
    public GameObject muzzleEffectPrefab;  // Use a prefab for the muzzle effect
    public GameObject bullet;
    public float shootForce, upwardForce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public Rigidbody playerRb;
    public float recoilForce;
    public Camera fpsCam;
    public Transform attackPoint;
    public WeaponModel thisWeaponModel;
    public Animator animator;

    // Private Fields
    private bool shooting, readyToShoot, reloading;
    private bool isEquipped;

    private static Vector3 originalPos;
    // private EquipWeapon equipWeaponScript;
    private GameObject muzzleEffectInstance;  // Store instance of the muzzle effect
    private bool allowInvoke = true;

    public enum WeaponModel
    {
        HandgunM1911,
        ThompsonM1A1,
        RifleAK74M
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        animator = GetComponent<Animator>();

        // Instantiate the muzzle effect once and deactivate it
        if (muzzleEffectPrefab != null)
        {
            muzzleEffectInstance = Instantiate(muzzleEffectPrefab, attackPoint);
            muzzleEffectInstance.SetActive(false);
        }
    }

    private void Start()
    {
        SetEquipped(false);
    }

    private void Update()
    {
        if (!PauseMenu.activePause)
        {
            if (PlayerScript.isAlive)
            {
                HandleInput();
            }
        }
    }

    public void SetEquipped(bool status)
    {
        isEquipped = status;
    }

    public bool GetEquipped()
    {
        return isEquipped;
    }

    private void HandleInput()
    {
        if (isEquipped)
        {
            originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            setChildrenLayer(this.gameObject, "WeaponRender");
            animator.enabled = true;
        }
        else
        {
            setChildrenLayer(this.gameObject, "Default");
            animator.enabled = false;
            return;
        }
        shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
        
        animator.SetBool("Shooting", false);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > 0)
        {
            Reload();
            return;
        }

        if (readyToShoot && shooting && !reloading)
        {
            if (bulletsLeft <= 0)
            {
                SoundManager.Instance.emptyMagizineSound.Play();
                if (WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > 0)
                {
                    Reload();
                }
            }
            else
            {
                bulletsShot = 0;
                Shoot();
                transform.position = originalPos;
            }
        }
    }

    public void setChildrenLayer(GameObject go, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        go.layer = layer;

        foreach (Transform child in go.transform)
        {
            if (child.gameObject.layer != layer)
            {
                child.gameObject.layer = layer;

                if (child.transform.childCount != 0)
                {   
                    setChildrenLayer(child.gameObject, layerName);
                }
            }
        }
    }

    private void Shoot()
    {
        CameraShaker.Instance.ShakeOnce(1.8f, 1f, .1f, .5f);
        animator.SetBool("Shooting", true);
        PlayMuzzleEffect();
        PlayShootingSound();

        readyToShoot = false;

        Vector3 directionWithSpread = CalculateSpreadDirection();
        GameObject currentBullet = InstantiateBullet(directionWithSpread);
        
        ApplyRecoil(directionWithSpread);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
        
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        Destroy(currentBullet, 0.1f);
    }

    private void PlayMuzzleEffect()
    {
        if (muzzleEffectInstance != null)
        {
            muzzleEffectInstance.SetActive(true);
            muzzleEffectInstance.GetComponent<ParticleSystem>().Play();
            Invoke("HideMuzzleEffect", 0.1f);  // Hide after a short delay
        }
    }

    private void HideMuzzleEffect()
    {
        if (muzzleEffectInstance != null)
        {
            muzzleEffectInstance.SetActive(false);
        }
    }

    private void PlayShootingSound()
    {
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
    }

    private Vector3 CalculateSpreadDirection()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit) ? hit.point : ray.GetPoint(75);
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);
        return directionWithoutSpread + new Vector3(x, y, 0);
    }

    private GameObject InstantiateBullet(Vector3 direction)
    {
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        return currentBullet;
    }

    private void ApplyRecoil(Vector3 direction)
    {
        playerRb.AddForce(-direction.normalized * recoilForce, ForceMode.Impulse);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        int reloadAmount = magazineSize - bulletsLeft;
        int availableAmmo = WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel);
        int bulletsToReload = Math.Min(reloadAmount, availableAmmo);

        bulletsLeft += bulletsToReload;
        WeaponManager.Instance.DecreaseTotalAmmo(bulletsToReload, thisWeaponModel);

        reloading = false;
    }
}
