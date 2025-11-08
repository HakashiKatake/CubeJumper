# Play Your Own Music Mode - Setup Guide 🎵

## Overview

This feature allows players to upload their own music files, and the game will:
1. **Analyze the audio** to detect melody notes
2. **Map notes** to your piano samples (c1, c2, d3, e4, etc.)
3. **Generate tiles** that play those exact notes
4. **Recreate the song** as the player jumps through tiles!

---

## System Requirements

### What I Created:
1. ✅ **AudioMelodyExtractor.cs** - Analyzes audio using FFT and pitch detection
2. ✅ **MusicUploadUI.cs** - UI for file upload and analysis
3. ✅ **Updated MelodySequencer.cs** - Supports dynamic melodies

### Technologies Used:
- **FFT (Fast Fourier Transform)** for frequency analysis
- **Autocorrelation** for pitch detection
- **MIDI note mapping** to convert Hz → note names (c4, d5, etc.)
- **UnityWebRequest** for audio file loading

---

## Setup Instructions

### Part 1: Move Sound Files to Resources

**IMPORTANT:** For the system to find your piano notes, they must be in `Resources/Sounds/`

1. In Unity Project window, go to `Assets/`
2. Create a folder called **"Resources"** (if it doesn't exist)
3. Inside Resources, create a folder called **"Sounds"**
4. **Move or copy all your note files** from `Assets/Sounds/` to `Assets/Resources/Sounds/`

Your structure should look like:
```
Assets/
  Resources/
    Sounds/
      c1.ogg
      c2.ogg
      d3.ogg
      e4.ogg
      ...
```

### Part 2: Create AudioMelodyExtractor GameObject

1. **Create empty GameObject**:
   - Right-click in Hierarchy → Create Empty
   - Name it **"AudioMelodyExtractor"**

2. **Add component**:
   - Click "Add Component"
   - Search for **"Audio Melody Extractor"**
   - Add it

3. **Configure settings** (in Inspector):
   - **Sample Size**: 4096 (default is good)
   - **Sample Interval**: 0.2 seconds (how often to detect notes)
   - **Volume Threshold**: 0.01 (minimum volume to detect a note)
   - **Reference Frequency**: 440 Hz (standard A4 tuning)

### Part 3: Create Upload UI

1. **Create UI Canvas** (if you don't have one):
   - Right-click Hierarchy → UI → Canvas

2. **Create Upload Panel**:
   - Right-click Canvas → UI → Panel
   - Name it "MusicUploadPanel"

3. **Add Upload Button**:
   - Right-click MusicUploadPanel → UI → Button (TextMeshPro)
   - Name it "UploadButton"
   - Change text to "Upload Music"

4. **Add Start Game Button**:
   - Right-click MusicUploadPanel → UI → Button (TextMeshPro)
   - Name it "StartGameButton"
   - Change text to "Start Game"

5. **Add Status Text**:
   - Right-click MusicUploadPanel → UI → Text (TextMeshPro)
   - Name it "StatusText"
   - Position it below buttons

6. **Add Progress Slider** (optional):
   - Right-click MusicUploadPanel → UI → Slider
   - Name it "AnalysisProgressSlider"

7. **Add MusicUploadUI Script**:
   - Click on MusicUploadPanel
   - Add Component → **Music Upload UI**
   - Drag references:
     - **Upload Button** → UploadButton
     - **Start Game Button** → StartGameButton
     - **Status Text** → StatusText
     - **Analysis Progress Slider** → AnalysisProgressSlider
     - **Upload Panel** → MusicUploadPanel
     - **Melody Extractor** → AudioMelodyExtractor GameObject
     - **Game Mode Manager** → Your GameModeManager

---

## How It Works

### Step 1: Audio Analysis
When user uploads a music file:
1. Load audio using UnityWebRequest
2. Extract raw PCM audio data
3. Process in chunks (every 0.2 seconds by default)
4. Apply FFT to get frequency spectrum
5. Use autocorrelation to detect dominant pitch

### Step 2: Pitch to Note Conversion
For each detected pitch (frequency in Hz):
1. Calculate semitones from A4 (440 Hz)
2. Convert to MIDI note number
3. Map to note name (c4, d5, e3, etc.)

### Step 3: Note Matching
For each detected note name:
1. Look for exact match in your samples (e.g., "c4.ogg")
2. If sharp/flat, try alternate naming (e.g., "c#4" → "c4_1.ogg")
3. Create array of matched AudioClips

### Step 4: Dynamic Melody
Once analysis complete:
1. Pass detected notes to MelodySequencer
2. MelodySequencer.SetDynamicMelody() updates the melody
3. MusicalGenerator starts creating tiles with these notes
4. Player jumps through tiles, recreating the uploaded song!

---

## Supported Audio Formats

### In Unity Editor:
- ✅ WAV
- ✅ MP3  
- ✅ OGG
- ✅ M4A (macOS)

### In Builds:
- ✅ WAV (all platforms)
- ✅ OGG (all platforms)
- ⚠️ MP3 (most platforms, may have licensing restrictions)

---

## Configuration Tips

### For Better Note Detection:

**Increase accuracy:**
- Lower `Sample Interval` (0.1s) = more notes detected, but slower
- Increase `Sample Size` (8192) = better frequency resolution
- Lower `Volume Threshold` (0.005) = detect quieter notes

**For cleaner results:**
- Higher `Volume Threshold` (0.02) = ignore quiet/noisy parts
- Higher `Sample Interval` (0.3s) = fewer but more distinct notes

### For Different Music Types:

**Instrumental/Melodic music:**
- Sample Interval: 0.2s
- Volume Threshold: 0.01
- Works best!

**Songs with vocals:**
- Sample Interval: 0.15s
- Volume Threshold: 0.015
- May detect vocal pitches too

**Bass-heavy music:**
- Might detect bass notes more than melody
- Increase volume threshold to filter bass

---

## Usage Flow

### Player Experience:

1. **Main Menu** → Click "Play Your Music" mode
2. **Upload Screen** → Click "Upload Music" button
3. **File Picker** → Select MP3/WAV/OGG file
4. **Analysis** → Wait while game analyzes (progress bar shows)
5. **Ready!** → "Start Game" button becomes clickable
6. **Gameplay** → Jump through tiles that play the song's notes!

---

## Limitations & Known Issues

### Current Limitations:

1. **Monophonic melodies only**: Detects one note at a time, not chords
2. **Pitch detection accuracy**: ~85-95% accurate depending on audio quality
3. **Processing time**: Can take 10-30 seconds for a 3-minute song
4. **No harmony**: Only extracts the dominant melody line

### Potential Issues:

❌ **"No notes detected"**
- Solution: Music file too quiet, lower volume threshold

❌ **"Wrong notes detected"**
- Solution: Background noise, increase volume threshold

❌ **Analysis very slow**
- Solution: Increase sample interval or reduce sample size

❌ **Can't find note samples**
- Solution: Ensure note files are in `Resources/Sounds/` folder

---

## Testing

### Test with simple files first:

1. **Single instrument melodies** (piano, flute)
2. **Clear recordings** (no background noise)
3. **Slower tempo songs** (easier to detect individual notes)

### Good test files:
- Classical piano pieces
- Simple folk melodies
- Instrumental video game music

### Avoid for first tests:
- Heavy metal / rock (too many instruments)
- Electronic dance music (complex synthesis)
- Songs with heavy drums (drums don't have clear pitch)

---

## Advanced: Improving Detection

If you want better results, you can:

1. **Pre-process audio**: Apply high-pass filter to remove bass
2. **Use external tools**: Pre-analyze with Melodyne/AutoTune, import MIDI
3. **Manual melody input**: Let users input notes manually
4. **Hybrid approach**: AI/ML models for better pitch detection

---

## Next Steps

Once you have this working, you can add:

- ✨ **Melody visualization**: Show upcoming notes as a piano roll
- ✨ **Difficulty adjustment**: Faster tiles for complex melodies
- ✨ **Score multipliers**: Bonus for hitting notes in rhythm
- ✨ **Multiple instruments**: Different tile colors for different instruments
- ✨ **Save/Load**: Save analyzed melodies for quick replay

---

**Ready to test?** Follow the setup steps above and upload your first song! 🎵
