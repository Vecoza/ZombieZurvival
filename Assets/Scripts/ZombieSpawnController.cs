// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class ZombieSpawnController : MonoBehaviour
// {
//     public int initialZombiesPerWave = 5;
//     public int currentZombiesPerWave;
//
//     public float spawnDelay = 0.5f;  //Delay between spawinign each zombie in wave
//
//     public int currentWave = 0;
//     public float waveCoolDown = 10; // time in seconds between waves
//
//     public bool inCooldown;
//     public float cooldownCounter = 0; // we only use this for testing and the UI
//
//     public List<Enemy> currentZombiesAlive;
//
//     public GameObject zombiePrefab;
//
//     public TextMeshProUGUI waveOverUI;
//     public TextMeshProUGUI cooldownCounterUI;
//
//     public TextMeshProUGUI currentWaveUI;
//     
//     private void Start()
//     {
//         currentZombiesPerWave = initialZombiesPerWave;
//
//         StartNextWave();
//     }
//     /*
//     private void StartNextWave()
//     {
//         //pokusaj
//         
//         foreach (Enemy zombie in currentZombiesAlive)
//         {
//             if (zombie.isDead)
//             {
//                 Destroy(zombie.gameObject);
//             }
//         }
//         //
//         
//         
//         currentZombiesAlive.Clear();
//         
//         currentWave++;
//         currentWaveUI.text = "Wave: " + currentWave.ToString();
//         StartCoroutine(SpawnWave());
//
//
//     }
//     */
//
//     private IEnumerator SpawnWave()
//     {
//         for (int i = 0; i < currentZombiesPerWave; i++)
//         {
//             // Generate a random offset within a specified range
//
//             Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
//             Vector3 spawnPosition = transform.position + spawnOffset;
//             
//             //Instantiate the Zombie
//             var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
//             
//             //Get Enemy script
//             Enemy enemyScript = zombie.GetComponent<Enemy>();
//             
//             //Track this zombie
//             currentZombiesAlive.Add(enemyScript);
//
//             yield return new WaitForSeconds(spawnDelay);
//
//         }
//     }
//
//     /*
//     private void Update()
//     {
//         // Get all dead zombies
//         List<Enemy> zombiesToRemove = new List<Enemy>();
//         foreach (Enemy zombie in currentZombiesAlive)
//         {
//             if (zombie.isDead)
//             {
//                 zombiesToRemove.Add(zombie);
//                 //Destroy(zombie.gameObject);
//             }
//             
//         }
//         
//         //Actually remove all dead zombies
//         foreach (Enemy zombie in zombiesToRemove)
//         {
//             currentZombiesAlive.Remove(zombie);
//         }
//         
//         zombiesToRemove.Clear();
//
//         if (currentZombiesAlive.Count == 0 && inCooldown == false)
//         {
//             //Start cooldown for next wave
//             StartCoroutine(WaveCoolDown());
//
//         }
//         
//         //run the cooldown counter
//         
//         if(inCooldown)
//         {
//             cooldownCounter -= Time.deltaTime;
//         }
//         else
//         {
//             cooldownCounter = waveCoolDown;
//         }
//
//         cooldownCounterUI.text = cooldownCounter.ToString("F0");
//
//     }
//
//
//     private IEnumerator WaveCoolDown()
//     {
//         inCooldown = true;
//         waveOverUI.gameObject.SetActive(true);
//         yield return new WaitForSeconds(waveCoolDown);
//
//         inCooldown = false;
//         waveOverUI.gameObject.SetActive(false);
//         
//         currentZombiesPerWave *= 2;
//         
//         StartNextWave();
//     } 
//
//     
//     */
//     
//     private void Update()
//     {
//         // Provjeravamo svakog zombija na listi i uklanjamo one koji su mrtvi
//         currentZombiesAlive.RemoveAll(zombie =>
//         {
//             if (zombie.isDead)
//             {
//                 Destroy(zombie.gameObject, 10f);
//                 return true;
//             }
//             return false;
//         });
//
//         if (currentZombiesAlive.Count == 0 && !inCooldown)
//         {
//             StartCoroutine(WaveCoolDown());
//         }
//     
//         // Pokretanje odbrojavanja za vrijeme hlađenja
//         if (inCooldown)
//         {
//             cooldownCounter -= Time.deltaTime;
//         }
//         else
//         {
//             cooldownCounter = waveCoolDown;
//         }
//
//         cooldownCounterUI.text = cooldownCounter.ToString("F0");
//     }
//
//     private IEnumerator WaveCoolDown()
//     {
//         inCooldown = true;
//         waveOverUI.gameObject.SetActive(true);
//         yield return new WaitForSeconds(waveCoolDown);
//
//         inCooldown = false;
//         waveOverUI.gameObject.SetActive(false);
//     
//         currentZombiesPerWave *= 2;
//     
//         StartNextWave();
//     } 
//
//     private void StartNextWave()
//     {
//         // Prije početka novog talasa uništavamo sve mrtve zombije
//         foreach (Enemy zombie in currentZombiesAlive)
//         {
//             if (zombie.isDead)
//             {
//                 Debug.Log("Destroying zombie before next wave");
//                 Destroy(zombie.gameObject, 2f);
//             }
//         }
//         currentZombiesAlive.Clear();
//     
//         // Nastavljamo sa standardnim postupkom za početak novog talasa
//         currentWave++;
//         currentWaveUI.text = "Wave: " + currentWave.ToString();
//         StartCoroutine(SpawnWave());
//     }
// }
//
//
//
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    [Header("Wave Settings")]
    public int initialZombiesPerWave = 5;
    private int currentZombiesPerWave;
    public float spawnDelay = 0.5f;

    [Header("Cooldown Settings")]
    public float waveCoolDown = 10f;
    private bool inCooldown;
    private float cooldownCounter;

    [Header("UI Elements")]
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    [Header("Zombie Settings")]
    public GameObject zombiePrefab;
    private List<Enemy> currentZombiesAlive = new List<Enemy>();

    private int currentWave;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;
        StartNextWave();
    }

    private void Update()
    {
        // Remove dead zombies
        currentZombiesAlive.RemoveAll(zombie =>
        {
            if (zombie.isDead)
            {
                Destroy(zombie.gameObject, 5f);
                return true;
            }
            return false;
        });

        // Check if all zombies are dead and not in cooldown
        if (currentZombiesAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCoolDown());
        }

        // Update cooldown counter
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCoolDown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator WaveCoolDown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCoolDown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentZombiesPerWave *= 2;
        StartNextWave();
    }

    private void StartNextWave()
    {
        // Clear dead zombies before starting a new wave
        currentZombiesAlive.RemoveAll(zombie =>
        {
            if (zombie.isDead)
            {
                Destroy(zombie.gameObject, 2f);
                return true;
            }
            return false;
        });

        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;
        
        currentWaveUI.text = $"Wave: {currentWave}";
        StartCoroutine(SpawnWave());
    }
}
