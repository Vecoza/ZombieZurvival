# ZombieZurvival

ZombieZurvival is a wave-based survival shooter built in Unity. You fight off zombies, manage weapon-specific ammo, and use pickups to stay alive as each wave grows harder.

---

## Getting Started
1. Install Unity `2022.3.20f1` (as listed in `ProjectSettings/ProjectVersion.txt`).
2. Open the project folder in Unity Hub: `ZombieZurvival/`.
3. Open a scene:
   - Main menu: `Assets/Prefabs/Scena/MainMenu.unity`
   - Gameplay map: `Assets/NewMapPackage/Map_v1.unity`
4. Press Play in the Unity Editor.

---

## Game Loop
- Waves spawn from `ZombieSpawnController` with a short delay between individual spawns (`spawnDelay`).
- When all zombies in a wave are dead, the game enters a cooldown (`waveCoolDown`) and then starts the next wave.
- Each new wave doubles the zombie count (default start is `initialZombiesPerWave = 5`).
- Your best wave is saved to PlayerPrefs and shown in the main menu.

---

## Weapons and Ammo
- Three weapon models are supported: `HandgunM1911`, `ThompsonM1A1` (SMG), and `RifleAK74M`.
- Each weapon has its own ammo pool managed by `WeaponManager`:
  - Handgun ammo
  - SMG ammo
  - Rifle ammo
- Reloading pulls from the matching ammo pool; if the magazine is empty and ammo exists, the weapon auto-reloads.
- Bullets inherit damage from the equipped weapon and apply it to enemies on collision.

---

## Items and Pickups
- **Ammo boxes** add to the correct ammo pool based on `AmmoBox.AmmoType`.
- **Medkits** restore health:
  - Small medkit: +20 HP
  - Big medkit: +50 HP
  - Health is capped at 100
- **Throwables** (grenades) can be picked up, charged, and thrown; they explode after a short delay and damage enemies in an area.

---

## Zombies and Combat
- Zombies use a state-driven AI: Idle → Patrol → Chase → Attack, based on player distance.
- Attacks are handled by the `ZombieHand` trigger; the player takes damage on contact.
- On death, a zombie has a chance to drop a random item from its `spawnItems` list.

---

## Controls (Game-Specific)
- Shoot: `Mouse0`
- Reload: `R`
- Interact / pick up: `E`
- Throw grenade: `G` (hold to charge, release to throw)
- Pause: `Esc`

---

## 🛠️ Technologies Used
- Unity
- C#
- Unity Animator, NavMesh, and Physics systems

---

## Project Structure
- `Assets/Scripts/WeaponScript.cs` — weapon firing, recoil, reload, ammo usage
- `Assets/Scripts/WeaponManager.cs` — weapon equip, ammo pools, throwables, medkit pickup
- `Assets/Scripts/InteractionManager.cs` — interactable detection and pickup logic
- `Assets/Scripts/ZombieSpawnController.cs` — wave spawning and cooldowns
- `Assets/Scripts/Enemy.cs` — zombie health, damage, and item drops
- `Assets/Scripts/Zombie*State.cs` — AI state machine behaviors
- `Assets/Scripts/Player.cs` — health, damage, death, and game over flow
