// // using System;
// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEditor.PackageManager;
// // using UnityEngine;
// //
// // public class InteractionManager : MonoBehaviour
// // {
// //     public static InteractionManager Instance { get; set; }
// //     public WeaponScript hoverdWeapon = null;
// //     public AmmoBox hoveredAmmoBox = null;
// //     public Throwable hoveredThrowable = null;
// //     private void Awake()
// //     {
// //         if (Instance != null && Instance != this)
// //         {
// //             Destroy(gameObject);
// //         }
// //         else
// //         {
// //             Instance = this;
// //         }
// //     }
// //     private void Update()
// //     {
// //         // Camera mainCamera = Camera.main;
// //         // Debug.Log("Update: Main Camera is " + mainCamera);
// //         // if (mainCamera == null)
// //         // {
// //         //     Debug.LogError("Main camera not found during Update. Ensure your camera is tagged as 'MainCamera'.");
// //         //     return;
// //         // }
// //         Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
// //         RaycastHit hit;
// //         if (Physics.Raycast(ray, out hit, 2f))
// //         {
// //             GameObject raycastObjectHit = hit.transform.gameObject;
// //             if (raycastObjectHit.GetComponent<WeaponScript>() && raycastObjectHit.GetComponent<WeaponScript>().GetEquiped() == false)
// //             {
// //                 hoverdWeapon = raycastObjectHit.gameObject.GetComponent<WeaponScript>();
// //                 hoverdWeapon.gameObject.GetComponent<Outline>().enabled = true;
// //                 if (Input.GetKeyDown(KeyCode.E))
// //                 {
// //                     WeaponManager.Instance.EquipWeapon(raycastObjectHit.gameObject);
// //                 }
// //             }
// //             else
// //             {
// //                 if (hoverdWeapon)
// //                 {
// //                     hoverdWeapon.gameObject.GetComponent<Outline>().enabled = false;
// //                 }
// //             }
// //             //Ammo
// //             if (raycastObjectHit.GetComponent<AmmoBox>())
// //             {
// //                 hoveredAmmoBox = raycastObjectHit.gameObject.GetComponent<AmmoBox>();
// //                 hoveredAmmoBox.gameObject.GetComponent<Outline>().enabled = true;
// //                 if (Input.GetKeyDown(KeyCode.E))
// //                 {
// //                     WeaponManager.Instance.PickUpAmmo(hoveredAmmoBox);
// //                     Destroy(raycastObjectHit.gameObject);
// //                 }
// //             }
// //             else
// //             {
// //                 if (hoveredAmmoBox)
// //                 {
// //                     hoveredAmmoBox.gameObject.GetComponent<Outline>().enabled = false;
// //                 }
// //             }
// //             if (raycastObjectHit.GetComponent<Throwable>())
// //             {
// //                 hoveredThrowable = raycastObjectHit.gameObject.GetComponent<Throwable>();
// //                 hoveredThrowable.gameObject.GetComponent<Outline>().enabled = true;
// //                 if (Input.GetKeyDown(KeyCode.E))
// //                 {
// //                     if (!hoveredThrowable.getHasBeenThrown())
// //                     {
// //                         WeaponManager.Instance.PickUpThrowable(hoveredThrowable);
// //                         hoveredThrowable.gameObject.GetComponent<Outline>().enabled = false;
// //                     }
// //                     else
// //                     {
// //                         hoveredThrowable.gameObject.GetComponent<Outline>().enabled = false;
// //                         return;
// //                     }
// //                 }
// //             }
// //             else
// //             {
// //                 if (hoveredThrowable)
// //                 {
// //                     hoveredThrowable.gameObject.GetComponent<Outline>().enabled = false;
// //                 }
// //             }
// //         }
// //     }
// // }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class InteractionManager : MonoBehaviour
// {
//     public static InteractionManager Instance { get; private set; }
//     
//     public WeaponScript hoveredWeapon = null;
//     public AmmoBox hoveredAmmoBox = null;
//     public Throwable hoveredThrowable = null;
//
//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//         }
//         else
//         {
//             Instance = this;
//         }
//     }
//
//     private void Update()
//     {
//         HandleRaycast();
//     }
//
//     private void HandleRaycast()
//     {
//         Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
//         if (Physics.Raycast(ray, out RaycastHit hit, 2f))
//         {
//             GameObject hitObject = hit.transform.gameObject;
//             HandleWeaponHit(hitObject);
//             HandleAmmoHit(hitObject);
//             HandleThrowableHit(hitObject);
//         }
//         else
//         {
//             ClearHoveredObjects();
//         }
//     }
//
//     private void HandleWeaponHit(GameObject hitObject)
//     {
//         var weapon = hitObject.GetComponent<WeaponScript>();
//         if (weapon && !weapon.GetEquipped())
//         {
//             SetHoveredWeapon(weapon);
//             if (Input.GetKeyDown(KeyCode.E))
//             {
//                 WeaponManager.Instance.EquipWeapon(hitObject);
//             }
//         }
//         else if (hoveredWeapon)
//         {
//             ClearHoveredWeapon();
//         }
//     }
//
//     private void HandleAmmoHit(GameObject hitObject)
//     {
//         var ammoBox = hitObject.GetComponent<AmmoBox>();
//         if (ammoBox)
//         {
//             SetHoveredAmmoBox(ammoBox);
//             if (Input.GetKeyDown(KeyCode.E))
//             {
//                 WeaponManager.Instance.PickUpAmmo(ammoBox);
//                 Destroy(hitObject);
//             }
//         }
//         else if (hoveredAmmoBox)
//         {
//             ClearHoveredAmmoBox();
//         }
//     }
//
//     private void HandleThrowableHit(GameObject hitObject)
//     {
//         var throwable = hitObject.GetComponent<Throwable>();
//         if (throwable)
//         {
//             SetHoveredThrowable(throwable);
//             if (Input.GetKeyDown(KeyCode.E) && !throwable.getHasBeenThrown())
//             {
//                 WeaponManager.Instance.PickUpThrowable(throwable);
//                 throwable.GetComponent<Outline>().enabled = false;
//             }
//         }
//         else if (hoveredThrowable)
//         {
//             ClearHoveredThrowable();
//         }
//     }
//
//     private void SetHoveredWeapon(WeaponScript weapon)
//     {
//         ClearHoveredWeapon();
//         hoveredWeapon = weapon;
//         weapon.GetComponent<Outline>().enabled = true;
//     }
//
//     private void ClearHoveredWeapon()
//     {
//         if (hoveredWeapon)
//         {
//             hoveredWeapon.GetComponent<Outline>().enabled = false;
//             hoveredWeapon = null;
//         }
//     }
//
//     private void SetHoveredAmmoBox(AmmoBox ammoBox)
//     {
//         ClearHoveredAmmoBox();
//         hoveredAmmoBox = ammoBox;
//         ammoBox.GetComponent<Outline>().enabled = true;
//     }
//
//     private void ClearHoveredAmmoBox()
//     {
//         if (hoveredAmmoBox)
//         {
//             hoveredAmmoBox.GetComponent<Outline>().enabled = false;
//             hoveredAmmoBox = null;
//         }
//     }
//
//     private void SetHoveredThrowable(Throwable throwable)
//     {
//         ClearHoveredThrowable();
//         hoveredThrowable = throwable;
//         throwable.GetComponent<Outline>().enabled = true;
//     }
//
//     private void ClearHoveredThrowable()
//     {
//         if (hoveredThrowable)
//         {
//             hoveredThrowable.GetComponent<Outline>().enabled = false;
//             hoveredThrowable = null;
//         }
//     }
//
//     private void ClearHoveredObjects()
//     {
//         ClearHoveredWeapon();
//         ClearHoveredAmmoBox();
//         ClearHoveredThrowable();
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public WeaponScript hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;
    public Medkit hoveredMedKit = null;

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

    private void Update()
    {
        HandleRaycast();
    } 

    private void HandleRaycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            GameObject hitObject = hit.transform.gameObject;
            HandleWeaponHit(hitObject);
            HandleAmmoHit(hitObject);
            HandleThrowableHit(hitObject);
            HandleHealthHit(hitObject);
        }
        else
        {
            ClearHoveredObjects();
        }
    }

    private void HandleWeaponHit(GameObject hitObject)
    {
        var weapon = hitObject.GetComponent<WeaponScript>();
        if (weapon && !weapon.GetEquipped())
        {
            SetHoveredWeapon(weapon);
            if (Input.GetKeyDown(KeyCode.E))
            {
                WeaponManager.Instance.EquipWeapon(hitObject);
            }
        }
        else if (hoveredWeapon)
        {
            ClearHoveredWeapon();
        }
    }
    private void HandleHealthHit(GameObject hitObject)
    {
        var medKit = hitObject.GetComponent<Medkit>();
        if (medKit)
        {
            SetHoveredMedKit(medKit);
            if (Input.GetKeyDown(KeyCode.E))
            {
                WeaponManager.Instance.PickUpMedKit(medKit);
                Destroy(hitObject);
            }
        }
        else if (hoveredMedKit)
        {
            ClearHoveredMedKit();
        }
    }
    private void HandleAmmoHit(GameObject hitObject)
    {
        var ammoBox = hitObject.GetComponent<AmmoBox>();
        if (ammoBox)
        {
            SetHoveredAmmoBox(ammoBox);
            if (Input.GetKeyDown(KeyCode.E))
            {
                WeaponManager.Instance.PickUpAmmo(ammoBox);
                Destroy(hitObject);
            }
        }
        else if (hoveredAmmoBox)
        {
            ClearHoveredAmmoBox();
        }
    }

    private void HandleThrowableHit(GameObject hitObject)
    {
        var throwable = hitObject.GetComponent<Throwable>();
        if (throwable)
        {
            SetHoveredThrowable(throwable);
            if (Input.GetKeyDown(KeyCode.E) && !throwable.getHasBeenThrown())
            {
                WeaponManager.Instance.PickUpThrowable(throwable);
                throwable.GetComponent<Outline>().enabled = false;
            }
        }
        else if (hoveredThrowable)
        {
            ClearHoveredThrowable();
        }
    }
    

    private void ClearHoveredMedKit()
    {
        if (hoveredMedKit)
        {
            hoveredMedKit.GetComponent<Outline>().enabled = false;
            hoveredMedKit = null;
        }
    }

    private void SetHoveredWeapon(WeaponScript weapon)
    {
        ClearHoveredWeapon();
        hoveredWeapon = weapon;
        weapon.GetComponent<Outline>().enabled = true;
    }

    private void ClearHoveredWeapon()
    {
        if (hoveredWeapon)
        {
            hoveredWeapon.GetComponent<Outline>().enabled = false;
            hoveredWeapon = null;
        }
    }

    private void SetHoveredAmmoBox(AmmoBox ammoBox)
    {
        ClearHoveredAmmoBox();
        hoveredAmmoBox = ammoBox;
        ammoBox.GetComponent<Outline>().enabled = true;
    }

    private void ClearHoveredAmmoBox()
    {
        if (hoveredAmmoBox)
        {
            hoveredAmmoBox.GetComponent<Outline>().enabled = false;
            hoveredAmmoBox = null;
        }
    }

    private void SetHoveredThrowable(Throwable throwable)
    {
        ClearHoveredThrowable();
        hoveredThrowable = throwable;
        throwable.GetComponent<Outline>().enabled = true;
    }
    private void SetHoveredMedKit(Medkit medKit)
    {
        ClearHoveredThrowable();
        hoveredMedKit = medKit;
        medKit.GetComponent<Outline>().enabled = true;
    }
    private void ClearHoveredThrowable()
    {
        if (hoveredThrowable)
        {
            hoveredThrowable.GetComponent<Outline>().enabled = false;
            hoveredThrowable = null;
        }
    }

    private void ClearHoveredObjects()
    {
        ClearHoveredWeapon();
        ClearHoveredAmmoBox();
        ClearHoveredThrowable();
    }
}
