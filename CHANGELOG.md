# Changelog# Changelog

## 1.0.0 (2025-01-14)


### Features

* Merge pull request [#4](https://github.com/dobiewan/LCGhostMod/issues/4) from dobiewan/develop ([7a1df94](https://github.com/dobiewan/LCGhostMod/commit/7a1df94a2e8d05372377df027542d7e13ee50407))


### Bug Fixes

* Fix a bug that meant the spectator could not hear the ghost SFX after dying twice due to SFX object being unloaded ([5596e52](https://github.com/dobiewan/LCGhostMod/commit/5596e524e276b9e1e9fad30133fb1544f92558e9))
* Fix an issue that meant only the host could hear their own ghost noises while spectating ([7ef8bb2](https://github.com/dobiewan/LCGhostMod/commit/7ef8bb2edeaf131fbb1fca07f579bf6abd8b1a69))

## [1.1.3] (2025-01-08)


* Updated readme with trailer video link.


## [1.1.2] (2025-01-08)


### Bug Fixes

* Fix an issue that meant only the host could hear their own ghost noises while spectating.


## [1.1.1] (2025-01-07)


### Bug Fixes

* Fix an issue that prevented a spectator from hearing ghost noises after the first run.

## [1.1.0] (2025-01-04)


### Features

* Expose certain values in a config file. Exposed values include:
    *  Microphone Amp Threshold
    *  Min/Max Cooldown Time
    *  Randomize Pitch Range
    *  Randomize Volume Range

## 1.0.0 (2025-01-03)


### Features

* Initial release! If a spectating user creates an input of sufficient amplitude, the user they are watching, and anyone else watching them, will hear a ghost sound effect.
