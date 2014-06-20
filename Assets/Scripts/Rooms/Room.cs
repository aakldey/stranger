using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SpriteTile;
using UnityEditor;



public class Room : MonoBehaviour {
	
	public Vector2 size;

	public GameObject roomManager;

	public Room() {

	}

	public void Start() {
		roomManager = GameObject.FindGameObjectWithTag("RoomManager");
	}

	public void Update() {

	}

	public List<EntryPoint> getEntryPoint(EntrySide side) {
		List<EntryPoint> list = new List<EntryPoint>(GetComponentsInChildren<EntryPoint>());
		return list.FindAll(a => (a.entrySide == side));
	}

	public List<EntryPoint> getFreeEntryPoints() {
		List<EntryPoint> list = new List<EntryPoint>(GetComponentsInChildren<EntryPoint>());
		return list.FindAll(a => (a.connectedEntry == null));
	}

	public List<EntryPoint> getAllEntryPoints() {
		List<EntryPoint> list = new List<EntryPoint>(GetComponentsInChildren<EntryPoint>());
		return list;
	}

	public List<EntryPoint> getAllConnectedPoints() {
		List<EntryPoint> list = new List<EntryPoint>(GetComponentsInChildren<EntryPoint>());
		return list.FindAll(a => (a.connectedEntry != null));
	}

	public List<EntryPoint> getConnectedEntryPoints(EntryPoint point) {
		List<EntryPoint> list = getAllConnectedPoints();
		list.Remove(point);
		return list;
	}

	public void RotateRight() {
		foreach(EntryPoint p in GetComponentsInChildren<EntryPoint>()) {
			if(p.entrySide ==EntrySide.DOWN)
				p.entrySide = 0;
			else
				p.entrySide+=1;

			if(p.entryDir ==EntryDir.LEFT)
				p.entryDir = 0;
			else
				p.entryDir +=1;





		}

		gameObject.transform.Rotate(0, 0, -90);
	}

	public void FlipHorizontal() {
		foreach(EntryPoint p in GetComponentsInChildren<EntryPoint>()) {
			if(p.entrySide ==EntrySide.LEFT)
				p.entrySide = EntrySide.RIGHT;
			else
				p.entrySide = EntrySide.LEFT;


		}
		gameObject.transform.Rotate(0, 180, 0);
	}


}
