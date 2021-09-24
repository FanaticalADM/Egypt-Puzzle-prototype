using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClickSound;

    private AudioSource _audioSource;

    public void ButtonClickSound()
    {
        _audioSource.PlayOneShot(_buttonClickSound);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
