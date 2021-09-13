using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class CommonSounds : MonoBehaviour
{
    public AudioClip destroyBuilding;
    public AudioClip gameOver;
    public AudioClip punch;
    public AudioClip wave;
    [HideInInspector] public AudioSource audioSource;
    public static CommonSounds inst;
    [SerializeField] private Camera battlefieldCamera;
    [SerializeField] private Camera exploCamera;
        

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            audioSource = GetComponent<AudioSource>();
        }
        else if (inst != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayCLip(AudioClip clip, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.loop = loop;
    }

    public void GoToExploration()
    {
        transform.SetParent(exploCamera.transform);
        transform.localPosition = Vector3.zero;
    }

    public void GoToBattlefield()
    {
        transform.SetParent(battlefieldCamera.transform);
        transform.localPosition = Vector3.zero;
    }
}
