using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }
    [Header("Ammo")] 
    [SerializeField] private TextMeshProUGUI magazineAmmoUI;
    [SerializeField] private TextMeshProUGUI totalAmmoUI;

    [Header("Throwables")] 
    [SerializeField] private Image lethalUI;
    [SerializeField] private TextMeshProUGUI lethalAmountI;
    
    [Header("EmptySlot")]
    [SerializeField] private Sprite emptySlot;

    [SerializeField] private Sprite greySlot;
    
    [Header("Weapons")] 
    private Dictionary<WeaponScript.WeaponModel, GameObject> WeaponModelsSprites;
    private GameObject selectedModel;
    [SerializeField] private GameObject AKRifleUI;
    [SerializeField] private GameObject HandGunUI;
    [SerializeField] private GameObject ThompsonUI;
    
    [Header("AmmoIcons")] 
    private Dictionary<WeaponScript.WeaponModel, GameObject> AmmoModelsSprites;
    private GameObject selectedAmmoModel;
    [SerializeField] private GameObject RifleAmmoUI;
    [SerializeField] private GameObject HandgunAmmoUI;
    [SerializeField] private GameObject SMGAmmoUI;

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

        LoadWeaponModelsSprites();
        LoadAmmoModelsSprites();
    }

    private void LoadAmmoModelsSprites()
    {
        AmmoModelsSprites = new Dictionary<WeaponScript.WeaponModel, GameObject>()
        {
            { WeaponScript.WeaponModel.HandgunM1911, HandgunAmmoUI },
            { WeaponScript.WeaponModel.ThompsonM1A1, SMGAmmoUI },
            { WeaponScript.WeaponModel.RifleAK74M, RifleAmmoUI },
        };
    }
    private void LoadWeaponModelsSprites()
    {
        WeaponModelsSprites = new Dictionary<WeaponScript.WeaponModel, GameObject>()
        {
            { WeaponScript.WeaponModel.HandgunM1911, HandGunUI },
            { WeaponScript.WeaponModel.ThompsonM1A1, ThompsonUI },
            { WeaponScript.WeaponModel.RifleAK74M, AKRifleUI },
        };
    }

    private void Update()
    {
        WeaponScript activeWeapon =
            WeaponManager.Instance.getSocket().GetComponentInChildren<WeaponScript>();
        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerTap}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeft(activeWeapon.thisWeaponModel)}";
            WeaponScript.WeaponModel model = activeWeapon.thisWeaponModel;
            
            if (!ReferenceEquals(selectedModel, null))
                selectedModel.SetActive(false);
            
            selectedModel = WeaponModelsSprites[activeWeapon.thisWeaponModel];
            selectedModel.SetActive(true);
            
            if(!ReferenceEquals(selectedAmmoModel, null))
                selectedAmmoModel.SetActive(false);
            
            selectedAmmoModel = AmmoModelsSprites[activeWeapon.thisWeaponModel];
            selectedAmmoModel.SetActive(true);
            
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
        }

        if (WeaponManager.Instance.getLethalsCount() <= 0)
        {
            lethalUI.sprite = greySlot;
        }
    }
    public void UpdateThrowableUI()
    {
        lethalAmountI.text = $"{WeaponManager.Instance.getLethalsCount()}";

        switch (WeaponManager.Instance.getEquippedLehtlType())
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}