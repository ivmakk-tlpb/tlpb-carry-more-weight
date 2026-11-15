# Configuration

Every setting lives in `BepInEx\config\com.ivmakk.thelastplagueblight.carrymoreweight.cfg`, written the first time you run the game with the mod installed. Edit it with any text editor while the game is closed. Changes apply at the next game start. A value outside its range is set to the nearest end of the range. A value that is already in the file stays after a mod update, also when the new version has another default: to get the new default, delete the line and start the game.

The divisor divides the weight of each heavy item. `0` makes each item weigh nothing. Above `0`, the divisor goes from `1` (as in the game) to `20`, and a value between `0` and `1` counts as `1`, because the mod never makes an item heavier than in the game.

## General

| Setting | Default | Values | What it does |
|---|---|---|---|
| `WeightDivisor` | `2` | `0`, or `1` to `20` | How many times lighter heavy items are: ores, ingots, bricks, planks, Firewood, logs, lumber, anvils, wheels, and shields. `2` is half the weight of the game, so you carry twice as much before you get Encumbered. With `0`, items weigh nothing and you never get Encumbered. The "Heavy" and "Very Heavy" tooltip lines follow the new weight. |
| `HandSlowdown` | `false` | `true` / `false` | `true` keeps the slowdown of the game: you run at half speed while you hold a log, a trunk, lumber, Firewood, an anvil, a wheel, a full Clay Jug, or another large heavy item in your hands, with no debuff tile. With `false`, this slowdown is gone, and only Encumbered slows you. |
| `Verbose` | `false` | `true` / `false` | Writes extra diagnostic lines to the BepInEx log for troubleshooting. Keep off in normal play. |
