# Musical Melody Mode - Setup Guide

## 🎵 Overview

The new **Musical Melody Mode** lets players create the song by landing on tiles! Each tile plays a specific note in sequence, so as you progress through the level, you're actually playing the melody of a song note by note.

## 🎹 How It Works

```
Tile 1 → Note 1 (e.g., C)
Tile 2 → Note 2 (e.g., D)
Tile 3 → Note 3 (e.g., E)
Tile 4 → Note 4 (e.g., C)
...and so on
```

As the player jumps on each tile, they hear the next note in the song's melody, creating the music as they play!

## 📦 New Components

### 1. MelodySequencer
- Manages the sequence of musical notes
- Tracks which note should play next
- Handles looping when melody ends

### 2. TileNotePlayer
- Attached to each tile automatically
- Stores the specific note for that tile
- Plays the note when player lands

### 3. MelodySequence (ScriptableObject)
- Reusable melody asset
- Stores array of notes
- Easy to create and share

## 🚀 Quick Setup (3 Steps)

### Step 1: Prepare Your Musical Notes

You need audio files for each note in your melody. For example:

```
Common melody notes you might need:
- C3.wav, D3.wav, E3.wav, F3.wav, G3.wav, A3.wav, B3.wav (low octave)
- C4.wav, D4.wav, E4.wav, F4.wav, G4.wav, A4.wav, B4.wav (middle octave)
- C5.wav, D5.wav, E5.wav, F5.wav, G5.wav, A5.wav, B5.wav (high octave)
```

**Where to get notes:**
- Record them yourself with a piano/keyboard
- Download free piano samples online
- Use a synthesizer to generate them
- Extract from MIDI files

Import all note audio files into `Assets/Sounds/` folder.

### Step 2: Create a Melody Sequence

**Option A: Using ScriptableObject (Recommended)**

1. Right-click in Project window
2. Create → CubeJumper → Melody Sequence
3. Name it (e.g., "TwinkleTwinkleMelody")
4. In Inspector:
   - Set Melody Name
   - Set array size to number of notes
   - Drag each note audio clip in sequence
   - Enable "Loop Melody" if you want it to repeat

**Example: "Twinkle Twinkle Little Star"**
```
Notes array (12 notes):
[0] C4
[1] C4
[2] G4
[3] G4
[4] A4
[5] A4
[6] G4
[7] F4
[8] F4
[9] E4
[10] E4
[11] D4
```

**Option B: Direct Array Assignment**

You can also assign notes directly to the MelodySequencer component without creating an asset.

### Step 3: Setup MelodySequencer in Scene

1. Find or create your **MusicalModeManager** GameObject
2. Add **MelodySequencer** component
3. In the Inspector:
   - **Option A**: Drag your MelodySequence asset to "Melody Sequence" field
   - **Option B**: Manually set "Melody Notes" array size and drag clips
4. Configure settings:
   - Loop Melody: true (melody repeats) or false (stops at end)
   - Note Volume: 0.8 (adjust as needed)
   - Pitch Variation: 0.0 (or small amount for variety)

### Step 4: Connect to MusicalGenerator

1. Find your **MusicalTilesGenerator** GameObject
2. In the MusicalGenerator component:
   - Drag **MusicalModeManager** to "Melody Sequencer" field

**Done!** Now when you play, each tile will have a specific note assigned to it.

## 🎮 How To Use

### In Normal Mode
- Tiles still play C3 and C6 as before
- No melody system active

### In Musical Mode
1. Enable Musical Mode in GameModeManager
2. Start playing
3. Each tile you land on plays the next note in your melody
4. After all notes are played, it loops back (if loop enabled)

## 🎼 Creating Your Own Melodies

### Simple Songs to Start With

**1. Mary Had a Little Lamb**
```
E D C D E E E (pause) D D D (pause) E G G
```

**2. Hot Cross Buns**
```
E D C (pause) E D C (pause) C C C C D D D D E D C
```

**3. Ode to Joy**
```
E E F G G F E D C C D E E D D
```

### Tips for Good Melodies

✅ **Do:**
- Use recognizable songs
- Keep it simple at first (5-12 notes)
- Use notes from the same key
- Test with loop enabled

❌ **Don't:**
- Make it too long initially (causes memory issues)
- Use random notes (won't sound musical)
- Forget to loop (melody will stop)

## 📁 Organizing Your Notes

Recommended folder structure:
```
Assets/
  Sounds/
    Notes/
      Piano/
        C3.wav, D3.wav, E3.wav, etc.
      Guitar/
        C3.wav, D3.wav, E3.wav, etc.
    Melodies/
      TwinkleTwinkle/
        (organized notes for this song)
      MaryHadALamb/
        (organized notes for this song)
```

## 🔧 Advanced Features

### Dynamic Melody Switching

You can switch melodies during gameplay:

```csharp
// In your code
MelodySequence newMelody = /* load your melody */;
MelodySequencer.Instance.SetMelody(newMelody.notes);
```

### Progress Tracking

```csharp
// Get current position in melody
int currentNote = MelodySequencer.Instance.GetCurrentNoteIndex();

// Get progress percentage
float progress = MelodySequencer.Instance.GetMelodyProgress();

// Display on UI
progressText.text = $"Progress: {progress * 100:F0}%";
```

### Custom Note Assignment

You can manually assign specific notes to tiles:

```csharp
TileNotePlayer notePlayer = tile.GetComponent<TileNotePlayer>();
notePlayer.assignedNote = myCustomNote;
notePlayer.noteIndex = 5; // 5th note in sequence
```

## 🎨 Visual Feedback Ideas

### Show Note Names on Tiles

Add this to your UI or tile display:

```csharp
// Get the note name
string noteName = tileNotePlayer.GetNoteName();

// Display above tile
noteNameText.text = noteName;
```

### Color Code by Note

```csharp
// Different colors for different notes
if (noteName.Contains("C")) tileColor = Color.red;
else if (noteName.Contains("D")) tileColor = Color.orange;
else if (noteName.Contains("E")) tileColor = Color.yellow;
// etc.
```

### Progress Bar

Show how far through the melody the player is:

```csharp
progressBar.fillAmount = MelodySequencer.Instance.GetMelodyProgress();
```

## 🐛 Troubleshooting

### No sound when landing on tiles
**Check:**
- MelodySequencer has notes assigned
- Audio clips are not null
- MelodySequencer is referenced in MusicalGenerator
- Musical Mode is enabled

### Same note plays every time
**Check:**
- Melody sequence has multiple different notes
- Loop Melody is enabled
- Notes are in correct order in array

### Tiles don't have notes assigned
**Check:**
- TileNotePlayer component is being added (automatic)
- MusicalGenerator has melodySequencer reference
- MelodySequencer.Instance is not null

### Wrong notes playing
**Check:**
- Note order in melody sequence
- Audio files are correctly named/imported
- No duplicate or swapped clips

## 📊 Example Melody Sequences

### Preset 1: C Major Scale
```
C4, D4, E4, F4, G4, A4, B4, C5
(Good for testing - goes up the scale)
```

### Preset 2: Happy Birthday (First Line)
```
C4, C4, D4, C4, F4, E4
(Familiar tune everyone knows)
```

### Preset 3: Super Mario Theme (First Few Notes)
```
E5, E5, (pause), E5, (pause), C5, E5, (pause), G5
(Use silence/rest notes for pauses)
```

## 💡 Pro Tips

### 1. Use Consistent Note Lengths
All audio clips should be similar duration (e.g., 0.5 seconds each)

### 2. Add Silence Notes
Create "Rest.wav" (silent audio) for pauses in melody:
```
C4, D4, Rest, E4, F4, Rest, G4
```

### 3. Test with Short Melodies First
Start with 4-8 notes, then expand to full songs

### 4. Match Game Speed to Melody Tempo
- Fast melody = increase game speed
- Slow melody = decrease game speed

### 5. Loop Point Planning
Make sure loop sounds natural when it repeats

## 🎵 Melody Resources

### Where to Find Note Audio Files

**Free Resources:**
- Freesound.org (search "piano note C4")
- VSCO2 Community Edition (free piano samples)
- Musical Artifacts
- 99Sounds

**Paid Resources:**
- Piano in 162 (kontakt library)
- Native Instruments
- Spitfire Audio

### Tools to Create Your Own

**Software:**
- Audacity (free) - record and edit
- GarageBand (Mac) - built-in instruments
- FL Studio (trial) - synthesize notes
- LMMS (free) - create and export notes

### MIDI to Audio Conversion

1. Find MIDI file of song
2. Open in DAW (GarageBand, FL Studio, etc.)
3. Load piano instrument
4. Export each note separately as WAV

## 🚀 Next Steps

### After Basic Setup:
1. ✅ Create your first melody (simple song)
2. ✅ Test in game
3. ✅ Adjust loop setting
4. ✅ Fine-tune game speed

### Enhancement Ideas:
- [ ] Display note names on tiles
- [ ] Add visual feedback for correct rhythm
- [ ] Create difficulty levels (different melodies)
- [ ] Add score multiplier for musical accuracy
- [ ] Show musical staff with notes
- [ ] Add chord support (multiple notes per tile)

---

## Quick Reference

### Create Melody Asset
```
Right-click → Create → CubeJumper → Melody Sequence
```

### Assign to Sequencer
```
MusicalModeManager → MelodySequencer → Melody Sequence field
```

### Access in Code
```csharp
MelodySequencer.Instance.PlayNextNote();
AudioClip note = MelodySequencer.Instance.PeekNextNote();
int index = MelodySequencer.Instance.GetCurrentNoteIndex();
```

### Reset Melody
```csharp
MelodySequencer.Instance.ResetMelody();
```

---

**Your musical journey starts now! 🎵🎮🎹**

Create beautiful melodies and let players perform them by jumping!
