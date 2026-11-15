![version](https://img.shields.io/badge/version-1.0.0-blue) ![coverage](https://img.shields.io/badge/coverage-100%25-brightgreen)

# Carry More Weight

Carry More Weight is a mod for the Steam game *The Last Plague: Blight*. In the game, a few Iron Ores in your backpack or a few Firewood in your hands make you Encumbered: you use Stamina faster and walk slower, so you walk the same route again and again to move ore and wood. A single log in your hands also halves your speed, with no debuff tile on screen. With this mod, heavy items weigh half as much, so you carry twice as much before the debuff starts, and a log in your hands no longer halves your speed. You set the factor, and `0` turns the debuff off.

## Features

- **Lighter heavy items.** Each heavy item weighs half as much (`WeightDivisor` 2): ores, ingots, bricks, roof tiles, jugs, planks, Firewood, logs, lumber, anvils, wheels, and shields. Most items, such as food, tools, and clothes, weigh nothing in the game, and they stay at nothing.
- **Encumbered later.** You get Encumbered when your carried weight is over 40, as in the game: the items in your hands, your backpack, and the bags in it. The penalty for each unit over 40 is the same as in the game, and the tooltip of the Encumbered debuff shows it.
- **No debuff at all.** Set `WeightDivisor` to `0`, and no item weighs anything, so you never get Encumbered.
- **No half speed with a log in your hands.** In the game, you run at half speed while you hold a Log, a Thick Log, a Thin Trunk, Lumber, Long Lumber, Firewood, a Large Sandstone Chunk, an anvil, Bellows, a Grinding Wheel, a Hand Cart Wheel, an Uprooted Blueberry Bush, or a full Clay Jug, at any weight and with no debuff tile. The mod removes this slowdown, so only Encumbered slows you. Set `HandSlowdown` to `true` to keep it.
- **Heavy labels follow.** The "Heavy" and "Very Heavy" lines of an item tooltip use the new weight. At the default, a Long Plank shows no line, and a Simple Wooden Shield shows "Heavy" in place of "Very Heavy".
- **Fair play.** `1` gives the game weights. The limit of 40 and the penalty stay as in the game. NPCs use the same item weights and lose the same slowdown, so they also carry more and keep their speed with a log. Long Lumber that you drag on the ground stays outside your inventory, so it adds no weight in the game or with the mod.

## How much you carry before Encumbered

The most of one item that you can carry with no debuff, with nothing else heavy on you. Your backpack space still limits how much you carry.

| Item | Game (x1) | x2 (default) | x5 | x10 |
|---|---|---|---|---|
| Iron Ore | 10 | 20 | 50 | 100 |
| Copper Ingot | 6 | 13 | 33 | 66 |
| Firewood | 2 | 4 | 10 | 20 |

One Thick Log in your hands makes you Encumbered in the game (Stamina depletes 180% faster, movement 36% slower). At x2 the penalty is small (30% and 6%), and from x3 there is no debuff.

## Item weights

The items with a weight in the game. Each other item weighs nothing.

| Weight in the game | Items |
|---|---|
| 1 | Mild Steel Chunk, Wrought Iron Chunk |
| 2 to 3 | Clay Roof Tile, Soft Clay Roof Tile, Short Plank, Clay Brick, Limestone Chunk, Slag |
| 4 to 8 | Iron Ore, Large Stone, Long Plank, Copper Ingot, Bronze Ingot, Clay Jug, Soft Clay Jug |
| 10 to 12 | Simple Wooden Shield, Reinforced Wooden Shield |
| 20 | Firewood, Hand Cart Wheel, Wheelbarrow Wheel |
| 30 to 50 | Bellows, Bronze Anvil, Steel Anvil, Grinding Wheel, Log, Large Sandstone Chunk |
| 100 | Thick Log, Thin Trunk, Lumber, Long Lumber |

## Save safety

The mod writes nothing to the save. The item weights come from the game data at each start. You can remove the mod at any time, and your save loads as before, with the game weights and the half speed with a log in your hands.

## Multiplayer

Made and tested for single player. Co-op is not tested.

## Requirements

- The Last Plague: Blight 0.12.7 or later.
- The [BepInEx Pack for The Last Plague: Blight](https://www.nexusmods.com/thelastplagueblight/mods/9), the BepInEx 6 (IL2CPP) build for the game.

## Install

Download the zip from [Nexus Mods](https://www.nexusmods.com/thelastplagueblight/mods/15) or [GitHub Releases](https://github.com/ivmakk-tlpb/tlpb-carry-more-weight/releases).

1. Install the [BepInEx Pack for The Last Plague: Blight](https://www.nexusmods.com/thelastplagueblight/mods/9). If no other mods were installed before, start the game once so BepInEx finishes setup, then quit.
2. Extract this mod's zip into the game folder (the folder with the game .exe). The DLL lands in `BepInEx\plugins`. Full path example:
   - Steam: `C:\Program Files (x86)\Steam\steamapps\common\The Last Plague\BepInEx\plugins\CarryMoreWeight.dll`

## Uninstall

Delete `CarryMoreWeight.dll` from the `BepInEx\plugins` folder. On the next game start, each item has the weight of the game.

## Configuration

To change mod settings, edit `BepInEx\config\com.ivmakk.thelastplagueblight.carrymoreweight.cfg` while the game is closed (see [CONFIG.md](CONFIG.md) for every setting). Changes apply at the next game start.

## Troubleshooting

If a game update renames a part of the game that the mod needs, that feature turns off with one warning in the log and works as in the game: the item weights, or the slowdown of a log in your hands. The other feature stays.

Check `BepInEx\LogOutput.log` for the `Carry More Weight loaded.` line, and for warnings or errors from Carry More Weight.

## Build

Carry More Weight is a BepInEx 6 IL2CPP plugin. Building requires the .NET 8 SDK and a game installation with BepInEx. Start the game once after installing BepInEx to generate the IL2CPP interop assemblies.

```
dotnet build src/CarryMoreWeight.csproj -c Release
```

`Directory.Build.props` sets `GameDir` to the default Steam installation path. For another location, set the `GameDir` environment variable or pass `-p:GameDir=...` to the build command. The output DLL is `src\bin\Release\CarryMoreWeight.dll`.

Unit tests cover all files of `src/` and run with no game and no BepInEx, because the test project compiles the patch code against fake game types.

```
dotnet test tests/CarryMoreWeight.Tests
```

## Package

```
dotnet build src/CarryMoreWeight.csproj -c Release -p:Package=true
```

The zip is `dist/CarryMoreWeight-<version>.zip`, laid out as `BepInEx/plugins/CarryMoreWeight.dll`.

## License

Licensed under the GNU General Public License v3.0. Copyright (C) 2026 ivmakk. See [LICENSE](LICENSE).

You may reuse and modify this mod, but you must keep it open under the same license and give credit. Do not reupload it without credit.
