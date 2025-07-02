using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    public AudioSource musicSource;
    
    [Header("Sound Effects")]
    public AudioClip[] pianoNotes; // Array of different piano notes
    public AudioSource sfxSource;
    
    [Header("Tile Piano Notes")]
    [Tooltip("Index in pianoNotes array for C3 (small tile)")]
    public int c3NoteIndex = 0; // Set this to the index of C3 in your pianoNotes array
    [Tooltip("Index in pianoNotes array for C6 (big tile)")]
    public int c6NoteIndex = 1; // Set this to the index of C6 in your pianoNotes array
    
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;
    
    public static AudioManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayBackgroundMusic();
    }
    
    public void PlayBackgroundMusic()
    {
        if (musicSource && backgroundMusic)
        {
            musicSource.clip = backgroundMusic;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    
    public void PlayPianoNote(int noteIndex)
    {
        if (sfxSource && noteIndex < pianoNotes.Length)
        {
            sfxSource.clip = pianoNotes[noteIndex];
            sfxSource.volume = sfxVolume;
            sfxSource.Play();
        }
    }
    
    // Play C3 for small tiles
    public void PlayC3Note()
    {
        PlayPianoNote(c3NoteIndex);
    }
    
    // Play C6 for big tiles
    public void PlayC6Note()
    {
        PlayPianoNote(c6NoteIndex);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicSource) musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}