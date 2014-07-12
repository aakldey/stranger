using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SpriteTile;
using UnityEditor;

[AddComponentMenu("Stranger/Room")]
public class Room : MonoBehaviour {
	
	public Vector2 size;

	public RoomManager roomManager;

	public List<EntryPoint> points;

	public bool rotatable = false;

	public bool flipHorizontal = false;

	public bool flipVertical = false;

	public bool randomHorizontal = false;

	public bool randomVertical = false;

	public bool inUse = false;

	public new string name = "Room";


	public Room() {

	}

	public void Start() {
		roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
		points = getAllEntryPoints();
		//Debug.Log("START "+ gameObject.name);
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
		List<Transform> list = new List<Transform>(GetComponentsInChildren<Transform>(true));
		List<EntryPoint> points = new List<EntryPoint>();
		foreach(Transform t in list) {
			if (t.gameObject.GetComponent<EntryPoint>() != null)
				points.Add(t.gameObject.GetComponent<EntryPoint>());
		}
		return points;
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
		foreach(EntryPoint p in getAllEntryPoints()) {
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
		foreach(EntryPoint p in points) {
			if(p.entrySide ==EntrySide.LEFT)
				p.entrySide = EntrySide.RIGHT;
			else if (p.entrySide == EntrySide.RIGHT)
				p.entrySide = EntrySide.LEFT;

			if(p.entryDir == EntryDir.LEFT)
				p.entryDir = EntryDir.RIGHT;
			else if(p.entryDir == EntryDir.RIGHT)
				p.entryDir = EntryDir.LEFT;


		}
		transform.Rotate(0, 180, 0);
	}


	public void FlipVertical() {
		Debug.Log("Flip vertical room "+gameObject.ToString());
		foreach (EntryPoint p in points) {
			if (p.entrySide == EntrySide.UP) 
				p.entrySide = EntrySide.DOWN;
			else if (p.entrySide == EntrySide.DOWN)
				p.entrySide = EntrySide.UP;

			if(p.entryDir == EntryDir.UP)
				p.entryDir = EntryDir.DOWN;
			else if(p.entryDir == EntryDir.DOWN)
				p.entryDir = EntryDir.UP;		

		}
		gameObject.transform.Rotate(180, 0, 0);
	}

	private EntrySide getRotationSide(EntrySide side, int i) {
		switch(i){
		case 0:
			return side;
			break;
		case 1:
			return (EntrySide)(((int)side+1)% 4);
			break;
		case 2:
			return (EntrySide)(((int)side+2)% 4);
			break;
		case 3:
			return (EntrySide)(((int)side+3)% 4);
			break;
		}
		return EntrySide.DOWN;
	}

	private EntryDir getRotationDir(EntryDir dir, int i) {
		switch(i){
		case 0:
			return dir;
			break;
		case 1:
			return (EntryDir)(((int)dir+1)% 4);
			break;
		case 2:
			return (EntryDir)(((int)dir+2)% 4);
			break;
		case 3:
			return (EntryDir)(((int)dir+3)% 4);
			break;
		}
		return EntryDir.DOWN;
	}



	public bool hasToConnect(EntryPoint point) {
		points = getAllEntryPoints();
		for (int i = 0; i < 4; i++) {
			if ((i > 0 && rotatable) || i == 0)
			foreach(EntryPoint p in points) {
				if (p.entrySide == getRotationSide(point.getOppositeSide(), i)){
					if (p.entryDir == getRotationDir(point.entryDir, i) && (point.canConnectWith.Count == 0 || point.canConnectWith.Find(a => (a.name == this.name)) != null || point.canConnectWith.Find(a => (a.name == "AllRooms")) != null) && (point.ignoreRooms.Find(a => (a.name == this.name)) == null) ) {
						//Debug.Log("has to connect");
						return true;
					}
				}
			}

		}

		if (flipHorizontal) {
			for (int i = 0; i < 4; i++) {
				
				if ((i > 0 && rotatable) || i == 0)
				foreach(EntryPoint p in points) {
					if (p.getHotizontalFlipSide() == getRotationSide(point.getOppositeSide(), i) && (point.canConnectWith.Count == 0 || point.canConnectWith.Find(a => (a.name == this.name)) != null || point.canConnectWith.Find(a => (a.name == "AllRooms")) != null)&& (point.ignoreRooms.Find(a => (a.name == this.name)) == null)) {
						if (p.getHorizontalFlipDir() == getRotationDir(point.entryDir, i)) {
							//Debug.Log("has to connect");
							return true;
						}
					}
				}
				
			}
		}

		if (flipVertical) {
			for (int i = 0; i < 4; i++) {
				
				if ((i > 0 && rotatable) || i == 0)
				foreach(EntryPoint p in points) {
					if (p.getVerticalFlipSide() == getRotationSide(point.getOppositeSide(), i) && (point.canConnectWith.Count == 0 || point.canConnectWith.Find(a => (a.name == this.name)) != null || point.canConnectWith.Find(a => (a.name == "AllRooms")) != null)&& (point.ignoreRooms.Find(a => (a.name == this.name)) == null)) {
						if (p.getVerticalFlipDir() == getRotationDir(point.entryDir, i)) {
							//Debug.Log("has to connect");
							return true;
						}
					}
				}
				
			}
		}

		return false;
	}

	public GameObject BuildLeftEntry() {
		GameObject trigger = new GameObject();
		trigger.name = "Left_Enter";
		trigger.transform.parent = gameObject.transform;
		BoxCollider2D col = trigger.AddComponent<BoxCollider2D>();
		col.isTrigger = true;
		col.size = new Vector2(1, 3);
		col.center = new Vector2(0.5f, 0);
		Rigidbody2D r = trigger.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		EntryPoint point = trigger.AddComponent<EntryPoint>();
		point.entryDir = EntryDir.UP;
		point.entrySide = EntrySide.LEFT;
		return trigger;
	}

	public GameObject BuildRightEntry() {
		GameObject trigger = new GameObject();
		trigger.name = "Right_Enter";
		trigger.transform.parent = gameObject.transform;
		BoxCollider2D col = trigger.AddComponent<BoxCollider2D>();
		col.isTrigger = true;
		col.size = new Vector2(1, 3);
		col.center = new Vector2(-0.5f, 0);
		Rigidbody2D r = trigger.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		EntryPoint point = trigger.AddComponent<EntryPoint>();
		point.entryDir = EntryDir.UP;
		point.entrySide = EntrySide.RIGHT;
		return trigger;
	}

	public GameObject BuildDownEntry() {
		GameObject trigger = new GameObject();
		trigger.name = "Left_Enter";
		trigger.transform.parent = gameObject.transform;
		BoxCollider2D col = trigger.AddComponent<BoxCollider2D>();
		col.isTrigger = true;
		col.size = new Vector2(3, 0.2f);
		col.center = new Vector2(0, 0.2f);
		Rigidbody2D r = trigger.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		EntryPoint point = trigger.AddComponent<EntryPoint>();
		point.entryDir = EntryDir.LEFT;
		point.entrySide = EntrySide.DOWN;
		return trigger;
	}

	public GameObject BuildUpEntry() {
		GameObject trigger = new GameObject();
		trigger.name = "Left_Enter";
		trigger.transform.parent = gameObject.transform;
		BoxCollider2D col = trigger.AddComponent<BoxCollider2D>();
		col.isTrigger = true;
		col.size = new Vector2(3, 0.2f);
		col.center = new Vector2(0, -0.1f);
		Rigidbody2D r = trigger.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		EntryPoint point = trigger.AddComponent<EntryPoint>();
		point.entryDir = EntryDir.LEFT;
		point.entrySide = EntrySide.UP;
		return trigger;
	}


}
