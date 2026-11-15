# Images

- `content.json` - the text, the icons, and the crops of the thumbnail and the page background. The `nexus-mod-images` tool renders both images from it.
- `01-thumb.jpg` - 1920x1080, the first gallery image, so Nexus uses it as the thumbnail in the mods grid. The title ("Carry More" in the accent, "Weight" in white), two captions with the Thick Log texture (`Texture2D/log_thick`) and the game's movement icon (`movement`), and the character in midday light, facing the camera with a Thick Log in the hands, from `src/thick-log.jpg` at 1:1.
- `02-encumbered-without-mod.jpg` - 1920x1080, gallery. The game weights (`WeightDivisor` 1): a Thick Log in the hands gives the Encumbered tooltip "Stamina depletes 180% faster" and "Movement speed is 36% slower". From `src/encumbered-game.jpg`, scaled down.
- `03-encumbered-with-mod.jpg` - 1920x1080, gallery. With the mod at its default (`WeightDivisor` 2): the same Thick Log gives "Stamina depletes 30% faster" and "Movement speed is 6% slower". From `src/encumbered-mod.jpg`, scaled down.
- `banner.jpg` - 1300x372, the page background. The character with the Thick Log, the camp, and the fire of `src/thick-log.jpg`, darker at the left and the bottom, so the mod name reads.
- `src/` - the screenshots, taken on 2026-11-15 at 3840x2160 on game version 0.12.7 at 12:00 game time in the game driver session `20261115-153947-025-session` on a test save, and stored as JPG at quality 95. `thick-log.jpg` has the HUD hidden.

The gallery crops are `[0, 560, 2844, 1600]` of the source screenshots, scaled to 1920x1080.

The description has no inline image. If one is added, upload the file to the Nexus images of the mod first and use the hosted URL.
