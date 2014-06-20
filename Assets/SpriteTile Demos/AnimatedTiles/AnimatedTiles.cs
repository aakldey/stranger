using UnityEngine;
using SpriteTile;

// In addition to animation, this demo shows the usage of layers to reduce the number of tiles,
// specifically that the grass tiles can work with both the water and path tiles.

public class AnimatedTiles : MonoBehaviour {

	public TextAsset level;

	void Start () {
		Tile.SetCamera();
		Tile.LoadLevel (level);
		
		// This line animates the water tiles, which start at tile #16 and have a total of 8 tiles.
		Tile.AnimateTileRange (new TileInfo(4, 16), 8, 8.0f);
		
		// This line animates the #27 flower tile, cycling through the 4 flower tiles.
		Tile.AnimateTile (new TileInfo(4, 27), 4, 2.0f);
		
		// This line animates the #29 flower tile. We want it to cycle through the 4 flower tiles also,
		// but it won't work with a range in this case, so we specify the tiles in an array.
		TileInfo[] tiles = {new TileInfo(4, 29), new TileInfo(4, 30), new TileInfo(4, 27), new TileInfo(4, 28)};
		Tile.AnimateTile (new TileInfo(4, 29), tiles, 2.0f);
	}
}
