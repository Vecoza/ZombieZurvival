using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    [SerializeField]
    private GameObject socket;

    [Header("Ammo")] 
    [SerializeField] private int totalRifle_Ammo = 0;
    [SerializeField] private int totalSMGAmmo = 0;
    [SerializeField] private int totalHGAmmo = 0;

    [Header("Throwables General")] 
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private GameObject throwableSpawn;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float forceMultiplierLimit = 3f;
    
    [Header("Lethals")]
    [SerializeField] private int lethalsCount = 0;
    [SerializeField] private Throwable.ThrowableType equippedLethal;
    [SerializeField] private GameObject throwablePrefab;

    [Header("Player")] 
    [SerializeField]
    private GameObject player;
    
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

    private void Start()
    {
        equippedLethal = Throwable.ThrowableType.None;
    }

    public GameObject getSocket()
    {
        return socket;
    }

    public Throwable.ThrowableType getEquippedLehtlType()
    {
        return equippedLethal;
    }
    public void EquipWeapon(GameObject equippedWeapon)
    {
        AddWeaponToSocket(equippedWeapon);
    }

    internal void PickUpAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifle_Ammo += ammo.ammoAmount;
                SoundManager.Instance.AmmoPickUpChannel.PlayOneShot(SoundManager.Instance.ammoBoxPickUp);
                break;
            case AmmoBox.AmmoType.SMGAmmo:
                totalSMGAmmo += ammo.ammoAmount;
                SoundManager.Instance.AmmoPickUpChannel.PlayOneShot(SoundManager.Instance.ammoBoxPickUp);
                break;
            case AmmoBox.AmmoType.HGAmmo:
                totalHGAmmo += ammo.ammoAmount;
                SoundManager.Instance.AmmoPickUpChannel.PlayOneShot(SoundManager.Instance.ammoBoxPickUp);
                break;
        }
    }
    private void AddWeaponToSocket(GameObject equippedWeapon)
    {
        DropCurrentWeapon(equippedWeapon);
        equippedWeapon.transform.SetParent(socket.transform);
        equippedWeapon.GetComponent<Rigidbody>().isKinematic = true;
        // equippedWeapon.GetComponent<MeshCollider>().enabled = false;
        equippedWeapon.GetComponent<BoxCollider>().enabled = false;
        equippedWeapon.transform.position = socket.transform.position;
        equippedWeapon.transform.rotation = socket.transform.rotation;
        WeaponScript weapon = equippedWeapon.GetComponent<WeaponScript>();
        weapon.animator.enabled = false;
        weapon.SetEquipped(true);
    }

    private void DropCurrentWeapon(GameObject equippedWeapon)
    {
        if (socket.transform.childCount > 0)
        {
            var weaponToDrop = socket.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<WeaponScript>().SetEquipped(false);
            weaponToDrop.transform.SetParent(equippedWeapon.transform.parent);
            weaponToDrop.transform.eulerAngles =
                new Vector3(weaponToDrop.transform.position.x, weaponToDrop.transform.position.y, weaponToDrop.transform.position.z);
            weaponToDrop.GetComponent<Rigidbody>().isKinematic = false;
            // weaponToDrop.GetComponent<MeshCollider>().enabled = true;
            weaponToDrop.GetComponent<Animator>().enabled = false;
            weaponToDrop.GetComponent<BoxCollider>().enabled = true;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, WeaponScript.WeaponModel model)
    {
        switch (model)
        {
            case WeaponScript.WeaponModel.HandgunM1911:
                totalHGAmmo -= bulletsToDecrease; break;
            case WeaponScript.WeaponModel.RifleAK74M:
                totalRifle_Ammo -= bulletsToDecrease; break;
            case WeaponScript.WeaponModel.ThompsonM1A1:
                totalSMGAmmo -= bulletsToDecrease; break;
        }
    }
    public int CheckAmmoLeft(WeaponScript.WeaponModel model)
    {
        switch (model)
        {
            case WeaponScript.WeaponModel.RifleAK74M:
                return totalRifle_Ammo;
            case WeaponScript.WeaponModel.ThompsonM1A1:
                return totalSMGAmmo;
            case WeaponScript.WeaponModel.HandgunM1911:
                return totalHGAmmo;
            default:
                return 0;
        }
    }

    public void PickUpThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
        }
    }

    private void PickupThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (equippedLethal == lethal || equippedLethal == Throwable.ThrowableType.None)
        {
            equippedLethal = lethal;
            if (lethalsCount < 2)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowableUI();
            }
        }
    }

    // private void PickUpGrenade()
    // {
    //     lethalsCount += 1;
    //
    //     HUDManager.Instance.UpdateThrowableUI();
    // }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0)
            {
                SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeThrow);
                ThrowLethal();
            }
        }

        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
                forceMultiplier = forceMultiplierLimit;
        }
    }
    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab();
        GameObject throwable =
            Instantiate(lethalPrefab, throwableSpawn.transform.position, throwableSpawn.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce*forceMultiplier), ForceMode.Impulse);
        throwable.GetComponent<Throwable>().setHasBeenThrown(true);
        lethalsCount -= 1;
        if (lethalsCount <= 0)
        {
            equippedLethal = Throwable.ThrowableType.None;
        }
        HUDManager.Instance.UpdateThrowableUI();
        forceMultiplier = 0f;
    }

    private GameObject GetThrowablePrefab()
    {
        switch (equippedLethal)
        {
            case Throwable.ThrowableType.Grenade:
                return throwablePrefab;
        }

        return new();
    }

    public int getLethalsCount()
    {
        return lethalsCount;
    }

    public void PickUpMedKit(Medkit medkit)
    {
        switch (medkit.medkitType)
        {
            case Medkit.MedKitType.SmallMedKit:
                player.GetComponent<PlayerScript>().UpdateHealthPoints(medkit.HPAmmount);
                break;
            case Medkit.MedKitType.BigMedKit:
                player.GetComponent<PlayerScript>().UpdateHealthPoints(medkit.HPAmmount);
                break;
        }
        Destroy(InteractionManager.Instance.hoveredMedKit.gameObject);
    }
}
