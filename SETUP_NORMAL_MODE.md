# Normal Mode - Quick Setup ⚡

## What You Need

### Scene Components:
- ✅ TilesGenerator (with **Generator** component)
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ Score UI
- ✅ GameController

### What to Remove:
- ❌ MusicalGenerator
- ❌ MelodySequencer
- ❌ MusicAnalyzer
- ❌ GameModeManager
- ❌ AudioMelodyExtractor

---

## Setup Checklist

### Step 1: TilesGenerator
```
GameObject: TilesGenerator
Components:
  ✅ Generator (ENABLED)
  ❌ MusicalGenerator (DELETE or DISABLE)

Generator Settings:
  - Tile Prefab: [Your Tile Prefab]
```

### Step 2: Audio
```
AudioManager:
  - Background Music: [Your Background Music Clip]
  - Jump Sound: c4.wav or similar
  - Collision Sound: c6.wav or similar
```

### Step 3: GameController
```
GameObject: GameController
Component: GameController
Settings:
  - Pause Menu: [Optional]
  - Main Menu Scene Name: "MainMenu"
```

---

## Testing

1. **Play the scene**
2. **Tiles should spawn** randomly (small and big)
3. **Jump on tiles** - hear jump sound
4. **Score increases** with each jump
5. **Press ESC** - pause menu (if configured)

---

## Common Issues

❌ **Tiles not spawning**
→ Check Generator component is enabled
→ Verify Tile Prefab is assigned

❌ **No sound**
→ Check AudioManager has clips assigned
→ Verify AudioSource components exist

❌ **Can't return to menu**
→ Check GameController.mainMenuSceneName = "MainMenu"

---

**That's it! Normal mode is the simplest setup.** ✅
