#pragma strict
import SpriteTile;

// In addition to animation, this demo shows the usage of layers to reduce the number of tiles,
// specifically that the grass tiles can work with both the water and path tiles.

var level : TextAsset;

function Start () {
	Tile.SetCamera();
	Tile.LoadLevel (level);
	
	// This line animates the water tiles, which start at tile #16 and have a total of 8 tiles.
	Tile.AnimateTileRange (TileInfo(4, 16), 8, 8.0);
	
	// This line animates the #27 flower tile, cycling through the 4 flower tiles.
	Tile.AnimateTile (TileInfo(4, 27), 4, 2.0);
	
	// This line animates the #29 flower tile. We want it to cycle through the 4 flower tiles also,
	// but it won't work with a range in this case, so we specify the tiles in an array.
	var tiles = [TileInfo(4, 29), TileInfo(4, 30), TileInfo(4, 27), TileInfo(4, 28)];
	Tile.AnimateTile (TileInfo(4, 29), tiles, 2.0);
}