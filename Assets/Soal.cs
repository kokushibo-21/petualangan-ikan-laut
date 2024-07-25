using UnityEngine;
using UnityEngine.UI;

public class Soal : MonoBehaviour
{
    public Button audioButton;
    public AudioSource audioSource;
    public Sprite playIcon;
    public Sprite pauseIcon;

    private bool isPlaying = false;

    void Start()
    {
        // Set the initial icon to play
        audioButton.image.sprite = playIcon;

        // Add a listener to the button
        audioButton.onClick.AddListener(ToggleAudio);
    }

    void ToggleAudio()
    {
        if (isPlaying)
        {
            // Pause the audio and change the button icon to play
            audioSource.Pause();
            audioButton.image.sprite = playIcon;
        }
        else
        {
            // Play the audio and change the button icon to pause
            audioSource.Play();
            audioButton.image.sprite = pauseIcon;
        }

        // Toggle the playing state
        isPlaying = !isPlaying;
    }
}
