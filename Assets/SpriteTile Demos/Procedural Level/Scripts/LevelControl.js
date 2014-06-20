// This creates a level procedurally using Perlin noise

#pragma strict
import SpriteTile;

var levelSize = Vector2(100, 100);
var perlinSize = 5.0;
var cutoff = 0.4;
var seed = 1;
var target : Transform;
private var seedString : String;

function Start () {
	Tile.SetCamera();
	Tile.NewLevel (Int2(levelSize), 0, 1.0, 0.0, LayerLock.None);
	
	CreateLevel (seed);
	seedString = seed.ToString();
}

function OnGUI () {
	GUI.color = Color.black;
	GUI.Label (Rect(10, 10, 100, 25), "Level number:");
	GUI.color = Color.white;
	var chr = Event.current.character;
	if (chr == "\n"[0]) {
		GUI.FocusControl ("Dummy");
	}
	if (chr < "0"[0] || chr > "9"[0]) {
		Event.current.character = "\0"[0];
	}
	seedString = GUI.TextField (Rect(100, 10, 40, 25), seedString, 4);
	GUI.SetNextControlName ("Dummy");
	GUI.Button (Rect(10000, 0, 120, 10), " ");
	if (GUI.Button (Rect(10, 50, 130, 30), "Change level")) {
		CreateLevel (parseInt(seedString));
	}
}

function CreateLevel (i : int) {
	// Create caves
	i *= 1000;
	var maxSize = Int2(levelSize.x - 1, levelSize.y - 1);
	for (var y = 0; y <= maxSize.y; y++) {
		for (var x = 0; x <= maxSize.x; x++) {
			if (Mathf.PerlinNoise ((x + i)/perlinSize, (y + i)/perlinSize) < cutoff) {
				Tile.SetTile (Int2(x, y), 3, 1, true);
			}
			else {
				Tile.SetTile (Int2(x, y), 3, 0, false);
			}
		}
	}

	// Create walls around the level	
	Tile.SetBorder (3, 1, true);
	
	// Make sure character is placed in an empty cell
	var pos = Int2(levelSize/2);
	var placed = false;
	while (!placed) {
		if (Tile.GetCollider (pos)) {
			pos += Int2.one;
			continue;
		}
		placed = true;
	}
	target.position = Vector2(pos.x, pos.y);
	target.GetComponent(CharacterControl).SetMapPosition();
}