// This moves a character around a SpriteTile level using square-by-square movement,
// and prevents the character from moving into collider cells
// Permission is granted to use, modify, and redistribute this script in any way.

#pragma strict
import SpriteTile;

var moveSpeed = 5.0;
var fastSpeedFactor = 4.0;
private var mapPos : Int2;
private var t = 0.0;
private var xInput = 0.0;
private var yInput = 0.0;

function Start () {
	yield;
	SetMapPosition();
	PlayerInput();
}

function SetMapPosition () {
	mapPos = Tile.GetMapPosition (transform.position);
}

// Coroutine that waits for input in the horizontal and vertical axes, and moves player accordingly
function PlayerInput () {
	while (true) {
		while (xInput == 0.0 && yInput == 0.0) {
			GetInput();
			yield;
		}
		
		if (xInput < 0.0) {
			yield Move (Int2(-1, 0));
		}
		else if (xInput > 0.0) {
			yield Move (Int2(1, 0));
		}
		else if (yInput < 0.0) {
			yield Move (Int2(0, -1));
		}
		else {
			yield Move (Int2(0, 1));
		}
	}
}

function Move (dir : Int2) {
	// No movement if the tile being moved into is a collider
	if (Tile.GetCollider (mapPos + dir)) {
		transform.position = Tile.GetWorldPosition (mapPos);	// Make sure position is exact in case the player was moving previously
		GetInput();
		return;
	}
	
	if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
		var multiplySpeed = fastSpeedFactor;
	}
	else {
		multiplySpeed = 1.0;
	}
	
	// Move to next tile
	var startPos = Tile.GetWorldPosition (mapPos);
	mapPos += dir;
	var endPos = Tile.GetWorldPosition (mapPos);
	
	while (t <= 1.0) {
		transform.position = Vector3.Lerp (startPos, endPos, t);
		t += Time.deltaTime * moveSpeed * multiplySpeed;
		yield;
	}
	t -= 1.0;	// Subtract 1 rather than resetting to 0, so that movement over multiple tiles is 100% smooth
	
	// If there's no input, then make the final position be exactly equal to endPos
	GetInput();
	if (xInput == 0.0 && yInput == 0.0) {
		transform.position = endPos;
		t = 0.0;
	}
}

function GetInput () {
	xInput = Input.GetAxis ("Horizontal");
	yInput = Input.GetAxis ("Vertical");
}