# Musical Mode - Quick Start Guide

## 🎵 What You Just Got

Your CubeJumper now has a **Musical Mode** that:
- ✅ Generates tiles synchronized with music
- ✅ Uses piano key theme (black & white tiles)
- ✅ High-intensity music → Big tiles (high jumps)
- ✅ Low-intensity music → Small tiles (short jumps)
- ✅ Visual effects that pulse with the music
- ✅ Glow effects when landing on tiles

## 📁 New Files Created

1. **MusicAnalyzer.cs** - Analyzes music in real-time
2. **MusicalGenerator.cs** - Generates tiles based on music
3. **GameModeManager.cs** - Switches between Normal/Musical modes
4. **PianoTileVisuals.cs** - Visual effects for piano-themed tiles
5. **MusicDebugVisualizer.cs** - Debug tool to tune settings
6. **AudioManager.cs** - Enhanced with musical mode support

## ⚡ Quick Setup (5 Minutes)

### 1. Create Musical Mode Manager
```
1. Create Empty GameObject → Name it "MusicalModeManager"
2. Add Component → MusicAnalyzer
3. Add Component → GameModeManager
4. Add Component → MusicDebugVisualizer (optional, for testing)
```

### 2. Setup MusicAnalyzer
```
- Music Source: Create new AudioSource on same object
- In the AudioSource: Add your music track
- Sample Size: 1024
- Intensity Multiplier: 3.0
- Beat Threshold: 0.8
```

### 3. Duplicate Your Tile Generator
```
1. Find "TilesGenerator" in Hierarchy
2. Duplicate it (Ctrl/Cmd + D)
3. Rename to "MusicalTilesGenerator"
4. Remove "Generator" component
5. Add "MusicalGenerator" component
6. Assign Tile Prefab
7. Drag MusicalModeManager to "Music Analyzer" field
```

### 4. Configure GameModeManager
```
On MusicalModeManager GameObject:
- Normal Generator: Drag "TilesGenerator"
- Musical Generator: Drag "MusicalTilesGenerator"
- Music Analyzer: Drag itself
- Musical Mode Audio Source: The AudioSource you created
- Musical Mode Track: Your music clip
```

### 5. Add Visuals to Tile Prefab
```
1. Open your Tile Prefab
2. Add Component → PianoTileVisuals
3. Set: Pulse With Music = true
4. Set: Glow On Step = true
5. Save Prefab
```

## 🎮 How to Use

### Switch to Musical Mode:
**Option A - Via Code:**
```csharp
GameModeManager.Instance.SetGameMode(GameModeManager.GameMode.Musical);
```

**Option B - Via UI Toggle:**
1. Create UI Toggle in your menu
2. Assign to GameModeManager → Musical Mode Toggle
3. Player can toggle in-game

**Option C - Auto-Start:**
Set `Current Mode = Musical` in GameModeManager Inspector

## 🎯 Testing

1. **Start the game**
2. **Enable Musical Mode**
3. **Watch the tiles sync with music!**

### Using Debug Visualizer:
- Assign UI Text elements to MusicDebugVisualizer
- See real-time intensity, beats, and tile recommendations
- Helps tune the sensitivity settings

## 🔧 Tuning the Experience

### Too many big tiles?
```
Decrease "Beat Threshold" in MusicAnalyzer (try 0.5)
```

### Too many small tiles?
```
Increase "Intensity Multiplier" in MusicAnalyzer (try 5.0)
Increase "High Jump Bias" in MusicalGenerator (try 0.2)
```

### Tiles not matching music well?
```
Increase "Music Responsiveness" in MusicalGenerator (try 0.9)
```

### Want more randomness?
```
Decrease "Music Responsiveness" in MusicalGenerator (try 0.4)
```

## 🎨 Making It Look Amazing

### Create Piano Key Materials:
1. **White Key Material:**
   - Color: White (255, 255, 255)
   - Optional: Enable Emission with white glow

2. **Black Key Material:**
   - Color: Black (0, 0, 0)
   - Optional: Enable Emission with cyan glow

3. **Assign to MusicalGenerator:**
   - White Piano Key Material → White material
   - Black Piano Key Material → Black material

### Visual Polish:
- Tiles pulse slightly with music intensity
- Glow yellow (white keys) or cyan (black keys) when stepped on
- Emission makes them look magical ✨

## 🎵 Best Music Choices

**Works Great:**
- Electronic/Dance music (clear beats)
- Rock/Metal (strong drums)
- Hip-Hop (prominent bass)
- Soundtrack/Epic music (varying intensity)

**Avoid:**
- Very quiet ambient music
- Classical music without percussion
- Constant-volume music
- Songs without clear rhythm

## 🐛 Common Issues

**Tiles not syncing?**
→ Check MusicAnalyzer has correct AudioSource assigned
→ Verify music is actually playing

**All tiles same type?**
→ Adjust Beat Threshold (too high/low)
→ Change Intensity Multiplier

**No visual effects?**
→ PianoTileVisuals must be on Tile Prefab
→ Check Glow On Step is enabled

**Wrong generator running?**
→ Only ONE generator should be enabled at a time
→ GameModeManager handles this automatically

## 📊 Understanding the System

```
Music Playing
    ↓
MusicAnalyzer (analyzes in real-time)
    ↓
Calculates: Intensity, Beats, Frequencies
    ↓
MusicalGenerator (asks "what tile next?")
    ↓
High Intensity/Beat → Big Tile (Black Key)
Low Intensity → Small Tile (White Key)
    ↓
Tile Spawns with PianoTileVisuals
    ↓
Player Lands → Glow Effect + Piano Sound
```

## 🚀 Next Steps

1. ✅ **Set up basic musical mode** (follow Quick Setup above)
2. 🎵 **Add your favorite song**
3. 🎮 **Test and tune settings**
4. 🎨 **Create beautiful piano key materials**
5. 🎯 **Add UI toggle for mode switching**
6. 🔧 **Use debug visualizer to perfect it**

## 💡 Pro Tips

1. **Start with default settings** - they work well for most music
2. **Use debug visualizer** - helps you see what the analyzer detects
3. **Test with different songs** - each may need slight tuning
4. **Adjust per song** - create presets for different music styles
5. **Balance gameplay** - too hard = frustrating, too easy = boring

## 🎉 You're Ready!

The musical mode is now ready to use. Start with the Quick Setup above, then customize to your heart's content. Have fun! 🎵🎮

---

Need help? Check the full **MUSICAL_MODE_SETUP.md** for detailed explanations and advanced features.
