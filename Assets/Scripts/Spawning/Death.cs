using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            Die();
        }    
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Die();
        }
    }

    private void Die() {
        int deaths = PlayerPrefs.GetInt("deahts") + 1;
        PlayerPrefs.SetInt("deaths", deaths);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
