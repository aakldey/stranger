#pragma strict
import SpriteTile;

var target : Transform;

function LateUpdate () {
	transform.position = Vector3(target.position.x, target.position.y, transform.position.z);
}