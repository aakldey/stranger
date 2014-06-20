using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public enum EntryType {
	ENTER, EXIT
}

public enum EntryDir {
	UP, RIGHT, DOWN, LEFT
}

public enum EntrySide {
	LEFT, UP, RIGHT,  DOWN
}

public class EntryPoint : MonoBehaviour {

	public EntryType entryType;
	public EntrySide entrySide;
	public EntryDir entryDir;

	public EntryPoint connectedEntry;

	private CharacterController player;

	// Use this for initializations
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		//if(other.tag == "Crate") {
			//other.transform.parent = this.transform.parent;
			//PrefabUtility.ReplacePrefab(this.connectedEntry.transform.parent.gameObject, PrefabUtility.GetPrefabParent(this.connectedEntry.transform.parent.gameObject), ReplacePrefabOptions.ConnectToPrefab);
		//	PrefabUtility.ReplacePrefab(this.transform.parent.gameObject, PrefabUtility.GetPrefabParent(this.transform.parent.gameObject), ReplacePrefabOptions.ConnectToPrefab);
		//}

		
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "PlayerTrigger") {
			if (entryType == EntryType.ENTER) {
				//getRoom().RotateRight();
				if ((player.isFacingRight && entrySide == EntrySide.LEFT) || (!player.isFacingRight && entrySide == EntrySide.RIGHT)) {
					
					Room room = getRoom();
					List<EntryPoint> list = connectedEntry.getRoom().getConnectedEntryPoints(connectedEntry);
					Debug.Log(list.Count);
					foreach(EntryPoint point in list) {
						GameObject r = point.connectedEntry.getRoom().gameObject;
						point.connectedEntry = null;
						
						if(r != null) {
							Debug.Log("deleting");
							Destroy(r);
						}
					}
				}
				
				if ((player.isFacingRight && entrySide == EntrySide.RIGHT) || (!player.isFacingRight && entrySide == EntrySide.LEFT)) {
					IRoomManager manager = transform.parent.gameObject.GetComponent<Room>().roomManager.GetComponent<RoomManager>();
					List<EntryPoint> freeList = connectedEntry.getRoom().getFreeEntryPoints();
					foreach(EntryPoint point in freeList) {
						EntryPoint p = manager.spawnRoom(point);
					}
				}
				
			}
		}

	}

	public Room getRoom() {
		return transform.parent.gameObject.GetComponent<Room>();
	}
}
