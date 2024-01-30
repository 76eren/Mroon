using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    public GameObject panel;
    private bool isPaused;

    void Awake() 
    {
        this.isPaused = panel.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            this.isPaused = !this.isPaused;
            panel.SetActive(this.isPaused);
        }
    }

    public void Restart() 
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }
}
