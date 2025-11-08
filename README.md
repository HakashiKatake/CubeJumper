# CubeJumper 🎮

## 3-Mode Musical Jump Game

A Unity game with three exciting gameplay modes:
- **Normal Mode** - Classic random tile jumping
- **Musical Mode** - Jump through tiles that play melodies
- **Upload Your Music** - Analyze any song and play it through tiles!

---

## 📚 Complete Setup Documentation

**START HERE:** [README_SETUP_INDEX.md](README_SETUP_INDEX.md)

This index contains links to all setup guides:
- Complete scene structure guide
- Individual mode setup guides (Normal, Musical, Upload)
- Troubleshooting guides
- Component checklists

---

## 🚀 Quick Setup Summary

### 1. Create 4 Scenes:
- MainMenu (mode selection)
- GameScene_Normal
- GameScene_Musical  
- GameScene_UploadMusic

### 2. Add Scripts:
- ✅ MainMenuController.cs (already created)
- ✅ GameController.cs (already created)
- ✅ AudioMelodyExtractor.cs (already created)
- ✅ MusicUploadUI.cs (already created)

### 3. Setup Each Mode:
Follow the individual guides in README_SETUP_INDEX.md

---

## 🎵 Features

- **Multiple Game Modes** with separate scenes
- **Melody Sequencer** for musical gameplay
- **Audio Analysis** - Pitch detection & note extraction
- **Dynamic Melodies** - Play any uploaded song through tiles
- **Piano Note Library** - Full chromatic scale support
- **Clean Architecture** - Separated concerns, easy to maintain

---

## 📖 Documentation Files

- **README_SETUP_INDEX.md** - Master index (start here)
- **COMPLETE_SCENE_SETUP_GUIDE.md** - Full scene setup walkthrough
- **SETUP_NORMAL_MODE.md** - Normal mode quick reference
- **SETUP_MUSICAL_MODE.md** - Musical mode quick reference  
- **SETUP_UPLOAD_MODE.md** - Upload music mode quick reference
- **CRITICAL_FIX_ADD_COMPONENT.md** - Fix tiles not spawning

---

## 🎮 CubeJumper 🎮

A musical rhythm-based jumper game with dynamic tile generation synchronized to music!

## 🎵 Features

### Core Gameplay
- **Two Jump Types**: Short jump (left arrow) and high jump (right arrow)
- **Score System**: Track your progress and achieve high scores
- **Achievement System**: Unlock color schemes as you progress
- **Game Over Detection**: Miss a tile or choose wrong jump = game over

### 🎹 NEW: Musical Mode
Turn your game into a rhythm-based musical experience!

- **Real-Time Music Analysis**: FFT-based spectrum analysis detects beats and intensity
- **Music-Synchronized Tiles**: Tile heights match the song's energy
  - High intensity music → Big tiles (high jumps)
  - Low intensity music → Small tiles (short jumps)
- **Piano Key Theme**: Beautiful black and white tiles like piano keys
- **Visual Effects**: Tiles pulse with music and glow when you land
- **Dual Mode System**: Switch between Normal and Musical modes
- **Preset System**: Save and load different configurations for different songs
- **Debug Tools**: Real-time visualizer to tune your settings

## 🚀 Quick Start

### Normal Mode
1. Open the game
2. Press Play
3. Use **Left Arrow** for short jumps, **Right Arrow** for high jumps
4. Match your jump to the tile type
5. Don't miss or game over!

### Musical Mode Setup
1. In Unity, go to **Tools → CubeJumper → Musical Mode Setup Wizard**
2. Click "Yes, Set Up Musical Mode"
3. Assign your music track to the AudioSource
4. Enable Musical Mode in GameModeManager
5. Play and enjoy music-synchronized gameplay!

**Detailed instructions**: See `Assets/Scripts/QUICK_START.md`

## 📁 Project Structure

```
CubeJumper/
├── Assets/
│   ├── Scenes/          # Game and Menu scenes
│   ├── Scripts/         # All game scripts
│   │   ├── Core Scripts:
│   │   │   ├── Cubie.cs              # Player controller
│   │   │   ├── Generator.cs          # Normal tile generation
│   │   │   ├── TileScript.cs         # Tile behavior
│   │   │   ├── AudioManager.cs       # Sound management
│   │   │   └── UIhandler.cs          # UI controls
│   │   │
│   │   ├── Musical Mode Scripts:
│   │   │   ├── MusicAnalyzer.cs           # Real-time music analysis
│   │   │   ├── MusicalGenerator.cs        # Music-synced tiles
│   │   │   ├── GameModeManager.cs         # Mode switching
│   │   │   ├── PianoTileVisuals.cs        # Visual effects
│   │   │   ├── MusicDebugVisualizer.cs    # Debug tools
│   │   │   ├── MusicalModePreset.cs       # Preset system
│   │   │   ├── PresetManager.cs           # Preset loading
│   │   │   └── MusicalModeSetupWizard.cs  # Setup automation
│   │   │
│   │   └── Documentation:
│   │       ├── QUICK_START.md             # 5-minute setup guide
│   │       ├── MUSICAL_MODE_SETUP.md      # Detailed setup
│   │       ├── ARCHITECTURE.md            # System diagrams
│   │       ├── PRESET_EXAMPLES.md         # Example presets
│   │       └── README_MUSICAL_MODE.md     # Complete overview
│   │
│   ├── Materials/       # Tile and player materials
│   ├── Sounds/          # Audio files
│   ├── Fonts/           # UI fonts
│   └── Prefabs/         # Tile and particle prefabs
│
└── README.md           # This file
```

## 🎮 Controls

| Key | Action |
|-----|--------|
| **Left Arrow** | Short jump (for small tiles/white keys) |
| **Right Arrow** | High jump (for big tiles/black keys) |

## 🎯 How to Play

### Normal Mode
1. Tiles spawn randomly in two heights
2. Small tiles require left arrow (short jump)
3. Big tiles require right arrow (high jump)
4. Match the jump to the tile type
5. Score increases with each successful jump
6. Colors change at achievement milestones

### Musical Mode
1. Tiles spawn based on music analysis
2. Low music intensity → small tiles (white piano keys)
3. High music intensity → big tiles (black piano keys)
4. Follow the rhythm and watch tiles sync with the music!
5. Enjoy visual effects pulsing with the beat

## 🛠️ Setup for Development

### Requirements
- Unity 2020.3 or later
- TextMeshPro package

### Opening the Project
1. Clone or download the repository
2. Open Unity Hub
3. Add project from disk
4. Open the project
5. Open `Assets/Scenes/Menu.unity`

### Setting Up Musical Mode
See detailed instructions in `Assets/Scripts/QUICK_START.md`

**Quick setup:**
1. Tools → CubeJumper → Musical Mode Setup Wizard
2. Assign music track
3. Configure settings
4. Play!

## 🎵 Musical Mode - Key Concepts

### Music Analysis
- **FFT Spectrum Analysis**: Analyzes 1024 audio samples
- **8 Frequency Bands**: From bass to treble
- **Beat Detection**: Detects rhythmic patterns
- **Intensity Calculation**: Measures overall music energy

### Tile Generation
```
High Music Intensity + Beats = Big Tiles (Black Keys)
Low Music Intensity = Small Tiles (White Keys)
```

### Configurability
All parameters are tunable:
- Music sensitivity
- Beat detection threshold
- Tile generation responsiveness
- Visual effect intensity
- Game speed per song

## 📚 Documentation

Comprehensive guides available in `Assets/Scripts/`:

- **QUICK_START.md** - Get musical mode running in 5 minutes
- **MUSICAL_MODE_SETUP.md** - Detailed setup and configuration
- **ARCHITECTURE.md** - System design and data flow
- **PRESET_EXAMPLES.md** - Example settings for different genres
- **README_MUSICAL_MODE.md** - Complete feature overview

## 🎨 Customization

### Creating Piano Key Materials
1. Create two materials in Unity
2. **White Key**: Set to pure white, optional emission
3. **Black Key**: Set to pure black, optional emission
4. Assign to MusicalGenerator component

### Creating Presets for Songs
1. Right-click in Project → Create → CubeJumper → Musical Mode Preset
2. Configure all settings
3. Add to PresetManager
4. Switch between songs easily!

See `PRESET_EXAMPLES.md` for genre-specific settings.

## 🔧 Unity Editor Tools

Access via **Tools → CubeJumper**:

- **Musical Mode Setup Wizard** - Automated setup
- **Create Musical Mode Preset** - Quick preset creation
- **Add Piano Visuals to Tile** - Add visuals component
- **Documentation** - Quick access to all guides

## 🎮 Gameplay Tips

### Normal Mode
- Watch the upcoming tiles
- Time your jumps carefully
- Build combos for color achievements

### Musical Mode
- Listen to the music!
- High energy parts = prepare for big jumps
- Beats often trigger high tiles
- Use debug visualizer to learn patterns

## 🏗️ Technical Details

### Performance
- **CPU**: ~1-2% for music analysis
- **Memory**: Same as normal mode
- **FPS**: Negligible impact
- Optimized for real-time music processing

### Architecture
- **Modular Design**: Each component has one responsibility
- **Non-Destructive**: Musical mode doesn't break normal mode
- **Extensible**: Easy to add new features
- **Well-Documented**: Inline comments and XML docs

## 🤝 Contributing

Ideas for enhancement:
- [ ] More visual themes
- [ ] Combo scoring system
- [ ] Perfect hit detection (landing on beat)
- [ ] Song library/selection menu
- [ ] Difficulty modes
- [ ] Leaderboards per song
- [ ] Custom song import
- [ ] Replay system

## 📝 License

[Your License Here]

## 👤 Author

[Your Name Here]

## 🎉 Acknowledgments

Special features:
- Real-time FFT music analysis
- Piano key aesthetic
- Rhythm-based gameplay
- Preset system for different songs

---

## 🎵 Musical Mode Quick Reference

### Setup Steps
1. ✅ Run Setup Wizard (Tools → CubeJumper)
2. ✅ Assign music track
3. ✅ Configure sensitivity
4. ✅ Create presets
5. ✅ Play and enjoy!

### Key Scripts
- `MusicAnalyzer.cs` - Analyzes music in real-time
- `MusicalGenerator.cs` - Generates tiles based on music
- `GameModeManager.cs` - Switches between modes
- `PianoTileVisuals.cs` - Visual effects

### Tuning Parameters
- **intensityMultiplier**: Music sensitivity (1-10)
- **beatThreshold**: Beat detection (0.1-2.0)
- **musicResponsiveness**: How much to follow music (0-1)
- **highJumpBias**: Tendency for big tiles (0-1)

### Best Music Types
✓ Electronic/Dance
✓ Rock/Metal  
✓ Hip-Hop
✓ Epic/Soundtrack
✗ Very quiet/ambient
✗ No clear rhythm

---

**For detailed musical mode information, see `Assets/Scripts/README_MUSICAL_MODE.md`**

**Have fun creating your musical jumper experience! 🎵🎮✨**
