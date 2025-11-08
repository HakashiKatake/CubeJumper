# Upload Music Mode - Quick Setup 🎼

## What You Need

### Scene Components:
- ✅ TilesGenerator (with MusicalGenerator)
- ✅ MelodySequencer GameObject (empty notes)
- ✅ AudioMelodyExtractor GameObject
- ✅ Upload UI Panel
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ GameController

### Critical Requirement:
- ✅ **Sound files MUST be in `Assets/Resources/Sounds/`**

---

## Setup Checklist

### Step 1: Move Sound Files (CRITICAL!)
```
1. Create folders:
   Assets/Resources/Sounds/

2. Move/Copy ALL piano notes to this folder:
   - c1.ogg, c2.ogg, d3.ogg, e4.ogg, etc.
   - ALL your note files (c, d, e, f, g, a, b)
   - ALL octaves (0-8)
   
This is REQUIRED - AudioMelodyExtractor uses Resources.LoadAll()
```

### Step 2: TilesGenerator
```
GameObject: TilesGenerator
Components:
  ❌ Generator (DISABLED)
  ✅ MusicalGenerator (ENABLED)

MusicalGenerator Settings:
  - Tile Prefab: [Your Tile Prefab]
  - Melody Sequencer: [Drag MelodySequencer GameObject]
  - Music Analyzer: [Leave empty]
```

### Step 3: Create MelodySequencer
```
GameObject: MelodySequencer
Component: Melody Sequencer

Settings:
  Melody Notes → Size: 0 (leave empty - set dynamically)
  ❌ Loop Melody: UNCHECKED (songs don't loop by default)
  Note Volume: 0.8
```

### Step 4: Create AudioMelodyExtractor
```
1. Create Empty GameObject → Name: "AudioMelodyExtractor"
2. Add Component: Audio Melody Extractor
3. Add Component: Audio Source

AudioMelodyExtractor Settings:
  - Uploaded Audio: [Leave empty - set at runtime]
  - Audio Source: [The AudioSource component]
  - Sample Size: 4096
  - Sample Interval: 0.2 (seconds between note detection)
  - Volume Threshold: 0.01 (adjust if needed)
  - Reference Frequency: 440 (Hz for A4)
```

### Step 5: Create Upload UI

#### 5a. Create Panel
```
Canvas → UI → Panel
Name: "MusicUploadPanel"
Make it fullscreen
```

#### 5b. Add Upload Button
```
MusicUploadPanel → UI → Button - TextMeshPro
Name: "UploadButton"
Text: "📁 Upload Music"
Position: Center-top
```

#### 5c. Add Status Text
```
MusicUploadPanel → UI → Text - TextMeshPro
Name: "StatusText"
Text: "Upload your music to begin!"
Font Size: 24
Alignment: Center
Position: Below upload button
```

#### 5d. Add Progress Slider
```
MusicUploadPanel → UI → Slider
Name: "AnalysisProgressSlider"
Position: Below status text
Min: 0, Max: 1, Value: 0
```

#### 5e. Add Start Game Button
```
MusicUploadPanel → UI → Button - TextMeshPro
Name: "StartGameButton"
Text: "🎵 Start Game"
Position: Bottom center
❌ Interactable: UNCHECKED (disabled initially)
```

#### 5f. Add Back Button
```
Canvas → UI → Button - TextMeshPro
Name: "BackButton"
Text: "← Back to Menu"
Position: Top-left corner
```

### Step 6: Add MusicUploadUI Script
```
Click MusicUploadPanel
Add Component: Music Upload UI

Assign References:
  - Upload Button: [UploadButton]
  - Start Game Button: [StartGameButton]
  - Status Text: [StatusText]
  - Analysis Progress Slider: [AnalysisProgressSlider]
  - Upload Panel: [MusicUploadPanel]
  - Melody Extractor: [AudioMelodyExtractor GameObject]
  - Game Mode Manager: [Leave empty]
```

### Step 7: Configure Back Button
```
BackButton → Inspector → On Click():
  1. Click "+"
  2. Drag GameController GameObject
  3. Select: GameController.BackToMainMenu
```

### Step 8: GameController
```
GameObject: GameController
Component: GameController
Settings:
  - Main Menu Scene Name: "MainMenu"
```

---

## How It Works

### User Flow:
1. **Scene loads** → Upload panel visible
2. **Click "Upload Music"** → File picker opens (Unity Editor only)
3. **Select MP3/WAV/OGG** → File loads
4. **Analysis starts** → Progress bar updates
5. **Analysis completes** → "Start Game" button enabled
6. **Click "Start Game"** → Upload panel hides, game starts
7. **Tiles spawn** with notes from analyzed song
8. **Player jumps** → Song plays back through tiles!

### Technical Flow:
```
1. MusicUploadUI.OnUploadButtonClick()
   ↓
2. LoadAudioFile() via UnityWebRequest
   ↓
3. AudioMelodyExtractor.AnalyzeAudio()
   ↓
4. Process audio in chunks (every 0.2s)
   ↓
5. Detect pitch using autocorrelation
   ↓
6. Convert frequency to note name (c4, d5, etc.)
   ↓
7. Find matching AudioClip from Resources/Sounds/
   ↓
8. MelodySequencer.SetDynamicMelody()
   ↓
9. MusicalGenerator creates tiles with detected notes
   ↓
10. Player jumps, tiles play notes → Song recreated!
```

---

## Testing

### Quick Test (Without UI):

1. **Add MusicUploadTester** script to scene
2. **Assign a test audio clip** in Inspector
3. **Play scene**
4. **Press SPACE** → Analysis starts
5. **Check Console** for detected notes

### Full Test (With UI):

1. **Play scene**
2. **Click "Upload Music"**
3. **Select a simple audio file** (piano/instrumental works best)
4. **Wait for analysis** (10-30 seconds)
5. **Check Console** for detected notes count
6. **Click "Start Game"**
7. **Jump through tiles** → Should hear song notes

---

## Tuning Detection

### If too many notes detected:
```
AudioMelodyExtractor:
  - Increase Sample Interval: 0.3-0.5
  - Increase Volume Threshold: 0.02-0.05
```

### If too few notes detected:
```
AudioMelodyExtractor:
  - Decrease Sample Interval: 0.1-0.15
  - Decrease Volume Threshold: 0.005-0.008
  - Increase Sample Size: 8192
```

### For different music types:

**Instrumental/Piano music:**
```
Sample Interval: 0.2
Volume Threshold: 0.01
✅ Works best!
```

**Vocal music:**
```
Sample Interval: 0.15
Volume Threshold: 0.015
⚠️ May detect vocals too
```

**Electronic/Synth:**
```
Sample Interval: 0.25
Volume Threshold: 0.02
⚠️ May miss quiet notes
```

---

## Common Issues

❌ **"No notes detected"**
→ Volume threshold too high - lower to 0.005
→ Music file too quiet - try different file
→ Sample interval too high - try 0.1

❌ **"Analysis failed / No sample found for note"**
→ Sound files NOT in Resources/Sounds/ folder
→ Note files have wrong names (should be c4.ogg, d5.ogg, etc.)
→ Check Console for which notes are missing

❌ **Upload button doesn't work**
→ Only works in Unity Editor (not builds without native plugin)
→ Use MusicUploadTester for testing

❌ **Wrong notes detected**
→ Background noise in music - increase volume threshold
→ Complex music - try simpler instrumental track
→ Adjust sample interval

❌ **Tiles still play wrong notes**
→ Check MelodySequencer has dynamic notes assigned
→ Verify SetDynamicMelody() was called (check Console)

❌ **Analysis takes forever**
→ Sample interval too low - increase to 0.3+
→ Long music file - test with 30-60 second clips first

---

## Supported File Formats

### Unity Editor:
- ✅ WAV
- ✅ MP3
- ✅ OGG
- ✅ M4A (macOS)

### Builds:
- ✅ WAV (all platforms)
- ✅ OGG (all platforms)
- ⚠️ MP3 (most platforms)

**For best compatibility: Use WAV or OGG**

---

## Best Test Files

Start testing with:
1. **Single instrument** (piano, flute, guitar)
2. **Clear melody** (no heavy bass/drums)
3. **Slower tempo** (easier to detect notes)
4. **30-60 seconds** (faster testing)

**Good examples:**
- Classical piano pieces
- Video game music
- Simple folk songs

**Avoid for first tests:**
- Heavy metal/rock
- EDM/Dubstep
- Songs with lots of drums

---

**Once working, you can analyze ANY music! 🎼**
