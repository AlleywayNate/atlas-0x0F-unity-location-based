using UnityEngine;
using System.Collections;

public class ZombieAudio : MonoBehaviour
{
    public AudioSource idleSource;
    public AudioSource RageSource;
    public AudioSource DamageSource;
    public AudioSource deathSource;
    public AudioSource hurtSource;
    public AudioSource stepSource;
    
    public AudioClip[] idleSounds;
    public AudioClip[] RageSounds;
    public AudioClip[] DamageSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] hurtSounds;
    public AudioClip[] stepSounds;
        
        
    void Start()
    {
        StartCoroutine(PlayIdleSounds());
    }

    IEnumerator PlayIdleSounds()
    {
        while (true)
        {
            idleSource.clip = idleSounds[Random.Range(0, idleSounds.Length)];
            idleSource.Play();
            yield return new WaitForSeconds(Random.Range(5, 15)); // Play sounds at random intervals
        }
    }
}
