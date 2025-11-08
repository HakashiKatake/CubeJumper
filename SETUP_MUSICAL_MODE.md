# Musical Mode - Quick Setup 🎵

## What You Need

### Scene Components:
- ✅ TilesGenerator (with **MusicalGenerator** component)
- ✅ MelodySequencer GameObject
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ Score UI
- ✅ GameController

### What to Remove:
- ❌ Normal Generator (disable it)
- ❌ GameModeManager (not needed in separate scenes)

---

## Setup Checklist

### Step 1: TilesGenerator
```
GameObject: TilesGenerator
Components:
  ❌ Generator (DISABLED - uncheck it)
  ✅ MusicalGenerator (ENABLED - check it)

MusicalGenerator Settings:
  - Tile Prefab: [Your Tile Prefab]
  - Melody Sequencer: [Drag MelodySequencer GameObject]
  - Music Analyzer: [Leave empty - optional]
  - White Piano Key Material: [Optional]
  - Black Piano Key Material: [Optional]
```

### Step 2: Create MelodySequencer
```
1. Create Empty GameObject → Name: "MelodySequencer"
2. Add Component: Melody Sequencer

Settings:
  Melody Notes → Size: [Number of notes in your melody]
  Example for C Major scale (8 notes):
    Element 0: c4
    Element 1: d4
    Element 2: e4
    Element 3: f4
    Element 4: g4
    Element 5: f4
    Element 6: e4
    Element 7: d4
  
  ✅ Loop Melody: CHECKED
  Note Volume: 0.8
  Pitch Variation: 0 (or small like 0.05)
```

### Step 3: Link Everything
```
TilesGenerator → MusicalGenerator:
  - Melody Sequencer → [Drag MelodySequencer GameObject]

MelodySequencer:
  - Note Audio Source → [Auto-created AudioSource]
```
,
### Step 4: Update Cubie (Player)
```
Cubie → Cubie Script:
Make sure OnCollisionEnter2D plays tile notes:
  - Checks for TileNotePlayer component
  - Calls PlayNote() when landing on tile
```

### Step 5: GameController
```
GameObject: GameController
Component: GameController
Settings:
  - Main Menu Scene Name: "MainMenu"
```

---

## Optional: Create Melody Sequence Asset

For reusable melodies:

1. **Project window** → Right-click → Create → **Melody Sequence**
2. **Name it**: "TestMelody" or your song name
3. **Configure**:
   ```
   Notes: [Assign audio clips]
   Loop Melody: ✅ Checked
   ```
4. **Assign to MelodySequencer**:
   - Drag asset to "Melody Sequence" field

---

## Testing

1. **Play the scene**
2. **First tile** plays note 0 (e.g., c4)
3. **Second tile** plays note 1 (e.g., d4)
4. **Third tile** plays note 2 (e.g., e4)
5. **After last note**, loops back to first note
6. **Tiles spawn continuously** even after looping

---

## Common Issues

❌ **Tiles stop spawning after 7-8**
→ Check MusicalGenerator component is **ENABLED** (checkbox checked)
→ Verify TilesGenerator has MusicalGenerator component added

❌ **All tiles play same note**
→ Check MelodySequencer has multiple different notes assigned
→ Verify GetNextNote() is being called

❌ **No sound when landing**
→ Check Cubie script has TileNotePlayer detection code
→ Verify Note Audio Source exists and is not muted

❌ **Notes don't loop**
→ Check "Loop Melody" checkbox is **CHECKED**
→ Verify MelodySequencer.loopMelody = true

❌ **Wrong generator active**
→ Disable "Generator" component (old one)
→ Enable "MusicalGenerator" component (new one)

---

## Example Melodies

### Simple C Major Scale:
```
c4, d4, e4, f4, g4, a4, b4, c5
```

### Twinkle Twinkle:
```
c4, c4, g4, g4, a4, a4, g4
f4, f4, e4, e4, d4, d4, c4
```

### Mario Theme (simplified):
```
e4, e4, e4, c4, e4, g4, g3
```

---

**Your melody mode should now work perfectly! 🎹**
