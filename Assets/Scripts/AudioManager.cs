using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    public AudioSource musicSource;
    
    [Header("Musical Mode")]
    public AudioClip musicalModeTrack;
    public AudioSource musicalModeSource;
    
    [Header("Sound Effects")]
    public AudioClip[] pianoNotes; // Array of different piano notes
    public AudioSource sfxSource;
    
    [Header("Tile Piano Notes")]
    [Tooltip("Index in pianoNotes array for C3 (small tile/white key)")]
    public int c3NoteIndex = 0; // Set this to the index of C3 in your pianoNotes array
    [Tooltip("Index in pianoNotes array for C6 (big tile/black key)")]
    public int c6NoteIndex = 1; // Set this to the index of C6 in your pianoNotes array
    
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;
    
    public static AudioManager Instance;
    
    private bool isMusicalMode = false;
    
    void Awake()
    {
        // Don't use DontDestroyOnLoad for scene-specific audio
        // Each scene should have its own AudioManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If another AudioManager exists, destroy it (from previous scene)
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }
    
    void Start()
    {
        PlayBackgroundMusic();
    }
    
    void OnDestroy()
    {
        // Clear instance reference when destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    public void PlayBackgroundMusic()
    {
        if (isMusicalMode)
        {
            PlayMusicalModeMusic();
        }
        else if (musicSource && backgroundMusic)
        {
            musicSource.clip = backgroundMusic;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            musicSource.Play();
            
            if (musicalModeSource != null && musicalModeSource.isPlaying)
            {
                musicalModeSource.Stop();
            }
        }
    }
    
    public void PlayMusicalModeMusic()
    {
        if (musicalModeSource && musicalModeTrack)
        {
            musicalModeSource.clip = musicalModeTrack;
            musicalModeSource.volume = musicVolume;
            musicalModeSource.loop = true;
            musicalModeSource.Play();
            
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }
    }
    
    public void SetMusicalMode(bool enabled)
    {
        isMusicalMode = enabled;
        PlayBackgroundMusic();
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
    
    // Play C3 for small tiles (white piano keys)
    public void PlayC3Note()
    {
        PlayPianoNote(c3NoteIndex);
    }
    
    // Play C6 for big tiles (black piano keys)
    public void PlayC6Note()
    {
        PlayPianoNote(c6NoteIndex);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicSource) musicSource.volume = volume;
        if (musicalModeSource) musicalModeSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
    
    public AudioSource GetActiveMusicSource()
    {
        if (isMusicalMode && musicalModeSource != null)
        {
            return musicalModeSource;
        }
        return musicSource;
    }
}