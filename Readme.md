# Ski Game 1
This is a gaming project inspired in the game Ski Free from MS entertainment pack 3 for Windows 3.0 in 1991. Maybe some remember this game from their childhood at some point. I had a first look into this in Windows 95. I assume there was a port to to this operating system at some point. I want to make a recreation of this game using Godot, new game dynamics, more sensation of speed, all while keeping simmilar (but refined) looks and concept.

For reference purposes, you can make a google search at the game SkyFree: [Google Search Results](https://www.google.com/search?q=skifree+game+ms+dos) 

## How to add the game assets

The current repository doesn't contain any third party game assets. Instead,download them directly from the source:
- Snow Platform Sprites_nnekart: https://nnekart.itch.io/snow-platform-tileset
- Roboden tileset: https://quasilyte.itch.io/roboden-tileset -> When decompressing, make sure you copy from the child folder as the result of the decompression is `roboden-tileset/roboden-tileset/{content}`
- Winter Asset Pack: https://say-k.itch.io/winter-asset-pack -> Same as in the previous source, when decompressing, make sure you copy from the child folder as the result of the decompression is `Winter Asset Pack/Winter Asset Pack/{content}`

These should go into the Assets directory as follows (Dont paste the parent folder instead paste the contents of it):
- Snow Platform Sprites_nnekart -> `{root}/Assets/Tilemaps/Environment`
- Roboden tileset -> `{root}/Assets/Tilemaps/Props/GeneralFoliage`
- Winter Asset Pack -> `{root}/Assets/Tilemaps/Props/WinterOnlyFoliage`
