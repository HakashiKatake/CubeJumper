# Example Presets Guide

## How to Create Presets

### Step 1: Create the Preset Asset
```
1. Right-click in Project window
2. Create → CubeJumper → Musical Mode Preset
3. Name it (e.g., "ElectronicMusic_Preset")
```

### Step 2: Configure the Preset
Fill in the Inspector fields with your settings.

---

## Preset Examples

### 🎸 Preset 1: Electronic/Dance Music
**Best for:** EDM, House, Techno, Dubstep

```
Preset Name: Electronic Dance
Song Name: [Your song name]
Music Track: [Drag your audio clip]

MUSIC ANALYZER SETTINGS:
- Sample Size: 1024
- Intensity Multiplier: 4.0
- Beat Threshold: 0.7

GENERATOR SETTINGS:
- Music Responsiveness: 0.85
- High Jump Bias: 0.1

VISUAL SETTINGS:
- Pulse With Music: true
- Pulse Amount: 0.08
- Glow Duration: 0.4

GAMEPLAY:
- Game Speed: 2.5
```

**Why these settings?**
- High responsiveness = follows electronic beats well
- Lower beat threshold = catches more drops
- Higher pulse = matches energetic vibe
- Fast game speed = matches tempo

---

### 🎵 Preset 2: Pop/Rock Music
**Best for:** Pop, Rock, Alternative

```
Preset Name: Pop Rock
Song Name: [Your song name]
Music Track: [Drag your audio clip]

MUSIC ANALYZER SETTINGS:
- Sample Size: 1024
- Intensity Multiplier: 3.0
- Beat Threshold: 0.8

GENERATOR SETTINGS:
- Music Responsiveness: 0.7
- High Jump Bias: 0.0

VISUAL SETTINGS:
- Pulse With Music: true
- Pulse Amount: 0.05
- Glow Duration: 0.5

GAMEPLAY:
- Game Speed: 2.0
```

**Why these settings?**
- Balanced settings for varied song structure
- Standard beat threshold for regular drums
- Moderate pulse for cleaner look
- Standard speed for comfortable play

---

### 🎼 Preset 3: Epic/Cinematic Music
**Best for:** Soundtracks, Orchestral, Epic

```
Preset Name: Epic Soundtrack
Song Name: [Your song name]
Music Track: [Drag your audio clip]

MUSIC ANALYZER SETTINGS:
- Sample Size: 2048
- Intensity Multiplier: 2.5
- Beat Threshold: 1.0

GENERATOR SETTINGS:
- Music Responsiveness: 0.8
- High Jump Bias: 0.2

VISUAL SETTINGS:
- Pulse With Music: true
- Pulse Amount: 0.1
- Glow Duration: 0.7

GAMEPLAY:
- Game Speed: 1.8
```

**Why these settings?**
- Larger sample size = catches crescendos better
- Lower multiplier = smoother for gradual builds
- Higher beat threshold = only triggers on big hits
- More high jumps = epic feeling
- Longer glow = cinematic effect
- Slower speed = easier to appreciate music

---

### 🎤 Preset 4: Hip-Hop/Rap
**Best for:** Hip-Hop, Rap, Trap

```
Preset Name: Hip Hop
Song Name: [Your song name]
Music Track: [Drag your audio clip]

MUSIC ANALYZER SETTINGS:
- Sample Size: 1024
- Intensity Multiplier: 4.5
- Beat Threshold: 0.6

GENERATOR SETTINGS:
- Music Responsiveness: 0.9
- High Jump Bias: 0.15

VISUAL SETTINGS:
- Pulse With Music: true
- Pulse Amount: 0.07
- Glow Duration: 0.3

GAMEPLAY:
- Game Speed: 2.2
```

**Why these settings?**
- High sensitivity for bass-heavy tracks
- Low beat threshold catches 808s
- Very high responsiveness for tight rhythm
- Short glow for snappy feel
- Moderate speed for flow

---

### 🎹 Preset 5: Calm/Ambient Music
**Best for:** Chill, Ambient, Lo-fi

```
Preset Name: Chill Vibes
Song Name: [Your song name]
Music Track: [Drag your audio clip]

MUSIC ANALYZER SETTINGS:
- Sample Size: 1024
- Intensity Multiplier: 5.0
- Beat Threshold: 0.5

GENERATOR SETTINGS:
- Music Responsiveness: 0.6
- High Jump Bias: -0.1 (use 0, doesn't go negative)

VISUAL SETTINGS:
- Pulse With Music: true
- Pulse Amount: 0.03
- Glow Duration: 0.8

GAMEPLAY:
- Game Speed: 1.5
```

**Why these settings?**
- High multiplier compensates for quiet music
- Low beat threshold catches subtle rhythms
- Lower responsiveness for relaxed feel
- Subtle pulse for calm aesthetic
- Long glow for smooth transitions
- Slower speed for relaxed gameplay

---

### 🎸 Preset 6: Metal/Hard Rock
**Best for:** Metal, Hard Rock, Punk

```
Preset Name: Heavy Metal
Song Name: [Your song name]
Music Track: [Drag your audio clip]

MUSIC ANALYZER SETTINGS:
- Sample Size: 1024
- Intensity Multiplier: 2.0
- Beat Threshold: 1.2

GENERATOR SETTINGS:
- Music Responsiveness: 0.75
- High Jump Bias: 0.25

VISUAL SETTINGS:
- Pulse With Music: true
- Pulse Amount: 0.12
- Glow Duration: 0.3

GAMEPLAY:
- Game Speed: 2.8
```

**Why these settings?**
- Lower multiplier = constant loud music doesn't overwhelm
- Higher threshold = only big drum hits trigger
- More high jumps = matches intensity
- Strong pulse = aggressive feel
- Fast speed = matches tempo

---

## Quick Reference Chart

| Music Genre | Intensity Mult. | Beat Threshold | Responsiveness | Game Speed |
|-------------|-----------------|----------------|----------------|------------|
| Electronic  | 4.0 | 0.7 | 0.85 | 2.5 |
| Pop/Rock    | 3.0 | 0.8 | 0.70 | 2.0 |
| Epic        | 2.5 | 1.0 | 0.80 | 1.8 |
| Hip-Hop     | 4.5 | 0.6 | 0.90 | 2.2 |
| Chill       | 5.0 | 0.5 | 0.60 | 1.5 |
| Metal       | 2.0 | 1.2 | 0.75 | 2.8 |

---

## Parameter Effects Guide

### Intensity Multiplier
- **1.0-2.0**: For loud, constant music (metal, hard rock)
- **2.5-3.5**: For normal music (pop, rock)
- **4.0-6.0**: For quiet or dynamic music (chill, acoustic)
- **7.0-10.0**: For very quiet music or if tiles rarely change

### Beat Threshold
- **0.3-0.5**: Very sensitive (catches every little beat)
- **0.6-0.8**: Normal sensitivity (main beats)
- **0.9-1.2**: Less sensitive (only strong beats)
- **1.3-2.0**: Very selective (only huge drops/hits)

### Music Responsiveness
- **0.3-0.5**: More random, loosely follows music
- **0.6-0.8**: Balanced music following
- **0.9-1.0**: Strictly follows music analysis

### High Jump Bias
- **0.0**: Balanced mix of tile types
- **0.1-0.3**: Slightly more high jumps (harder)
- **0.4-0.6**: Many high jumps (very hard)
- **0.7-1.0**: Almost all high jumps (extreme)

### Game Speed
- **0.5-1.0**: Very slow (practice mode)
- **1.5-2.0**: Normal pace
- **2.5-3.0**: Fast (challenging)
- **3.0+**: Very fast (expert)

---

## Testing Your Preset

### 1. Create the preset
Follow steps above

### 2. Add to PresetManager
```
1. Find MusicalModeManager in hierarchy
2. Add PresetManager component (if not there)
3. Increase "Presets" array size
4. Drag your preset into the array
```

### 3. Test in-game
```
1. Start the game
2. Enable Musical Mode
3. Select your preset from dropdown (if UI setup)
   OR it will auto-load if it's first in array
4. Play and observe
```

### 4. Tune as needed
```
1. Enable MusicDebugVisualizer
2. Watch intensity values and beat detection
3. Adjust preset values
4. Test again
5. Repeat until perfect
```

---

## Advanced: Creating Multiple Presets for One Song

You can create difficulty variations:

**Easy Preset:**
```
- Music Responsiveness: 0.5 (more forgiving)
- High Jump Bias: 0.0 (balanced)
- Game Speed: 1.5 (slower)
```

**Normal Preset:**
```
- Music Responsiveness: 0.7
- High Jump Bias: 0.1
- Game Speed: 2.0
```

**Hard Preset:**
```
- Music Responsiveness: 0.9 (strict following)
- High Jump Bias: 0.3 (more high jumps)
- Game Speed: 2.5 (faster)
```

---

## Pro Tips

1. **Start with a preset from this guide** that matches your genre
2. **Use debug visualizer** to see what's actually happening
3. **Small adjustments** - change one thing at a time
4. **Test thoroughly** - play through the whole song
5. **Save variations** - keep old versions while testing new ones
6. **Document changes** - note what worked and what didn't
7. **Player feedback** - get others to test

---

## Preset Naming Convention

Good naming helps organization:

```
[Genre]_[SongName]_[Difficulty]
Examples:
- EDM_Levels_Hard
- Rock_Believer_Normal
- Chill_Lofi_Easy
- Epic_TwoSteps_Extreme
```

---

## Sharing Presets

To share a preset with others:
```
1. Select the preset asset in Project
2. Export Package
3. Share the .unitypackage file
4. Others can import it
```

---

## Template Preset

Copy this template when creating new presets:

```
=================================
PRESET: [Name]
SONG: [Song Name]
GENRE: [Genre]
DIFFICULTY: [Easy/Normal/Hard]
=================================

MUSIC ANALYZER:
□ Sample Size: 1024
□ Intensity Multiplier: 3.0
□ Beat Threshold: 0.8

GENERATOR:
□ Music Responsiveness: 0.7
□ High Jump Bias: 0.0

VISUALS:
□ Pulse With Music: true
□ Pulse Amount: 0.05
□ Glow Duration: 0.5

GAMEPLAY:
□ Game Speed: 2.0

NOTES:
[What works well?]
[What to watch out for?]
[Special sections in song?]
=================================
```

Happy preset creating! 🎵
