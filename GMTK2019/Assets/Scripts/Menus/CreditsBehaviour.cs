using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreditsBehaviour : MonoBehaviour
{

    [SerializeField]
    AudioClip buttonSound = null;

    AudioSource source;

    public void Start()
    {

        source = GetComponent<AudioSource>();
    }

    public void GoBack()
    {
        source.PlayOneShot(buttonSound);
        SceneManager.LoadScene("Menú");
    }
}
