using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{

    public new Camera camera;
    public new Sprite enabled;
    public Sprite disabled;
    private Image image;
    private AudioSource[] allAudioSources;


    void Start()
    {
        image = GetComponent<Image>();

        Debug.Log("sound_controller");

        //si encara no s'ha loguejat vol dir que acaba d'obrir el joc i activem el audio
        Debug.Log("currentUser = " + Main.currentUser);
        if (Main.currentUser == 0) Main.AudioEnabled = true;

        Debug.Log("audio_enabled = " + Main.AudioEnabled);


        //si no hi ha audio posem l'icona adequat
        // if(!Main.AudioEnabled) image.sprite = disabled;

        //camera.GetComponent<AudioListener>().volume = 0.0f; = Main.AudioEnabled;

        /* AudioListener audioListener = camera.GetComponent<AudioListener>();
         if (audioListener != null)
         {
             audioListener.volume = 0.0f;
         }   */

        allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = !Main.AudioEnabled;
        }

        //si no hi ha audio posem l'icona adequat
        if (Main.AudioEnabled) image.sprite = enabled;
        else image.sprite = disabled;
    }

    public void AudioClick()
    {
        Main.AudioEnabled = !Main.AudioEnabled;
        //camera.GetComponent<AudioListener>().enabled = false;
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = !Main.AudioEnabled;
        }
        if (Main.AudioEnabled) image.sprite = enabled;
        else image.sprite = disabled;

        Debug.Log("audio_enabled = " + Main.AudioEnabled);
    }

}
