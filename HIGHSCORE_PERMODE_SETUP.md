# Per-Mode High Score System - Setup Guide 🏆

## Overview

Each game mode now has its **own separate high score**:
- ✅ **Normal Mode** - High score for normal gameplay
- ✅ **Musical Mode** - High score for musical mode
- ✅ **Upload Music Mode** - High score for custom music mode

High scores are displayed **in-game** at the start and **fade away** when the player moves.

---

## What Changed

### 1. HighScoreManager.cs ✅ (Updated)
- Now stores **3 separate high scores** (one per mode)
- Uses different PlayerPrefs keys:
  - `HighScore_Normal`
  - `HighScore_Musical`
  - `HighScore_Upload`
- Automatically sets game mode based on which scene you're in

### 2. HighScoreDisplay.cs ✅ (NEW)
- Displays high score at start of game
- **Fades out** when player moves (jumps on first tile)
- Or fades after a timer
- Configurable fade duration and delay

### 3. ScoreDisplay.cs ✅ (NEW)
- Helper component to track current score
- Provides easy access to score value for other scripts

---

## Setup Instructions

### Setup for Each Game Mode Scene

Do this for **GameScene_Normal**, **GameScene_Musical**, and **GameScene_UploadMusic**:

#### Step 1: Add ScoreDisplay Component

1. **Find your Score Text** in the scene:
   - Look in Canvas → find the TextMeshProUGUI showing score
   - It's probably named "Score", "ScoreText", or similar

2. **Add ScoreDisplay component**:
   - Click on the score text GameObject
   - Add Component → **Score Display**
   - **Score Text** field → Should auto-assign, or drag the TextMeshProUGUI

#### Step 2: Create High Score Display UI

1. **Create UI Text for High Score**:
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name: "HighScoreText"
   
2. **Position it**:
   - Place it near top-center of screen
   - Above or below the current score
   - Make it visible and prominent

3. **Style the text**:
   - Font Size: 36-48 (big enough to see)
   - Alignment: Center
   - Color: White or gold
   - Example text: "High Score: 999"

4. **Add CanvasGroup** (for fading):
   - Click HighScoreText GameObject
   - Add Component → **Canvas Group**

#### Step 3: Add HighScoreDisplay Component

1. **Add the script**:
   - Click HighScoreText GameObject
   - Add Component → **High Score Display**

2. **Configure settings**:
   ```
   UI References:
     - High Score Text: [Drag the TextMeshProUGUI component]
   
   Fade Settings:
     - Display Duration: 2 (seconds before fade if not using player move)
     - Fade Duration: 1 (how long the fade takes)
     - ✅ Fade On Player Move: CHECKED (recommended)
   
   Game Mode:
     - GameScene_Normal: Set to "Normal"
     - GameScene_Musical: Set to "Musical"
     - GameScene_UploadMusic: Set to "Upload"
   ```

---

## Configuration Per Scene

### GameScene_Normal:
```
HighScoreDisplay settings:
  - High Score Text: [HighScoreText TMP]
  - Display Duration: 2
  - Fade Duration: 1
  - Fade On Player Move: ✅ Checked
  - Game Mode: Normal  ← IMPORTANT!
```

### GameScene_Musical:
```
HighScoreDisplay settings:
  - High Score Text: [HighScoreText TMP]
  - Display Duration: 2
  - Fade Duration: 1
  - Fade On Player Move: ✅ Checked
  - Game Mode: Musical  ← IMPORTANT!
```

### GameScene_UploadMusic:
```
HighScoreDisplay settings:
  - High Score Text: [HighScoreText TMP]
  - Display Duration: 2
  - Fade Duration: 1
  - Fade On Player Move: ✅ Checked
  - Game Mode: Upload  ← IMPORTANT!
```

---

## How It Works

### Game Start:
1. **HighScoreDisplay** loads the high score for current mode
2. Displays it on screen: "High Score: 25"
3. Waits for player to move

### Player Moves (Jumps on First Tile):
1. **ScoreDisplay** detects score changed from 0 to 1
2. **HighScoreDisplay** is notified
3. **Fade out animation** starts
4. High score text fades to transparent over 1 second
5. GameObject is disabled

### Game Over:
1. **Cubie.CheckHighScore()** compares final score to high score
2. If higher, **HighScoreManager** saves new high score
3. Next time you play that mode, new high score is displayed!

---

## Testing

### Test High Score Display:

1. **Play Normal Mode**:
   - ✅ Should show "High Score: 0" (first time)
   - Jump on one tile
   - ✅ High score should fade out
   
2. **Play to score 10, then die**:
   - ✅ Check console: "New High Score for Normal: 10"
   
3. **Play Normal Mode again**:
   - ✅ Should show "High Score: 10"
   - Jump on tile
   - ✅ Fades out
   
4. **Play Musical Mode**:
   - ✅ Should show "High Score: 0" (separate from Normal!)
   - Score something in Musical Mode
   - ✅ Musical mode now has its own high score

5. **Switch between modes**:
   - Normal: High Score 10
   - Musical: High Score 5
   - Upload: High Score 8
   - ✅ Each mode remembers its own high score!

---

## Customization Options

### Change Fade Behavior:

**Fade after timer instead of player movement:**
```
Fade On Player Move: ❌ Unchecked
Display Duration: 3 (seconds)
```
High score will fade after 3 seconds automatically.

**Instant disappear (no fade):**
```
Fade Duration: 0
```

**Longer display:**
```
Display Duration: 5
Fade Duration: 2
```
Shows for 5 seconds, then fades over 2 seconds.

### Styling Ideas:

**Gold high score text:**
- Color: Gold (#FFD700)
- Font: Bold
- Glow effect: Add Shadow or Outline

**Animated entrance:**
- Add DOTween or Animation
- Scale from 0 to 1 on start
- Then fade out when player moves

**Positioning:**
- Top center: Above score
- Top right: Next to score
- Center screen: Big and prominent

---

## Advanced: Manual Trigger

If you want to control fade from code:

```csharp
// Get reference to HighScoreDisplay
HighScoreDisplay hsd = FindObjectOfType<HighScoreDisplay>();

// Trigger fade manually
hsd.TriggerFade();

// Reset and show again
hsd.ResetDisplay();
```

---

## Remove High Score from Main Menu

### Option 1: Delete UI Element
1. Open MainMenu scene
2. Find high score text/display
3. Delete it

### Option 2: Hide It
1. Find high score GameObject
2. Uncheck it in Inspector (disable)

---

## Troubleshooting

### ❌ High score shows 0 even after scoring
**Solution:**
- Check Cubie.cs has `CheckHighScore()` call in `Death()` method
- Verify HighScoreManager exists in scene (persists via DontDestroyOnLoad)
- Check game mode is set correctly in HighScoreDisplay

### ❌ Doesn't fade when player moves
**Solution:**
- Check "Fade On Player Move" is **CHECKED**
- Verify ScoreDisplay component is attached to score text
- Check ScoreDisplay has correct scoreText reference

### ❌ Fades immediately
**Solution:**
- Display Duration might be 0
- Check "Fade On Player Move" setting
- Verify player score starts at 0

### ❌ High score same for all modes
**Solution:**
- Check Game Mode setting in each scene's HighScoreDisplay
- Normal scene should be "Normal"
- Musical scene should be "Musical"
- Upload scene should be "Upload"

### ❌ Error: "HighScoreManager not found"
**Solution:**
- HighScoreManager should exist from previous setup
- It uses DontDestroyOnLoad, so it persists
- If missing, create GameObject with HighScoreManager component

---

## File Changes Summary

### Modified:
- ✅ **HighScoreManager.cs** - Now supports 3 game modes

### Created:
- ✅ **HighScoreDisplay.cs** - In-game high score display with fade
- ✅ **ScoreDisplay.cs** - Score tracking helper

---

## Quick Setup Checklist

For each game scene:

**GameScene_Normal:**
- ✅ ScoreDisplay on score text
- ✅ HighScoreText UI created
- ✅ HighScoreDisplay component added
- ✅ Game Mode = Normal
- ✅ Fade On Player Move = Checked

**GameScene_Musical:**
- ✅ ScoreDisplay on score text
- ✅ HighScoreText UI created
- ✅ HighScoreDisplay component added
- ✅ Game Mode = Musical
- ✅ Fade On Player Move = Checked

**GameScene_UploadMusic:**
- ✅ ScoreDisplay on score text
- ✅ HighScoreText UI created
- ✅ HighScoreDisplay component added
- ✅ Game Mode = Upload
- ✅ Fade On Player Move = Checked

**MainMenu:**
- ✅ Remove/hide old high score display

---

## Example Scene Hierarchy

```
GameScene_Normal
├── Canvas
│   ├── ScoreText  ← Has ScoreDisplay component
│   │   └── (TextMeshProUGUI)
│   └── HighScoreText  ← Has HighScoreDisplay + CanvasGroup
│       └── (TextMeshProUGUI showing "High Score: X")
├── Player (Cubie)
├── Camera
└── ...
```

---

**Your high scores are now mode-specific and beautifully fade away! 🏆✨**
