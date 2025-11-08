# Complete Scene Structure Setup Guide 🎮

## Overview

Your game will have **4 scenes**:
1. **MainMenu** - Mode selection screen
2. **GameScene_Normal** - Classic random tile gameplay
3. **GameScene_Musical** - Predefined melody mode
4. **GameScene_UploadMusic** - Upload & analyze your own music

---

## Step 1: Create All Scenes

### 1.1 Create MainMenu Scene

1. **File → New Scene**
2. **Save As**: `MainMenu.unity` in `Assets/Scenes/`
3. Keep this scene open for now

### 1.2 Duplicate Your Current Scene

1. In Project window, find your current game scene
2. **Duplicate it 3 times** (Ctrl+D or Cmd+D)
3. Rename them:
   - `GameScene_Normal.unity`
   - `GameScene_Musical.unity`
   - `GameScene_UploadMusic.unity`

### 1.3 Add Scenes to Build Settings

1. **File → Build Settings**
2. Click **"Add Open Scenes"** for MainMenu
3. Drag all 4 scenes into the build list:
   ```
   [0] MainMenu
   [1] GameScene_Normal
   [2] GameScene_Musical
   [3] GameScene_UploadMusic
   ```
4. Click **Close**

---

## Step 2: Setup MainMenu Scene

### 2.1 Create UI

1. **Open MainMenu scene**
2. **Create Canvas**:
   - Right-click Hierarchy → UI → Canvas
   - Name it "MainMenuCanvas"

3. **Create Title**:
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name it "TitleText"
   - Text: "CUBE JUMPER"
   - Font Size: 72
   - Alignment: Center
   - Position: Top center of screen

4. **Create Mode Selection Panel**:
   - Right-click Canvas → UI → Panel
   - Name it "ModeSelectionPanel"
   - Position: Center of screen

5. **Create Buttons** (inside ModeSelectionPanel):
   
   **Normal Mode Button:**
   - Right-click ModeSelectionPanel → UI → Button - TextMeshPro
   - Name: "NormalModeButton"
   - Text: "Normal Mode"
   - Position: Top

   **Musical Mode Button:**
   - Duplicate NormalModeButton (Ctrl+D)
   - Name: "MusicalModeButton"
   - Text: "Musical Mode"
   - Position: Middle

   **Upload Music Mode Button:**
   - Duplicate again
   - Name: "UploadMusicModeButton"
   - Text: "Play Your Music"
   - Position: Bottom

6. **Create Quit Button** (optional):
   - Right-click Canvas → UI → Button - TextMeshPro
   - Name: "QuitButton"
   - Text: "Quit"
   - Position: Bottom left corner

### 2.2 Add MainMenuController Script

1. **Create empty GameObject** in MainMenu scene:
   - Right-click Hierarchy → Create Empty
   - Name: "MenuController"

2. **Add Component**:
   - Click MenuController GameObject
   - Add Component → **Main Menu Controller**

3. **Assign References** (in Inspector):
   - **Normal Mode Button** → Drag NormalModeButton
   - **Musical Mode Button** → Drag MusicalModeButton
   - **Upload Music Mode Button** → Drag UploadMusicModeButton
   - **Title Text** → Drag TitleText
   - **Mode Selection Panel** → Drag ModeSelectionPanel

4. **Verify Scene Names**:
   - Normal Mode Scene: `GameScene_Normal`
   - Musical Mode Scene: `GameScene_Musical`
   - Upload Music Scene: `GameScene_UploadMusic`

5. **(Optional) Setup Quit Button**:
   - Click QuitButton → Inspector
   - On Click() → Add MenuController → MainMenuController.QuitGame

### 2.3 Save MainMenu Scene

- **File → Save** (Ctrl+S / Cmd+S)

---

## Step 3: Setup GameScene_Normal (Classic Mode)

### 3.1 Open Scene
- Open `GameScene_Normal.unity`

### 3.2 Configure TilesGenerator

1. Find **"TilesGenerator"** GameObject
2. Make sure it has **Generator** component (NOT MusicalGenerator)
3. **Enable** Generator component (checkbox checked)
4. Assign:
   - **Tile Prefab** → Your tile prefab

### 3.3 Remove Musical Mode Components

Remove these if they exist:
- ❌ MusicalGenerator component
- ❌ MelodySequencer GameObject
- ❌ MusicAnalyzer GameObject
- ❌ GameModeManager GameObject
- ❌ AudioMelodyExtractor GameObject

Keep only:
- ✅ Generator
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ Score/UI

### 3.4 Add GameController

1. **Create empty GameObject**:
   - Right-click Hierarchy → Create Empty
   - Name: "GameController"

2. **Add Component**:
   - Add Component → **Game Controller**

3. **Add SceneTransitionManager** (IMPORTANT for clean audio):
   - Create another empty GameObject
   - Name: "SceneTransitionManager"
   - Add Component → **Scene Transition Manager**
   - This ensures audio doesn't carry over between scenes

4. **Create Pause Menu** (optional):
   - Right-click Canvas → UI → Panel
   - Name: "PauseMenu"
   - Add Resume, Restart, Main Menu buttons inside it
   - Initially set PauseMenu to **inactive** (uncheck in Inspector)

5. **Assign to GameController**:
   - Drag PauseMenu to **Pause Menu** field
   - Assign button references

### 3.5 Save Scene
- **File → Save**

---

## Step 4: Setup GameScene_Musical (Melody Mode)

### 4.1 Open Scene
- Open `GameScene_Musical.unity`

### 4.2 Configure TilesGenerator

1. Find **"TilesGenerator"** GameObject

2. **Disable Generator** component (uncheck it)

3. **Add MusicalGenerator** component:
   - Add Component → **Musical Generator**
   - **Enable it** (checkbox checked)

4. **Assign References**:
   - **Tile Prefab** → Your tile prefab
   - **Melody Sequencer** → (will create next)
   - **Music Analyzer** → Leave empty (optional)
   - **White Piano Key Material** → (optional)
   - **Black Piano Key Material** → (optional)

### 4.3 Create MelodySequencer

1. **Create empty GameObject**:
   - Right-click Hierarchy → Create Empty
   - Name: "MelodySequencer"

2. **Add Component**:
   - Add Component → **Melody Sequencer**

3. **Configure Melody**:
   - **Melody Notes** → Size: (however many notes you want)
   - Drag piano note audio clips (c4, d4, e4, f4, g4, etc.)
   - **Loop Melody** → ✅ Check this
   - **Note Volume** → 0.8

4. **Add AudioSource** (auto-created by script):
   - Verify AudioSource component exists
   - Spatial Blend → 0 (2D sound)

### 4.4 Link MelodySequencer to MusicalGenerator

1. Click **TilesGenerator** GameObject
2. In **Musical Generator** component:
   - **Melody Sequencer** → Drag MelodySequencer GameObject

### 4.5 Create Melody Sequence Asset (Optional but Recommended)

1. **Right-click in Project** → Create → **Melody Sequence**
2. Name it: "TestMelody"
3. Select it, in Inspector:
   - **Notes** → Size: 8 (for example)
   - Assign: c4, d4, e4, f4, g4, f4, e4, d4 (or any melody)
   - **Loop Melody** → ✅ Check
4. Drag this to MelodySequencer's **Melody Sequence** field

### 4.6 Remove Normal Mode Components

Remove these:
- ❌ Generator component (disable it)
- ❌ GameModeManager (not needed in separate scenes)

Keep:
- ✅ MusicalGenerator
- ✅ MelodySequencer
- ✅ Player (Cubie)
- ✅ Camera
- ✅ AudioManager
- ✅ Score/UI

### 4.7 Add GameController
- Same as Normal Mode (Step 3.4)

### 4.8 Save Scene
- **File → Save**

---

## Step 5: Setup GameScene_UploadMusic (Custom Music Mode)

### 5.1 Open Scene
- Open `GameScene_UploadMusic.unity`

### 5.2 Move Sound Files to Resources (CRITICAL!)

1. In Project window, create:
   ```
   Assets/Resources/Sounds/
   ```

2. **Copy ALL your piano note files** to this folder:
   - c1.ogg, c2.ogg, d3.ogg, e4.ogg, etc.
   - This is REQUIRED for AudioMelodyExtractor to find them

### 5.3 Configure TilesGenerator
- Same as Musical Mode (Step 4.2)

### 5.4 Create MelodySequencer
- Same as Musical Mode (Step 4.3)
- But DON'T assign notes yet (they'll be set dynamically)

### 5.5 Create AudioMelodyExtractor

1. **Create empty GameObject**:
   - Right-click Hierarchy → Create Empty
   - Name: "AudioMelodyExtractor"

2. **Add Component**:
   - Add Component → **Audio Melody Extractor**

3. **Configure Settings**:
   - **Sample Size**: 4096
   - **Sample Interval**: 0.2
   - **Volume Threshold**: 0.01
   - **Reference Frequency**: 440

4. **Add AudioSource**:
   - Add Component → Audio Source
   - Assign to **Audio Source** field in AudioMelodyExtractor

### 5.6 Create Upload UI

1. **Create UI Canvas** (if not exists):
   - Right-click Hierarchy → UI → Canvas

2. **Create Upload Panel**:
   - Right-click Canvas → UI → Panel
   - Name: "MusicUploadPanel"
   - Make it cover the screen

3. **Add Upload Button**:
   - Right-click MusicUploadPanel → UI → Button - TextMeshPro
   - Name: "UploadButton"
   - Text: "📁 Upload Music"
   - Position: Center-top

4. **Add Status Text**:
   - Right-click MusicUploadPanel → UI → Text - TextMeshPro
   - Name: "StatusText"
   - Text: "Upload your music to begin!"
   - Position: Below upload button
   - Font Size: 24

5. **Add Progress Slider**:
   - Right-click MusicUploadPanel → UI → Slider
   - Name: "AnalysisProgressSlider"
   - Position: Below status text

6. **Add Start Game Button**:
   - Right-click MusicUploadPanel → UI → Button - TextMeshPro
   - Name: "StartGameButton"
   - Text: "🎵 Start Game"
   - Position: Bottom center
   - **Initially disabled** (Interactable unchecked)

7. **Add Back Button**:
   - Right-click Canvas → UI → Button - TextMeshPro
   - Name: "BackButton"
   - Text: "← Back"
   - Position: Top-left corner

### 5.7 Add MusicUploadUI Script

1. Click **MusicUploadPanel**
2. **Add Component** → **Music Upload UI**
3. **Assign References**:
   - **Upload Button** → UploadButton
   - **Start Game Button** → StartGameButton
   - **Status Text** → StatusText
   - **Analysis Progress Slider** → AnalysisProgressSlider
   - **Upload Panel** → MusicUploadPanel
   - **Melody Extractor** → AudioMelodyExtractor GameObject
   - **Game Mode Manager** → Leave empty (not used)

### 5.8 Configure BackButton

1. Click **BackButton**
2. **On Click()** → Add New
3. Drag **GameController** → Select **GameController.BackToMainMenu**

### 5.9 Hide Upload Panel After Start

Modify MusicUploadUI or create a simple manager:
- When "Start Game" clicked → Hide MusicUploadPanel
- Show game UI

### 5.10 Save Scene
- **File → Save**

---

## Step 6: Final Setup

### 6.1 Test Scene Flow

1. **Play MainMenu scene**
2. Click each button:
   - Normal Mode → Should load GameScene_Normal
   - Musical Mode → Should load GameScene_Musical
   - Play Your Music → Should load GameScene_UploadMusic

3. **In each game scene**:
   - Press ESC → Pause menu appears
   - Click "Main Menu" → Returns to MainMenu

### 6.2 Verify Build Settings

**File → Build Settings** should show:
```
✅ MainMenu (index 0)
✅ GameScene_Normal (index 1)
✅ GameScene_Musical (index 2)
✅ GameScene_UploadMusic (index 3)
```

### 6.3 Set MainMenu as Default Scene

In **Build Settings**:
- Drag **MainMenu** to be index 0 (top of list)

---

## Quick Checklist

### MainMenu Scene:
- ✅ MainMenuController script added
- ✅ 3 mode buttons created and assigned
- ✅ Scene names match in Build Settings

### GameScene_Normal:
- ✅ Generator component enabled
- ✅ MusicalGenerator disabled/removed
- ✅ GameController added
- ✅ Pause menu setup

### GameScene_Musical:
- ✅ MusicalGenerator enabled
- ✅ Generator disabled
- ✅ MelodySequencer created with notes assigned
- ✅ Loop Melody checked
- ✅ GameController added

### GameScene_UploadMusic:
- ✅ Sound files in Resources/Sounds/
- ✅ AudioMelodyExtractor created
- ✅ MelodySequencer created (empty notes)
- ✅ Upload UI panel created
- ✅ MusicUploadUI script configured
- ✅ GameController added

### All Scenes:
- ✅ Added to Build Settings
- ✅ Player (Cubie) exists
- ✅ Camera configured
- ✅ AudioManager present
- ✅ UI (Score, etc.) working

---

## Troubleshooting

❌ **"Scene not found in Build Settings"**
→ Add all scenes to File → Build Settings

❌ **Tiles not spawning in Musical mode**
→ Check TilesGenerator has MusicalGenerator enabled (not Generator)

❌ **"No notes detected" in Upload mode**
→ Ensure sound files are in Assets/Resources/Sounds/

❌ **Buttons don't work in MainMenu**
→ Check MainMenuController references are assigned

❌ **Can't return to main menu**
→ Verify GameController.mainMenuSceneName = "MainMenu"

---

## Next Steps

Once setup complete:
1. **Test all 3 modes** work correctly
2. **Customize UI** styling for each mode
3. **Add mode descriptions** on MainMenu
4. **Create tutorial** for Upload Music mode
5. **Polish transitions** between scenes

**You now have a clean, modular 3-mode game! 🎮🎵**
