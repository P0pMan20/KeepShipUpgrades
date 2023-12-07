# KeepShipUpgrades v0.0.1

This mod will ensure ship upgrades (loud-horn etc) and placeables (pumpkins) persist through rounds

Currently it doesn't really work and I need to rewrite it to work properly (see comments and commit messages)
## Explaination from my comments
```C#
    // this is really shit and should be rewritten to be more easily updatable
    // and less copyright infringing
    // and less fuck up others mods
    // it literally just copies the original code (from ILSpy) and removes all the shit that deletes upgrades
    // We also skip any prefix that comes after this mod
    // ideally you would copy the list of bought unlockables (ie loud horn, tv and pumpkin) before removal
    // and give them back in a postfix
    // might do it tomorrow```