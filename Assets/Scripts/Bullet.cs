// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Bullet : MonoBehaviour
// {
//     
//     // ovo je vezano za zombie sto ja imam
//
//
//     [SerializeField] private int bulletDamage;
//     [SerializeField] private GameObject _gun;
//     
//     private void Start()
//     {
//         bulletDamage = _gun.GetComponent<WeaponScript>().weaponDamage;
//     }
//
//        private void OnCollisionEnter(Collision objectWeHit)
//        {
//            if (objectWeHit.gameObject.CompareTag("Enemy"))
//            {
//                if (objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
//                {
//                    objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
//                }
//
//                //CreateBulletImpactEffect(objectWeHit);
//
//                CreateBloodSprayEffect(objectWeHit);
//            }
//
//            if (objectWeHit.gameObject.CompareTag("Wall"))
//            {
//                print("hit a wall");
//
//                CreateBulletImpactEffect(objectWeHit);
//                
//                // Destroy(gameObject);
//
//            }
//            
//            
//       }
//
//        private void CreateBloodSprayEffect(Collision objectWeHit)
//        {
//            ContactPoint contact = objectWeHit.contacts[0];
//
//            GameObject bloodSprayPrefav = Instantiate(
//                GlobalReferences.Instance.bloodSprayEffect,
//                contact.point,
//                Quaternion.LookRotation(contact.normal)
//            );
//            
//            bloodSprayPrefav.transform.SetParent(objectWeHit.gameObject.transform);
//        }
//
//        void CreateBulletImpactEffect(Collision objectWeHit)
//        {
//            ContactPoint contact = objectWeHit.contacts[0];
//
//            GameObject hole = Instantiate(
//             GlobalReferences.Instance.bulletImpactEffectPrefab,
//             contact.point,
//             Quaternion.LookRotation(contact.normal)
//            );
//            
//            hole.transform.SetParent(objectWeHit.gameObject.transform);
//
//
//        }
//        
//        private void OnCollisionExit(Collision other)
//        {
//            // Destroy(this.gameObject, 2f);
//        }
//        /*
//       private void CreateBloodSprayEffect(Collision objectWeHit)
//       {
//           ContactPoint contact = objectWeHit.contacts[0];
//
//           GameObject hole = Instantiate(
//               GlobalReferences.Instace.bulletImpact
//           );
//           
//           
//
//       } */
// }
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletDamage;
    [SerializeField] private GameObject gun;

    private void Start()
    {
        bulletDamage = gun.GetComponent<WeaponScript>().weaponDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hitObject = collision.gameObject;

        if (hitObject.CompareTag("Enemy"))
        {
            HandleEnemyHit(collision, hitObject);
        }
        else if (hitObject.CompareTag("Wall"))
        {
            HandleWallHit(collision);
        }
    }

    private void HandleEnemyHit(Collision collision, GameObject enemyObject)
    {
        var enemy = enemyObject.GetComponent<Enemy>();
        if (enemy != null && !enemy.isDead)
        {
            enemy.TakeDamage(bulletDamage);
            CreateBloodSprayEffect(collision);
        }
    }

    private void HandleWallHit(Collision collision)
    {
        Debug.Log("Hit a wall");
        CreateBulletImpactEffect(collision);
    }

    private void CreateBloodSprayEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        var bloodSprayEffect = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        bloodSprayEffect.transform.SetParent(collision.transform);
    }

    private void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        var bulletImpactEffect = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        bulletImpactEffect.transform.SetParent(collision.transform);
    }

    private void OnCollisionExit(Collision other)
    {
        // If needed, handle logic for when the bullet exits a collision.
    }
}
