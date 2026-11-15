# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2026-11-15

### Added

- Heavy items weigh half as much (`WeightDivisor` 2), so you carry twice as much before you get Encumbered. With `0`, items weigh nothing and you never get Encumbered.
- A log, a trunk, lumber, Firewood, an anvil, or another large heavy item in your hands no longer halves your speed (`HandSlowdown` `false`). Only Encumbered slows you. Set `HandSlowdown` to `true` to keep the slowdown of the game.
- The limit of 40 and the Encumbered penalty stay as in the game.
- The "Heavy" and "Very Heavy" tooltip lines follow the new weight.
- The mod writes nothing to the save. Made and tested for single player.
