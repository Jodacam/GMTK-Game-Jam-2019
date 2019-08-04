using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{

    [SerializeField]
    AudioClip buttonSound = null;

    AudioSource source;

    public void Start()
    {
        MusicController.Instance.SetMusic("Menu");
        source = GetComponent<AudioSource>();
    }

    public void GoToCredits()
    {
        source.PlayOneShot(buttonSound);
        SceneManager.LoadScene("Creditos");
    }

    public void GoToGame()
    {
        source.PlayOneShot(buttonSound);        
        SceneManager.LoadScene("GameScene");
    }

    private void Update() {
        if(Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
    }
}
