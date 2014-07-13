using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectLayer : MonoBehaviour {
	
	[HideInInspector] public Vector2 gridSize = new Vector2(32, 32);
	[HideInInspector] public Vector2 gridOffset = new Vector2(0, 0);
	[HideInInspector] public Vector2 layerSize = new Vector2(100, 50);
	
	[HideInInspector] public Transform selected;
	[HideInInspector] public bool snapToGrid = true;
	
	public GameObject [] prefabs;
	
	
	public Bounds FindBounds(Transform t) {
		Bounds ret;
		if(t.renderer!=null) ret = t.renderer.bounds;
		else if(t.collider!=null) ret = t.collider.bounds;
		else ret = new Bounds(t.position, new Vector3(gridSize.x, gridSize.y, 10));
		ret.size *= 1.1f;
		
		return ret;
	}
	
	
	
}
