# WORLD ENGINE — PROJECT REFERENCE

> Living technical reference for the current state of the game.
> Update this document whenever a major system, mechanic, or architecture decision changes.

---

# 1. PROJECT OVERVIEW

## Game
2D side-scrolling action roguelike inspired by games such as:
- Risk of Rain Returns
- Dead Cells

The game takes place on Vespara and revolves around World Engines, misprint enemies, weapons, attachments, items, procedural terrain, crates, bosses/events, and increasing threat.

## Current Development Goal
Build a complete playable version with:
- Player movement/combat
- Procedural world generation
- Enemies
- Weapons
- Weapon attachments
- Stackable player items
- Crates/loot
- Bosses/events
- Threat progression
- UI
- Character abilities
- Multiple weapons/content options

## Engine
Unity

## Input
Unity NEW Input System.

Do NOT use legacy `UnityEngine.Input` for new gameplay code.

Example mouse input:

Mouse.current.position.ReadValue()

---

# 2. PROJECT WORKFLOW

## Version Control
GitHub / GitHub Desktop.

Commit major working milestones before beginning risky changes.

## Documentation
This file is the authoritative technical/current-state reference:

Assets/Documentation/PROJECT_REFERENCE.md

The GDD is used for higher-level game design.

When modifying an existing system, always check the CURRENT script before making major changes rather than relying only on documentation.

---

# 3. PLAYER

## Root Structure

General structure:

Player
├── Visual
├── GroundCheck
└── WeaponHolder

The Player root owns gameplay/physics.

Visual owns:
- SpriteRenderer
- Animator

## Physics

Player currently uses:
- Rigidbody2D
- CapsuleCollider2D

Rigidbody2D:
- Body Type: Dynamic
- Simulated: On
- Gravity Scale: 1

Player tag:
Player

## Movement

PlayerMovement includes:
- Horizontal movement
- Jumping
- Coyote time
- Jump buffering
- Ground checking

Uses Rigidbody2D.linearVelocity.

PlayerMovement exposes:
- IsGrounded
- HorizontalSpeed
- VerticalVelocity

These are used by animation.

---

# 4. PLAYER ANIMATION

Current animations:
- Idle
- Run
- Jump

Animator parameters:
- IsGrounded (Bool)
- YVelocity (Float)
- Speed (Float)

Current transitions:

Idle -> Run
Speed > 0.1

Run -> Idle
Speed < 0.1

Idle -> Jump
IsGrounded = false

Run -> Jump
IsGrounded = false

Jump -> Idle
IsGrounded = true
Speed < 0.1

Jump -> Run
IsGrounded = true
Speed > 0.1

PlayerAnimationController currently flips the player based on mouse aiming.

Future:
Potential backward-running/shooting animations when movement direction opposes aim direction.

---

# 5. PLAYER HEALTH

Current script:
PlayerHealth.cs

Current basic behavior:
- maxHealth defaults to 5
- currentHealth initialized at game start
- TakeDamage(int damage)
- Death currently freezes the game using Time.timeScale = 0

Current PlayerHealth does NOT yet have:
- Heal()
- Public CurrentHealth property
- Health percentage helpers

These will be needed for Emergency Pack.

---

# 6. WEAPON SYSTEM

Weapons use a universal Weapon.cs base.

Current weapon stats include:
- Base damage
- Base fire rate
- Base projectile speed
- Magazine size
- Reserve ammo
- Reload time
- Rarity
- Weapon family

Weapons support attachment slots.

## Projectile

WeaponProjectile.cs stores:
- Direction
- Damage
- Speed

Projectile calls:

EnemyHealth.TakeDamage(damage)

on collision with an EnemyHealth component.

## Reloading

Reload system uses:
- isReloading
- reloadEndTime
- CheckReload()

This allows reload completion to remain consistent when switching weapons.

## Weapon Slots

Player has two weapon slots.

Weapon switching:
- 1
- 2

Future:
When both slots are occupied and a new weapon is taken, the replaced weapon should physically drop while retaining its state.

---

# 7. CURRENT WEAPONS

Implemented / created:

## Assault Rifle — Rook
Behavior:
3-round burst.

## Assault Rifle — Rattlesnake
Behavior:
Full auto.
Moderate fire rate.

## SMG — Hornet
Behavior:
Very high full-auto fire rate.

## Shotgun — Breacher
Behavior:
Pump shotgun.
High damage.
Low fire rate.

## Pistol — Verdict
Behavior:
Semi-auto.
Moderate damage/fire rate.

## Sniper — Pilgrim
Behavior:
Semi-auto.
High damage.
Low fire rate.

---

# 8. WEAPON FAMILIES

Current WeaponFamily values include:

- Rifle
- SMG
- Pistol
- Shotgun
- Sniper
- Launcher
- Bow
- Crossbow
- Special

---

# 9. ATTACHMENT SYSTEM

Attachments belong to individual weapons.

They do NOT stack like player items.

Only one attachment can occupy a particular attachment slot on a weapon.

Applying another attachment to the same slot replaces the existing attachment.

Attachments may restrict compatible weapon families.

Weapon stats are recalculated from equipped attachments.

## Current Attachment Slot Types

Current/known slots include:
- Magazine
- Optic
- Stock
- Underbarrel
- AmmoType
- String
- ArrowType
- GuidanceModule
- Special

IMPORTANT:
A Barrel slot needs to be added/confirmed because barrel attachments exist.

UI may call Optic "Scope" even if the internal enum remains Optic.

## Magazine Formula

Magazine size:

Max(
    1,
    RoundToInt(baseMagazineSize * magazineSizeMultiplier)
    + magazineFlatBonus
)

---

# 10. ATTACHMENTS

## Magazine

### Extended Magazine
Rarity: Common
Effect:
Increased magazine size.

STATUS: Created

### Quickdraw Magazine
Rarity: Common
Effect:
Improved reload speed.

STATUS: Created

### Drum Magazine
Rarity: Uncommon
Effect:
Large magazine increase.

STATUS: Created

---

## Underbarrel

### Foregrip
Rarity: Common
Effects:
- Reduced spread
- Slight movement increase

STATUS: Created

### Underbarrel Grenade Launcher
Rarity: Rare
Planned:
After sustained firing, periodically fires a slow explosive projectile.

STATUS: Planned

### Underbarrel Shotgun
Rarity: Rare
Planned:
After sustained firing, periodically fires a shotgun pellet spread.

STATUS: Planned

---

## Barrel

### Muzzle
Rarity: Common
Effect:
Reduced spread.

STATUS: Created

Possible future rename:
Compensator / Muzzle Brake

### Long Barrel
Rarity: Uncommon
Effects:
- Increased projectile speed
- Tighter spread
- Reduced fire rate

STATUS: Created

### Short Barrel
Rarity: Uncommon
Effects:
- Increased fire rate
- Increased movement
- Looser spread

STATUS: Created

### Suppressor
Rarity: Rare
Planned effects:
- Large crit chance increase
- Reduced enemy detection range

STATUS: Planned

---

## Scope / Optic

### Red Dot Sight
Rarity: Common
Effect:
Slight movement increase.

STATUS: Created

### Marksman Scope
Rarity: Rare
Planned effects:
- Slower shots
- Reduced spread
- Increased damage

STATUS: Planned

### ACOG Scope
Rarity: Rare
Planned effects:
- Increased damage
- Reduced spread

STATUS: Planned

---

## Ammo Type

### Armor Penetration Rounds
Rarity: Uncommon
Effect:
Bullets penetrate enemies.

STATUS: Created

### Hollowpoint Bullets
Rarity: Uncommon
Effect:
Extra damage to first enemy hit.

STATUS: Created

### Explosive Rounds
Rarity: Epic
Planned:
Projectile explodes on enemy/wall collision.

STATUS: Planned

### Shock Rounds
Rarity: Epic
Planned:
Briefly stun enemies on hit.

STATUS: Planned

### Inferno Rounds
Rarity: Legendary
Planned:
Ignite enemies.

STATUS: Planned

---

## Grip / Stock

### Sawed-off Stock
Rarity: Uncommon
Effects:
- Increased movement
- Increased spread

STATUS: Created

### Collapsible Stock
Rarity: Uncommon
Effect:
Increased movement.

STATUS: Created

### Target Stock
Rarity: Rare
Effect:
Reduced spread.

STATUS: Planned

---

# 11. PLAYER ITEM SYSTEM

Items are PLAYER-WIDE passive upgrades.

Unlike attachments:
- Items stack.
- Items affect the player globally.
- Multiple copies increase their effects.

## ItemData

Items use ItemData ScriptableObjects.

ItemData contains:
- ItemID
- Item name
- Loot rarity
- Description

## PlayerItemInventory

Player has a PlayerItemInventory component.

Items are stored using stack counts by ItemID.

Important methods include:

GetStackCount(ItemID)

HasItem(ItemID)

GetWeaponDamageMultiplier()

---

# 12. ITEMS

## Overcharge Cell

Rarity:
EPIC

Description:
"Increases weapon damage by 25%"

STATUS:
FULLY FUNCTIONAL

Current effect:
+25% weapon damage per stack.

Current stacking:

0 stacks = 1.00x
1 stack  = 1.25x
2 stacks = 1.50x
3 stacks = 1.75x
4 stacks = 2.00x

Weapon.Damage combines:
- Base weapon damage
- Attachment damage multiplier
- Player item damage multiplier

---

## Emergency Pack

Rarity:
Common

Planned behavior:
Automatically restores health when player health falls below a threshold.

Stacking should:
- Increase healing
- Reduce cooldown
- Increase activation health threshold

STATUS:
NEXT ITEM TO IMPLEMENT

---

## Rusty Screw

Rarity:
Common

Effect:
Chance for weapon to malfunction and accidentally fire additional shots.

Stacks:
- Increase proc chance
- Increase accidental shots

STATUS:
Planned

---

## Heat Sink

Rarity:
Common

Effect:
Firing builds heat.

Higher heat:
- Increases fire rate
- Worsens spread

Stacks:
- Build heat faster
- Increase fire-rate benefit
- Modify spread penalty

STATUS:
Planned

---

## Cracked Capacitor

Rarity:
Uncommon

Effect:
Dealing damage builds an electrical field around the player.

Stacks:
- Builds faster
- Larger field

STATUS:
Planned

---

## Blackbox Recorder

Rarity:
Rare

Effect:
Damage dealt stores additional delayed damage on an enemy.

Stored damage releases when:
- Stored damage exceeds remaining enemy HP
OR
- Enemy has not been hit for a period of time

Stacks increase stored damage percentage.

STATUS:
Planned

---

## Gravity Nail

Rarity:
Rare

Effect:
Critical hits can embed a nail.

After enough nails/hits:
- Nail shatters
- Pulls nearby enemies
- Implodes for heavy damage

Stacks:
- Increase proc chance
- Increase damage
- Increase pull range

STATUS:
Planned

---

## Light-in-a-Bottle

Effect:
Kills charge the bottle.

When full, automatically grants a temporary blessing:
- Healing
- Movement speed
- Fire rate

STATUS:
Planned

Rarity:
To be finalized.

---

## De-atomizing Ray

Rarity:
Legendary

On-hit chance to apply a powerful laser/debuff.

Effects may include:
- Slow
- Increased vulnerability
- Reduced enemy damage
- Damage over time

Stacks strengthen/increase duration/proc chance.

STATUS:
Planned

---

## Reactive Barrier

Rarity:
Uncommon

Effect:
After taking damage, reduces the next incoming hit by a percentage.

Stacks increase damage reduction.

STATUS:
Planned

---

# 13. DAMAGE NUMBER SYSTEM

STATUS:
FUNCTIONAL

EnemyHealth spawns damage numbers when actual damage is received.

Architecture:

Weapon
-> WeaponProjectile
-> EnemyHealth.TakeDamage()
-> DamageNumberSpawner
-> DamageNumber prefab

DamageNumber behavior:
- Displays actual rounded damage
- Slight randomized horizontal movement
- Floats upward
- Scales over lifetime
- Fades out
- Destroys itself

Current placeholder enemies require approximately:

Damage Number Offset Y = -2

This is due to their current transform/visual positioning.

Offsets should eventually be tuned per enemy.

Future:
Critical hits can use:
- Larger text
- Different visual treatment
- Stronger pop animation

---

# 14. ENEMY HEALTH

EnemyHealth currently:
- Stores max health
- Stores current health
- Receives float weapon damage
- Rounds actual damage to an integer
- Spawns damage number
- Dies at <= 0 HP

Current placeholder max health may be very low for testing.

---

# 15. LOOT / PICKUPS

Current pickups use IInteractable.

ItemPickup:
- Holds ItemData
- Displays interaction text
- Highlights when selected
- Adds item to PlayerItemInventory
- Destroys pickup after collection

Typical item pickup hierarchy:

ItemPickup
├── Visual
└── GroundCheck

Common components:
- ItemPickup
- LootFloat
- GroundSnap
- Trigger BoxCollider2D

Visual:
- SpriteRenderer
- Loot sorting layer

GroundCheck:
Used by GroundSnap.

Future:
Replace yellow-tint highlighting with an outline effect.

---

# 16. CRATES

Crate artwork already exists.

Sprites available:
- Closed/unopened crate
- Opened crate

Crate system has NOT been implemented yet.

Planned basic flow:

Player interacts with crate
-> crate opens
-> sprite/animation changes
-> loot is rolled
-> physical loot spawns near/above crate

Crates should handle loot selection rather than WorldGenerator selecting individual items directly.

Future crate variants may use different loot tables/rarity behavior.

---

# 17. LOOT CATEGORIES

Procedural loot/content needs to account for THREE major categories:

1. ITEMS
   Player-wide stacking passive effects.

2. WEAPON ATTACHMENTS
   Individual weapon modifications occupying attachment slots.

3. WEAPONS
   Actual equippable guns/weapons.

Do not design procedural loot spawning as though only items exist.

---

# 18. LOOT UI

Existing loot UI includes:
- Bottom-center interaction prompt
- Selected pickup highlighting
- Top-right information panel

ItemInfoUI supports:
- Weapons
- Items
- Attachments
- Rarity colors
- Weapon comparisons

Future:
Attachment visual indicator UI.

---

# 19. PIXEL ART / IMPORT SETTINGS

General pixel-art scale:
32 PPU.

Player is approximately 32 pixels tall.

Typical item/attachment/power-up sprite canvas:
32x32 transparent.

Recommended import:
- Texture Type: Sprite (2D and UI)
- Pixels Per Unit: 32
- Filter Mode: Point
- Compression: None
- Mip Maps: Off

---

# 20. CAMERA / PIXEL PERFECT

Pixel Perfect Camera is being used/tested.

Known issue:
Odd Game view resolutions such as 731x411 can produce undesirable pixel behavior.

Possible clean testing resolution:
1280x720.

Possible reference resolution:
640x360.

Further camera/pixel tuning may be required later.

---

# 21. PROCEDURAL WORLD GENERATION

STATUS:
FIRST TERRAIN PASS FUNCTIONAL.
CURRENTLY PAUSED WHILE CONTENT/LOOT SYSTEMS ARE BUILT.

IMPORTANT ARCHITECTURE:

The world is NOT primarily assembled from handcrafted room/chunk prefabs.

Normal terrain is generated algorithmically.

Chunks are intended as technical/invisible processing sections.

Special structures may be handcrafted.

Conceptually:

Random procedural terrain
+
Predetermined/special structures

similar to how a Minecraft-like world combines generated terrain with structures.

---

# 22. TERRAIN TILEMAP

Current hierarchy:

WorldGrid
└── GroundTilemap

WorldGrid:
- Grid component
- Position 0,0,0
- Scale 1
- Cell size 1,1,0

GroundTilemap:
- Tilemap
- Tilemap Renderer
- Tilemap Collider 2D
- Ground layer

With 32 PPU:

1 tile = 1 Unity unit.

Player collision with generated terrain is confirmed working.

---

# 23. TERRAIN TILESET

Current terrain palette contains 13 tiles.

Original outer/standard tiles:
- Top Left
- Top
- Top Right
- Left
- Center
- Right
- Bottom Left
- Bottom
- Bottom Right

Inner corner tiles:
- Inner Top Left
- Inner Top Right
- Inner Bottom Left
- Inner Bottom Right

Automatic neighbor-based tile selection is functional.

Inner/outer corners visually work.

---

# 24. WORLD GENERATOR

Current WorldGenerator uses a solidTerrain bool[,] representation.

Important current settings include approximately:

worldWidth = 256
groundDepth = 12

startingGroundHeight = 8
minimumGroundHeight = -12
maximumGroundHeight = 14

minFeatureWidth = 20
maxFeatureWidth = 45

maxFeatureHeightChange = 2

downwardChance = 0.60

combatAreaChance = 0.25

minCombatAreaWidth = 20
maxCombatAreaWidth = 35

useRandomSeed = true
seed = 12345

Terrain features use gradual interpolation rather than changing height every tile.

There is a mild overall downward bias.

Flat combat areas can be generated.

Seeded/random generation exists.

---

# 25. PROCEDURAL GENERATION — FUTURE

Planned additions:

- Technical chunk processing
- Enemy spawn locations
- Crate spawn locations
- More traversal rules
- Gaps
- Platforms
- Caves
- Special structures
- Crash Site
- Boss approach
- Large boss-event hole
- Boss arena
- Future structures/events

Tentative technical chunk size:

64 tiles wide
32 tiles high

Expose chunk dimensions in Inspector rather than hardcoding them.

Current world can initially generate entirely at scene start.

Streaming/unloading can be considered later.

---

# 26. SLOPES

Slope terrain is intentionally POSTPONED.

Future goal:
Use slanted terrain sprites/colliders for smoother terrain similar to Terraria / Risk of Rain Returns.

Possible slope types:
- 1:1
- Gentle 2-tile slopes

DO NOT prioritize this until returning to terrain work.

---

# 27. BACKGROUND / PARALLAX

Current project has one main backdrop.

ParallaxBackground exists.

Horizontal movement uses standard parallax.

Vertical movement follows most camera movement to prevent strange separation during elevation changes.

Approximate far-background values previously tested:
- Horizontal Parallax: 0.08
- Vertical Parallax: 0.03

Future:
- Additional layers
- Atmospheric fog
- Repeating/infinite background if world edge becomes visible

---

# 28. CURRENT DEVELOPMENT ORDER

Current planned development sequence:

1. Finish items and their mechanics
2. Finish/add weapon attachments
3. Finish/add weapons
4. Build crate system
5. Create loot tables / loot rolling
6. Integrate crates into procedural generation
7. Integrate enemies into procedural generation
8. Continue procedural world features
9. Add special structures/events
10. Continue bosses/threat progression/etc.

This order can change if testing requires it.

---

# 29. IMMEDIATE NEXT TASK

Implement:

EMERGENCY PACK

Required first:
Upgrade PlayerHealth with:
- CurrentHealth access
- Heal()
- Safe health clamping
- Health percentage access

Then implement Emergency Pack:
- Detect health threshold
- Automatically heal
- Cooldown
- Stack scaling

---

# 30. IMPORTANT DEVELOPMENT RULES

- Prefer simple working systems before overengineering.
- Keep content systems modular.
- WorldGenerator should NOT contain individual item behavior.
- Crates should handle loot rolling.
- Items affect player globally.
- Attachments affect individual weapons.
- Weapons remain separate loot/equipment.
- Inspector settings should be exposed when designers may need to tune them.
- Use the NEW Input System.
- Keep player physics/gameplay on the Player root and visuals on Visual child.
- Check current scripts before replacing important existing systems.
- Test each new mechanic before moving to the next.
- Commit working milestones before major/risky changes.

---

# 31. CURRENT CHECKPOINT

WORKING:
- Player movement
- Jumping
- Player animation
- Mouse-facing player visuals
- Weapon system
- Multiple weapons
- Ammo/reloading
- Weapon switching
- Attachment foundation
- Item pickup foundation
- Player item stacking
- Overcharge Cell
- Damage numbers
- Enemy health/damage
- Procedural terrain
- Terrain collisions
- Terrain autotiling
- Inner/outer terrain corners
- Random terrain features
- Downward terrain bias
- Flat combat areas
- Seed system
- Background parallax

CURRENTLY WORKING ON:
- Player items

NEXT:
- Emergency Pack
- Remaining items
- Remaining attachments/weapons
- Crates
- Procedural loot/enemy integration

## CURRENT DEBUGGING CHECKPOINT

Emergency Pack implementation has been started but is NOT yet confirmed working.

Basic health UI (100 / 100 text) has been created but is NOT currently updating/displaying correctly.

PlayerHealth was discovered to have disappeared from the Player GameObject during testing and was re-added.

NEXT SESSION:
1. Verify Player components and references.
2. Fix health UI.
3. Verify PlayerItemInventory is attached.
4. Debug Emergency Pack activation.
5. Confirm Emergency Pack stacking.