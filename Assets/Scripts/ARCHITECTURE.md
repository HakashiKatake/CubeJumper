# Musical Mode System Architecture

## Component Relationships

```
┌─────────────────────────────────────────────────────────────┐
│                    MUSICAL MODE SYSTEM                       │
└─────────────────────────────────────────────────────────────┘

┌──────────────────┐
│  AudioManager    │──── Manages music playback
│                  │     - Normal mode music
│                  │     - Musical mode music
│                  │     - Piano note SFX
└────────┬─────────┘
         │
         │ provides music to
         ↓
┌──────────────────┐
│  MusicAnalyzer   │──── Real-time audio analysis
│                  │     - FFT spectrum analysis
│                  │     - Intensity calculation
│                  │     - Beat detection
│                  │     - Frequency bands (8)
└────────┬─────────┘
         │
         │ sends data to
         ↓
┌──────────────────┐
│MusicalGenerator  │──── Tile generation
│                  │     - Reads music data
│                  │     - Decides tile type
│                  │     - Applies piano theme
│                  │     - Spawns tiles
└────────┬─────────┘
         │
         │ creates
         ↓
┌──────────────────┐
│   Tile Prefab    │──── Visual representation
│ + PianoTileVisuals│    - Piano key colors
│                  │     - Pulse with music
│                  │     - Glow on landing
└────────┬─────────┘
         │
         │ player interacts
         ↓
┌──────────────────┐
│      Cubie       │──── Player character
│  (Player)        │     - Jumps on tiles
│                  │     - Triggers effects
│                  │     - Plays piano notes
└──────────────────┘

         ┌────────────────────┐
         │ GameModeManager    │──── Mode switching
         │                    │     - Normal ↔ Musical
         │                    │     - Generator control
         │                    │     - Music control
         └────────────────────┘
```

## Data Flow Diagram

```
MUSICAL MODE GAME LOOP
======================

1. MUSIC PLAYING
   │
   └──→ AudioSource plays track
        │
        ↓
2. ANALYSIS PHASE (every frame)
   │
   ├──→ MusicAnalyzer.GetSpectrumData()
   │    └──→ samples[1024] ← raw FFT data
   │
   ├──→ AnalyzeSpectrum()
   │    └──→ frequencyBand[8] ← divided into bands
   │
   ├──→ CalculateIntensity()
   │    └──→ currentIntensity ← average of bands
   │
   └──→ DetectBeat()
        └──→ isBeat ← bass threshold check
        │
        ↓
3. TILE GENERATION (when needed)
   │
   ├──→ MusicalGenerator.GenerateTiles()
   │    │
   │    ├──→ Ask: ShouldBeHighJump()?
   │    │    │
   │    │    ├──→ Check intensity > average? → YES
   │    │    ├──→ Check isBeat? → YES  
   │    │    ├──→ Check bass strong? → YES
   │    │    └──→ Any YES? → Big Tile | All NO? → Small Tile
   │    │
   │    ├──→ Apply musicResponsiveness factor
   │    │    └──→ 70% follow music, 30% random
   │    │
   │    └──→ Spawn tile at position
   │         │
   │         ├──→ Set tag (smallTile/bigTile)
   │         ├──→ Apply piano material (black/white)
   │         └──→ Add PianoTileVisuals component
   │
   ↓
4. VISUAL EFFECTS (every frame)
   │
   └──→ PianoTileVisuals.Update()
        │
        ├──→ PulseWithMusic()
        │    └──→ scale = original + sin(time) * intensity
        │
        └──→ UpdateGlow() (when player lands)
             └──→ color fade over glowDuration
        │
        ↓
5. PLAYER INTERACTION
   │
   └──→ Cubie.OnCollisionEnter2D()
        │
        ├──→ Check: correct jump type?
        │    ├──→ Small jump + small tile = ✓
        │    └──→ Big jump + big tile = ✓
        │
        ├──→ Play piano note (C3 or C6)
        │
        ├──→ Trigger glow effect
        │
        └──→ Update score
        │
        ↓
6. TILE CLEANUP
   │
   └──→ TileScript.Update()
        └──→ Check if off-screen
             └──→ Trigger new tile generation
                  └──→ LOOP back to step 3
```

## Music Analysis Breakdown

```
FREQUENCY SPECTRUM ANALYSIS
============================

Input: AudioSource playing music
       ↓
GetSpectrumData(samples[1024])
       ↓
┌──────────────────────────────────┐
│   Raw FFT Data (1024 samples)    │
│  [freq0, freq1, ... freq1023]    │
└──────────────────────────────────┘
       ↓
Divide into 8 bands:
       ↓
Band 0: ████████ (Bass - 0-250 Hz)
Band 1: ██████   (Low Mid - 250-500 Hz)
Band 2: █████    (Mid - 500-2K Hz)
Band 3: ████     (High Mid - 2K-4K Hz)
Band 4: ███      (High - 4K-6K Hz)
Band 5: ██       (Very High - 6K-8K Hz)
Band 6: █        (Ultra High - 8K-12K Hz)
Band 7: █        (Top - 12K-20K Hz)
       ↓
Calculate Average:
       ↓
currentIntensity = sum(all bands) / 8
       ↓
Beat Detection:
       ↓
bassIntensity = (band[0] + band[1]) / 2
if (bassIntensity > beatThreshold) → BEAT!
       ↓
Tile Decision:
       ↓
if (intensity > avg * 1.2 OR isBeat OR bass > threshold)
   → BIG TILE (high jump)
else
   → SMALL TILE (low jump)
```

## Piano Key Theme

```
TILE TYPE MAPPING
=================

Small Tile (Short Jump)          Big Tile (High Jump)
─────────────────────           ──────────────────────
│                   │           │                    │
│   WHITE PIANO KEY │           │  BLACK PIANO KEY   │
│                   │           │                    │
│   ┌───────────┐   │           │   ┌──────────┐    │
│   │  C3 Note  │   │           │   │  C6 Note │    │
│   └───────────┘   │           │   └──────────┘    │
│                   │           │                    │
│  Glow: Yellow ✨  │           │   Glow: Cyan ✨    │
│                   │           │                    │
│  Height: +0.4     │           │   Height: +1.35    │
│  Jump: Left Arrow │           │   Jump: Right Arrow│
└───────────────────┘           └────────────────────┘

        ↑                               ↑
        │                               │
    Low Music                      High Music
    Intensity                      Intensity
```

## Mode Switching

```
GAME MODE MANAGER
=================

┌─────────────┐              ┌──────────────┐
│             │              │              │
│ NORMAL MODE │◄────────────►│ MUSICAL MODE │
│             │   Toggle     │              │
└─────────────┘              └──────────────┘
      │                             │
      ├─ Generator                  ├─ MusicalGenerator
      │  (random tiles)             │  (music-synced tiles)
      │                             │
      ├─ Regular music              ├─ Musical mode track
      │  (background)               │  (analyzed in real-time)
      │                             │
      ├─ Color cycling              ├─ Piano key theme
      │  (rainbow tiles)            │  (black & white)
      │                             │
      └─ Standard visuals           └─ Music-reactive visuals
                                       (pulse, glow)
```

## Performance Considerations

```
OPTIMIZATION NOTES
==================

✓ MusicAnalyzer: Once per frame
  - Sample size: 1024 (power of 2 for FFT efficiency)
  - Frequency bands: 8 (balanced detail vs performance)
  
✓ MusicalGenerator: Only when tile needed
  - Not every frame
  - Triggered by TileScript cleanup
  
✓ PianoTileVisuals: Per-tile, every frame
  - Material instances (isolated changes)
  - Auto-cleanup on destroy
  - Disable on low-end devices if needed
  
✓ Tile Count: Same as normal mode
  - 7 initial tiles
  - Generate 1 when 1 destroyed
  - No extra memory usage

⚠ Watch Out:
  - Large sample sizes (2048+) = more CPU
  - Emission effects = more GPU
  - Too many active tiles = lag
  → Use object pooling if needed
```

## Integration Points

```
EXISTING SYSTEMS
================

Your Original Code → Enhanced With Musical Mode
─────────────────    ────────────────────────────

Generator.cs         + MusicalGenerator.cs
                       (music-driven version)

AudioManager.cs      + Musical mode support
                       (dual music tracks)

Cubie.cs            + Piano tile detection
                       (glow trigger)

TileScript.cs       + Works unchanged
                       (cleanup logic)

MainMenu.cs         + Mode selection
                       (optional UI)

NOTHING BREAKS! All original features still work.
Musical mode is ADDITIVE, not destructive.
```

This architecture is designed to be:
- 🎯 **Modular**: Each component has one clear job
- 🔄 **Extensible**: Easy to add new features
- 🛡️ **Safe**: Doesn't break existing gameplay
- ⚡ **Performant**: Optimized for real-time music
- 🎨 **Flexible**: Highly configurable parameters
