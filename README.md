
# 🧠 Unity AI Puzzle Solver – "Thunder and Boltz"

This project is a Unity-based AI puzzle solver inspired by games like *Thunder and Boltz*, where the goal is to sort colored nuts onto bolts. The player (or AI) must avoid stacking different colors on top of each other. The AI uses a custom A* pathfinding algorithm enhanced with a heuristic system to solve complex puzzle states.

## 🎮 Features

- 🧩 Puzzle logic with movable, colored nut pieces
- ⚙️ Heuristic-based AI pathfinding (A* style)
- 🧠 Memory-efficient design for large state space search
- 🪛 Intelligent move scoring and pruning to avoid wasteful actions
- 📊 Debugging + step-by-step path replay
- 🔳 UI feedback during AI solving
- 🖥️ Fullscreen support for builds

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/Unity-AI-Puzzle-Solver.git
```

### 2. Open in Unity
- Unity Version: `2022.3.x` (or later)
- Open the project folder from Unity Hub or Unity Editor.

### 3. Run the Scene
- Load the `MainScene.unity` file (or whatever your scene name is).
- Press `R` to begin the AI solving process.
- Use `→` / `←` to step through the solution.
- Press `Backspace` to exit playback.

### 4. Build
- Go to **File → Build Settings**, select your platform, and click **Build**.
- The game will launch in fullscreen by default.

## 🧠 How the AI Works

The AI simulates all possible moves using a **priority queue** and a **custom heuristic** that evaluates each game state based on:
- Number of misplaced colors
- Partially sorted bolts
- Penalty for wasteful or reversed moves
- Encouragement for full and sorted bolts

### Example Heuristic:
- Sorted + full bolt → bonus
- Moving sorted nuts to empty → penalty
- Mixed colors → penalty

This allows the AI to solve puzzles with millions of states in seconds.

## 📁 Project Structure

```
Assets/
│
├── Scripts/
│   ├── AiSolver.cs          # Main AI logic + coroutine solver
│   ├── Heuristic.cs         # Heuristic evaluation functions
│   ├── GameState.cs         # Manages bolt/nut states & transitions
│   └── GameNode.cs          # Represents a state node with g/h/f scores
│
├── Prefabs/
├── Scenes/
│   └── MainScene.unity
└── UI/
    └── Canvas + Text Handler
```

## 💻 Controls

| Key | Function |
|-----|----------|
| R   | Start AI solving |
| →   | Next move |
| ←   | Previous move |
| Backspace | Exit playback mode |


## ✅ To Do / Ideas
- Add difficulty levels or custom level editor
- Support mobile/touch UI
- Add "instant solve" visualization
- Visual trail of AI decisions (heatmap-style?)

## 📜 License

This project is open-source under the [MIT License](LICENSE).

---

> Made with ❤️ by Amirparsa Aminian  
> AI logic, pathfinding, and heuristic design by Amirparsa Aminian
