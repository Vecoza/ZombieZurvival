using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int HP = 100;

    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealthUI;

    public GameObject gameOverUI;

    public static bool isAlive = true;

    public bool isDead;
    private void Start()
    {
        playerHealthUI.text = $"Health: {HP}";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            PlayerDead();
            isDead = true;
            
        }
        else
        {
            StartCoroutine(bloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    public void UpdateHealthPoints(int HPAddAmmount)
    {
        HP += HPAddAmmount;
        if (HP > 100)
        {
            HP = 100;
        }
        playerHealthUI.text = $"Health: {HP}";
    }
    private void PlayerDead()
    {
        isAlive = false;
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);
        
        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);
        
        InteractionManager.Instance.enabled = false;
        //Dying animaction
        var temp1 = GetComponentInChildren<CharacterController>().gameObject;
        var blabla = temp1.GetComponentInChildren<Animator>(true);
        blabla.enabled = true;
        
        playerHealthUI.gameObject.SetActive(false);
        
        GetComponent<ScreenFader>().StartFade();

        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        int waveSurivived = GlobalReferences.Instance.waveNumber;

        
        if (waveSurivived - 1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurivived-1);
        }


        StartCoroutine(ReturnToMainMenu());

        this.gameObject.SetActive(false);
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(6f);

        SceneManager.LoadScene("MainMenu");


    }


    private IEnumerator bloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();
 
        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 2f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
 
            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
 
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
 
            yield return null; ; // Wait for the next frame.
        }
        
        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
        
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
            
        }
    }
}
