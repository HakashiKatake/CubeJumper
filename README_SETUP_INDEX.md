# CubeJumper - Complete Setup Index 📚

**Welcome to the complete setup guide for your 3-mode CubeJumper game!**

---

## 🎯 Quick Start - Read This First

### What You're Building:
A game with **3 different modes** accessible from a main menu:
1. **Normal Mode** - Classic random tile gameplay
2. **Musical Mode** - Predefined melody sequence gameplay  
3. **Upload Music Mode** - Analyze & play your own music

### Architecture:
```
MainMenu Scene
    ├─→ GameScene_Normal (Random tiles)
    ├─→ GameScene_Musical (Melody tiles)
    └─→ GameScene_UploadMusic (Custom music tiles)
```

---

## 📖 Setup Guide Documents

### Start Here:
**[COMPLETE_SCENE_SETUP_GUIDE.md](COMPLETE_SCENE_SETUP_GUIDE.md)** ⭐ START HERE
- Complete walkthrough for all 4 scenes
- Step-by-step instructions
- UI creation guide
- Build settings configuration

### Individual Mode Guides (Quick Reference):

**[SETUP_NORMAL_MODE.md](SETUP_NORMAL_MODE.md)**
- Fastest setup (simplest mode)
- Components needed
- Quick checklist
- Troubleshooting

**[SETUP_MUSICAL_MODE.md](SETUP_MUSICAL_MODE.md)**
- Melody setup guide
- MelodySequencer configuration
- Creating melody sequences
- Common issues + solutions

**[SETUP_UPLOAD_MODE.md](SETUP_UPLOAD_MODE.md)**
- Most complex setup
- Audio analysis configuration
- Upload UI creation
- Tuning detection parameters
- Testing guide

### Critical Fixes:
**[CRITICAL_FIX_ADD_COMPONENT.md](CRITICAL_FIX_ADD_COMPONENT.md)**
- Fix tiles not spawning issue
- How to add MusicalGenerator component

**[AUDIO_CLEANUP_FIX.md](AUDIO_CLEANUP_FIX.md)**
- Fix audio persisting between scenes
- Scene transition cleanup

**[HIGHSCORE_PERMODE_SETUP.md](HIGHSCORE_PERMODE_SETUP.md)** ⭐ NEW
- Separate high scores for each mode
- In-game high score display with fade effect
- Remove high score from main menu

---

## 🚀 Recommended Setup Order

### Phase 1: Create Scene Structure (30 min)
1. Read **COMPLETE_SCENE_SETUP_GUIDE.md** → Step 1-2
2. Create all 4 scenes
3. Add scenes to Build Settings
4. Setup MainMenu scene with UI

### Phase 2: Setup Normal Mode (15 min)
1. Read **SETUP_NORMAL_MODE.md**
2. Open GameScene_Normal
3. Configure Generator component
4. Test gameplay

### Phase 3: Setup Musical Mode (30 min)
1. Read **SETUP_MUSICAL_MODE.md**
2. Open GameScene_Musical
3. Add MusicalGenerator component
4. Create MelodySequencer
5. Assign melody notes
6. Test gameplay

### Phase 4: Setup Upload Music Mode (45 min)
1. Read **SETUP_UPLOAD_MODE.md**
2. **CRITICAL:** Move sound files to Resources/Sounds/
3. Open GameScene_UploadMusic
4. Create AudioMelodyExtractor
5. Build Upload UI
6. Test with simple audio file

### Phase 5: Final Testing (20 min)
1. Test MainMenu → All modes
2. Test pause/resume in each mode
3. Test back to menu functionality
4. Polish UI

**Total Time: ~2.5 hours**

---

## 📁 File Structure Reference

### Required Folder Structure:
```
Assets/
  ├── Scenes/
  │   ├── MainMenu.unity ⭐
  │   ├── GameScene_Normal.unity
  │   ├── GameScene_Musical.unity
  │   └── GameScene_UploadMusic.unity
  │
  ├── Scripts/
  │   ├── MainMenuController.cs ✅ (created)
  │   ├── GameController.cs ✅ (created)
  │   ├── Generator.cs (your original)
  │   ├── MusicalGenerator.cs (created earlier)
  │   ├── MelodySequencer.cs (created earlier)
  │   ├── AudioMelodyExtractor.cs ✅ (created)
  │   ├── MusicUploadUI.cs ✅ (created)
  │   ├── MusicUploadTester.cs ✅ (created)
  │   ├── TileScript.cs (updated earlier)
  │   ├── Cubie.cs (your player)
  │   └── ... (other scripts)
  │
  ├── Resources/ ⚠️ CRITICAL for Upload Mode
  │   └── Sounds/
  │       ├── c1.ogg
  │       ├── c2.ogg
  │       ├── d3.ogg
  │       └── ... (all piano notes)
  │
  └── Sounds/ (optional backup location)
      └── ... (note files)
```

---

## ✅ Component Checklist

### MainMenu Scene:
- ✅ Canvas
- ✅ MainMenuController GameObject
- ✅ 3 Mode selection buttons
- ✅ UI elements (title, panel)

### GameScene_Normal:
- ✅ TilesGenerator → **Generator** (enabled)
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ GameController
- ✅ Score UI

### GameScene_Musical:
- ✅ TilesGenerator → **MusicalGenerator** (enabled)
- ✅ MelodySequencer GameObject
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ GameController
- ✅ Score UI

### GameScene_UploadMusic:
- ✅ TilesGenerator → **MusicalGenerator** (enabled)
- ✅ MelodySequencer GameObject (empty notes)
- ✅ AudioMelodyExtractor GameObject
- ✅ Upload UI Panel
- ✅ MusicUploadUI script
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ GameController
- ✅ Score UI

---

## 🔧 Scripts Created

### New Scripts:
1. **MainMenuController.cs** - Mode selection & scene loading
2. **GameController.cs** - Pause, restart, back to menu
3. **AudioMelodyExtractor.cs** - Music analysis & pitch detection
4. **MusicUploadUI.cs** - Upload interface & file handling
5. **MusicUploadTester.cs** - Quick testing without UI

### Updated Scripts:
- **TileScript.cs** - Now works with both Generator types
- **MelodySequencer.cs** - Added SetDynamicMelody() method

### Existing Scripts (No Changes):
- Generator.cs
- MusicalGenerator.cs
- Cubie.cs
- AudioManager.cs

---

## 🎮 Game Flow

### Normal Mode Flow:
```
MainMenu → Click "Normal Mode"
    ↓
GameScene_Normal loads
    ↓
Generator spawns random tiles
    ↓
Player jumps, score increases
    ↓
ESC → Pause menu → Back to MainMenu
```

### Musical Mode Flow:
```
MainMenu → Click "Musical Mode"
    ↓
GameScene_Musical loads
    ↓
MusicalGenerator spawns tiles with melody notes
    ↓
Player jumps → Tile plays its assigned note
    ↓
Melody loops when reaching end
    ↓
ESC → Pause menu → Back to MainMenu
```

### Upload Music Mode Flow:
```
MainMenu → Click "Play Your Music"
    ↓
GameScene_UploadMusic loads
    ↓
Upload panel visible
    ↓
User clicks "Upload Music"
    ↓
File picker → Select audio file
    ↓
AudioMelodyExtractor analyzes (10-30s)
    ↓
Notes detected → MelodySequencer updated
    ↓
"Start Game" button enabled
    ↓
User clicks "Start Game"
    ↓
Upload panel hides, game starts
    ↓
Tiles play detected notes from song
    ↓
ESC → Pause menu → Back to MainMenu
```

---

## 🐛 Troubleshooting Quick Reference

### Scene Loading Issues:
❌ **"Scene not found"** → Add scene to Build Settings
❌ **Wrong scene loads** → Check scene name spelling in MainMenuController

### Tiles Not Spawning:
❌ **In Normal Mode** → Check Generator component enabled
❌ **In Musical Mode** → Check MusicalGenerator enabled, Generator disabled
❌ **After 7-8 tiles** → Add MusicalGenerator component (see CRITICAL_FIX)

### Audio Issues:
❌ **No sound on tiles** → Check TileNotePlayer component added
❌ **Same note every tile** → Check MelodySequencer has different notes
❌ **Notes don't loop** → Check "Loop Melody" checkbox

### Upload Mode Issues:
❌ **"No notes detected"** → Lower volume threshold to 0.005
❌ **"No sample found"** → Move note files to Resources/Sounds/
❌ **Upload doesn't work** → Only works in Unity Editor (or use test audio clip)
❌ **Wrong notes** → Adjust sample interval & volume threshold

---

## 📱 Build Settings

### Required Scenes (in order):
```
0. MainMenu
1. GameScene_Normal
2. GameScene_Musical
3. GameScene_UploadMusic
```

### Platform Considerations:

**Unity Editor:**
- ✅ All features work
- ✅ File upload works
- ✅ All audio formats supported

**Standalone (Windows/Mac/Linux):**
- ✅ All features work
- ⚠️ File upload needs native plugin (or use Resources folder)
- ✅ WAV/OGG work best

**WebGL:**
- ✅ Normal & Musical modes work
- ⚠️ Upload mode needs JavaScript integration
- ✅ Use OGG format

---

## 🎨 Customization Ideas

### Easy Customizations:
- Change button colors/styles
- Add mode descriptions on MainMenu
- Add loading screens between scenes
- Customize tile colors per mode

### Advanced Customizations:
- Add difficulty selection
- Save/load custom melodies
- Visualize upcoming notes
- Add rhythm scoring system
- Multiple melody presets
- Share analyzed songs

---

## 📊 Testing Checklist

### Before Release:

**MainMenu:**
- ✅ All 3 buttons work
- ✅ Scenes load correctly
- ✅ UI looks good
- ✅ Quit button works (standalone builds)

**Normal Mode:**
- ✅ Tiles spawn continuously
- ✅ Random heights work
- ✅ Sound plays on jump
- ✅ Score increases
- ✅ Pause menu works
- ✅ Back to menu works

**Musical Mode:**
- ✅ Tiles spawn continuously
- ✅ Each tile plays different note
- ✅ Melody loops correctly
- ✅ Notes play in sequence
- ✅ Pause menu works
- ✅ Back to menu works

**Upload Music Mode:**
- ✅ Upload UI appears
- ✅ Can upload audio file (Editor)
- ✅ Analysis completes successfully
- ✅ Detected notes shown in console
- ✅ Start button enables after analysis
- ✅ Game starts with detected notes
- ✅ Tiles play song notes
- ✅ Back to menu works

---

## 🎯 Success Criteria

### You'll know it's working when:

✅ **MainMenu loads** when you play
✅ **3 buttons** take you to different scenes
✅ **Normal mode** has random tile gameplay
✅ **Musical mode** plays melody through tiles
✅ **Upload mode** analyzes audio and plays it back
✅ **All modes** can return to MainMenu
✅ **No console errors** during gameplay

---

## 📞 Getting Help

### If you're stuck:

1. **Check console** for error messages
2. **Verify components** are assigned in Inspector
3. **Compare with checklists** in each guide
4. **Test one mode at a time** (start with Normal)
5. **Check scene names** match exactly in code

### Common Error Messages:

**"NullReferenceException"** → Component reference not assigned
**"Scene could not be loaded"** → Scene not in Build Settings
**"Object reference not set"** → Drag & drop GameObject in Inspector
**"Component cannot be added"** → Component already exists

---

## 🎉 You're Ready!

Follow the guides in order:
1. **COMPLETE_SCENE_SETUP_GUIDE.md** (overall structure)
2. **SETUP_NORMAL_MODE.md** (easiest first)
3. **SETUP_MUSICAL_MODE.md** (next)
4. **SETUP_UPLOAD_MODE.md** (advanced)

**Good luck! You're about to have 3 awesome game modes! 🎮🎵🎼**
