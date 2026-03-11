# Quang Chó

This repository contains the gameplay and UI scripts for a Unity block puzzle game in the style of **Jewel Sliding / Brick classic**:

- Slide a block horizontally.
- Let it fall.
- Complete full rows to clear them.
- Chain clears for combo and higher score.

## Game Overview

Core flow:

1. Spawn playable blocks.
2. Player drags a block left/right.
3. Block drops and settles.
4. System checks line clears and combo chains.
5. Update score/level, continue until board is full.
6. Optional continue flow after watching ads.

Main orchestration logic is in:

- `Gameplay/Managers/GameplayManager.cs`

## Tech Stack

- **Engine:** Unity
- **Language:** C#
- **UI:** TextMeshPro
- **Tween/Animation:** LeanTween
- **Monetization:** Google AdMob integration
- **Services:** Leaderboard-related integration

## Repository Structure

- `Gameplay/`
  - Game loop managers (`GameplayManager`, hint/suggestion managers)
  - Board and block GUI
  - Block model/factory/solver helpers
- `UI/`
  - Score, combo popups, level/exp UI
  - Game popups (pause, continue, game over, rank)
- `Scenes/`
  - Scene scripts for home/game scenes
- `Events/`
  - Event type definitions used across systems
- `Others/`
  - AdMob and leaderboard/service-related scripts

## How to Run

This repo appears to be a **script-focused snapshot** of a Unity project.

To run in Unity:

1. Open the project (or import these scripts into an existing Unity project).
2. Ensure required packages/assets are present (e.g. TextMeshPro, LeanTween, scene assets/prefabs).
3. Open the game scene and press Play in Unity Editor.

> Note: full Unity project metadata/config files may be incomplete in this repository.

## Current Status

- No automated tests are included in this repository.
- No additional build pipeline documentation is currently provided.

## Reference Inspiration

The gameplay style matches games like:

- Jewel Sliding / brick-style row-clear puzzle games

