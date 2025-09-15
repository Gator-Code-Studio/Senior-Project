using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    public AudioClip background;
    public AudioClip punch;
    public AudioClip Sword; 
    public AudioClip jump;
    public AudioClip land;


private void Start()
{
        musicSource.clip = background;
        musicSource.Play();

}
public void PlaySFX(AudioClip clip)
{
        SFXSource.PlayOneShot(clip);
}
 

}