# Musical Mode Setup Guide

## Overview
This guide explains how to set up and use the new Musical Mode feature in CubeJumper. Musical Mode synchronizes tile generation with music, creating a rhythm-based gameplay experience with piano-themed visuals.

## New Scripts Created

### 1. **MusicAnalyzer.cs**
Analyzes audio in real-time to extract music data:
- **Music Intensity**: Overall volume/energy level
- **Frequency Bands**: 8 different frequency ranges analyzed
- **Beat Detection**: Detects bass beats in the music
- **Tile Recommendations**: Suggests when to spawn high/low tiles based on music

### 2. **MusicalGenerator.cs**
Generates tiles synchronized with music:
- Replaces normal Generator during musical mode
- Creates piano-themed tiles (black and white)
- Height/jump type determined by music intensity
- Responsive to music changes with configurable settings

### 3. **GameModeManager.cs**
Manages switching between Normal and Musical modes:
- Toggles between generators
- Handles music playback switching
- Updates UI to show current mode
- Can be controlled via UI toggle or code

### 4. **PianoTileVisuals.cs**
Visual effects for piano-themed tiles:
- Pulse effect synchronized with music
- Glow effect when player lands
- Different colors for black/white piano keys
- Optional emission effects

## Unity Setup Instructions

### Step 1: Setup Audio Manager
1. Find your **AudioManager** GameObject in the scene
2. In the Inspector, add these new fields:
   - **Musical Mode Track**: Drag your music file here (the song that will drive tile generation)
   - **Musical Mode Source**: Add a new AudioSource component for musical mode music

### Step 2: Create Musical Mode Components
1. Create an empty GameObject named **"MusicalModeManager"**
2. Add the following components:
   - **MusicAnalyzer**
   - **GameModeManager**

### Step 3: Configure MusicAnalyzer
In the MusicAnalyzer component:
- **Music Source**: Assign the AudioSource that will play your musical mode track
- **Sample Size**: 1024 (default, power of 2)
- **Intensity Multiplier**: 3.0 (adjust for sensitivity)
- **Beat Threshold**: 0.8 (lower = more sensitive to beats)
- **Frequency Bands**: 8 (default)

### Step 4: Create MusicalGenerator
1. Find your **TilesGenerator** GameObject
2. Duplicate it and rename to **"MusicalTilesGenerator"**
3. Remove the **Generator** component
4. Add the **MusicalGenerator** component
5. Configure MusicalGenerator:
   - **Tile Prefab**: Same as normal Generator
   - **Music Analyzer**: Drag the MusicalModeManager GameObject
   - **Music Responsiveness**: 0.7 (how much it follows music vs random)
   - **High Jump Bias**: 0.0 (0 = balanced, 1 = more high jumps)

### Step 5: Configure GameModeManager
In the GameModeManager component:
- **Current Mode**: Start with Normal or Musical
- **Normal Generator**: Drag your original TilesGenerator
- **Musical Generator**: Drag the new MusicalTilesGenerator
- **Music Analyzer**: Drag the MusicalModeManager
- **Musical Mode Audio Source**: Assign the AudioSource for musical mode
- **Musical Mode Track**: Assign your music clip

### Step 6: Create Piano Key Materials (Optional but Recommended)
1. Create two new Materials:
   - **WhitePianoKey**: Set Albedo to pure white (RGB: 255, 255, 255)
   - **BlackPianoKey**: Set Albedo to pure black (RGB: 0, 0, 0)
2. Optional: Enable Emission for glow effects
3. Assign these to MusicalGenerator:
   - **White Piano Key Material**: WhitePianoKey
   - **Black Piano Key Material**: BlackPianoKey

### Step 7: Add Visual Effects to Tile Prefab
1. Open your **Tile Prefab**
2. Add the **PianoTileVisuals** component
3. Configure settings:
   - **Pulse With Music**: true
   - **Glow On Step**: true
   - **Pulse Amount**: 0.05
   - **Pulse Speed**: 2.0
   - **Glow Color**: Yellow for white keys, Cyan for black keys
   - **Glow Duration**: 0.5 seconds
   - **Glow Intensity**: 2.0

### Step 8: Add Mode Toggle to UI (Optional)
1. In your Game scene, add a **Toggle** UI element
2. In GameModeManager:
   - **Mode Text**: Assign a TextMeshProUGUI to display current mode
   - **Musical Mode Toggle**: Assign the Toggle
3. The toggle will now switch between modes

## How It Works

### Music Analysis
The MusicAnalyzer continuously analyzes the playing music:
```
Low Intensity → Small Tiles (White Piano Keys) → Short Jump
High Intensity → Big Tiles (Black Piano Keys) → High Jump
Beat Detected → Likely Big Tile
```

### Tile Generation Logic
```csharp
// Musical mode checks:
1. Is music intensity above average? → High tile
2. Is this a beat moment? → High tile  
3. Are bass frequencies strong? → High tile
4. Otherwise → Low tile (with some randomness)
```

### Visual Feedback
- **White Tiles** = Low notes, short jumps, glow yellow
- **Black Tiles** = High notes, big jumps, glow cyan
- **Pulse Effect** = Tiles breathe with music intensity
- **Glow Effect** = Tiles light up when player lands

## Testing Musical Mode

### Test in Unity Editor:
1. Start the game
2. Toggle Musical Mode on (via Toggle UI or GameModeManager)
3. Play and observe tiles syncing with music
4. Adjust settings if needed:
   - **Too many big tiles?** Lower `Beat Threshold` in MusicAnalyzer
   - **Not responsive enough?** Increase `Intensity Multiplier`
   - **Want more randomness?** Lower `Music Responsiveness` in MusicalGenerator

### Choosing Good Music:
Musical mode works best with songs that have:
- ✓ Clear beats and rhythm
- ✓ Varying intensity (quiet and loud parts)
- ✓ Strong bass/drums
- ✗ Avoid: Very quiet/ambient music
- ✗ Avoid: Constant loud music (everything becomes high tiles)

## Customization Options

### Adjust Music Sensitivity
```csharp
// In MusicAnalyzer Inspector:
intensityMultiplier: 1.0 - 10.0 (higher = more sensitive)
beatThreshold: 0.1 - 2.0 (lower = easier to trigger beats)
```

### Adjust Tile Generation Balance
```csharp
// In MusicalGenerator Inspector:
musicResponsiveness: 0.0 - 1.0 (1 = fully follow music, 0 = random)
highJumpBias: 0.0 - 1.0 (adds bias towards more high jumps)
```

### Change Piano Key Colors
Modify the materials or use code:
```csharp
// In PianoTileVisuals:
glowColor = Color.cyan; // For black keys
glowColor = Color.yellow; // For white keys
```

## Troubleshooting

### Problem: Tiles not syncing with music
**Solution**: 
- Check that MusicAnalyzer.musicSource is assigned
- Verify the music is actually playing
- Increase intensityMultiplier

### Problem: All tiles are the same type
**Solution**:
- Check beatThreshold (might be too high or low)
- Adjust musicResponsiveness in MusicalGenerator
- Verify music has varying intensity

### Problem: Visual effects not showing
**Solution**:
- Ensure PianoTileVisuals is on the tile prefab
- Check that materials support emission
- Verify glowOnStep is enabled

### Problem: Wrong generator active
**Solution**:
- Check GameModeManager currentMode setting
- Verify correct generator is enabled in hierarchy
- Only one generator should be active at a time

## Code Integration Example

### Switching Modes from Code:
```csharp
// Switch to musical mode
GameModeManager.Instance.SetGameMode(GameModeManager.GameMode.Musical);

// Switch to normal mode
GameModeManager.Instance.SetGameMode(GameModeManager.GameMode.Normal);

// Toggle between modes
GameModeManager.Instance.ToggleMode();

// Check current mode
if (GameModeManager.Instance.IsMusicalMode())
{
    // Musical mode is active
}
```

### Getting Music Data:
```csharp
// Get current music intensity
float intensity = MusicAnalyzer.Instance.GetIntensity();

// Check if beat detected
if (MusicAnalyzer.Instance.IsBeat())
{
    // Do something on beat
}

// Get specific frequency band (0-7)
float bass = MusicAnalyzer.Instance.GetFrequencyBand(0);
```

## Future Enhancement Ideas

1. **Visual Spectrum Analyzer**: Display frequency bands as bars in UI
2. **Combo System**: Bonus points for staying on-beat
3. **Multiple Songs**: Song selection menu
4. **Difficulty Scaling**: Faster music = faster game speed
5. **Perfect Hit Detection**: Extra points for landing exactly on beat
6. **Color Themes**: Different color schemes for different songs
7. **Practice Mode**: Slow down music to learn patterns

## Performance Notes

- MusicAnalyzer runs every frame - keep sampleSize reasonable (1024 or 2048)
- Each tile with PianoTileVisuals has its own material instance
- Emission effects can be disabled on lower-end devices
- Consider object pooling for tiles if performance is an issue

---

**Enjoy your new Musical Mode!** 🎵🎹🎮
