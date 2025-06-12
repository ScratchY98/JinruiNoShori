using UnityEngine;
public class TwoPartAudio : MonoBehaviour
{
    [SerializeField] private AudioClip IntroAudio;
    [SerializeField] private AudioClip LoopAudio;

    private AudioSource audioSource;
    private bool asPlayintro = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = IntroAudio;
        audioSource.loop = false;
        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying && !asPlayintro)
        {
            PlayLoop();
        }
    }

    private void PlayLoop()
    {
        asPlayintro = true;
        audioSource.clip = LoopAudio;
        audioSource.Play();
        audioSource.loop = true;
    }
}
