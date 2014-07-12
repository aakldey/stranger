using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EntryDir {
	UP, RIGHT, DOWN, LEFT
}

public enum EntrySide {
	LEFT, UP, RIGHT,  DOWN
}

public class EntryPoint : MonoBehaviour {
	
	public EntrySide entrySide;
	public EntryDir entryDir;

	public EntryPoint connectedEntry;

	private CharacterController player;

	private List<EntryPoint> list;

	public List<Room> canConnectWith = new List<Room>();
	public List<Room> ignoreRooms = new List<Room>();

	public List<EntryPoint> pointsToConnectWith = new List<EntryPoint>();
	
	// Use this for initializations
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() 
	{
		//Debug.Log(entrySide.ToString());
		Vector3 pos = transform.position;
		pos.x += GetComponent<BoxCollider2D>().center.x;
		Gizmos.DrawIcon(pos, entryDir.ToString()+"_"+entrySide.ToString()+".tif", false);
	}


	void OnTriggerEnter2D(Collider2D other) {
		//if(other.tag == "Crate") {
			//other.transform.parent = this.transform.parent;
			//PrefabUtility.ReplacePrefab(this.connectedEntry.transform.parent.gameObject, PrefabUtility.GetPrefabParent(this.connectedEntry.transform.parent.gameObject), ReplacePrefabOptions.ConnectToPrefab);
		//	PrefabUtility.ReplacePrefab(this.transform.parent.gameObject, PrefabUtility.GetPrefabParent(this.transform.parent.gameObject), ReplacePrefabOptions.ConnectToPrefab);
		//}

		
	}

	public EntrySide getOppositeSide() {
		switch(entrySide){
		case EntrySide.LEFT:
			return EntrySide.RIGHT;
		case EntrySide.RIGHT:
			return EntrySide.LEFT;
		case EntrySide.DOWN:
			return EntrySide.UP;
		case EntrySide.UP:
			return EntrySide.DOWN;
		}
		return EntrySide.DOWN;
	}

	public EntryDir getOppositeDir() {
		switch(entryDir){
		case EntryDir.LEFT:
			return EntryDir.RIGHT;
		case EntryDir.RIGHT:
			return EntryDir.LEFT;
		case EntryDir.DOWN:
			return EntryDir.UP;
		case EntryDir.UP:
			return EntryDir.DOWN;
		}
		return EntryDir.DOWN;
	}

	public EntrySide getHotizontalFlipSide() {
		switch(entrySide){
			case EntrySide.LEFT:
				return EntrySide.RIGHT;
			case EntrySide.RIGHT:
				return EntrySide.LEFT;

			}
		return entrySide;
	}

	public EntrySide getVerticalFlipSide() {
		switch(entrySide){
		case EntrySide.UP:
			return EntrySide.DOWN;
		case EntrySide.DOWN:
			return EntrySide.UP;
			
		}
		return entrySide;
	}

	public EntryDir getHorizontalFlipDir() {
		switch(entryDir) {
		case EntryDir.LEFT:
			return EntryDir.RIGHT;
		case EntryDir.RIGHT:
			return EntryDir.LEFT;
		}
		return entryDir;
	}

	public EntryDir getVerticalFlipDir() {
		switch(entryDir) {
		case EntryDir.UP:
			return EntryDir.DOWN;
		case EntryDir.DOWN:
			return EntryDir.UP;
		}
		return entryDir;
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "PlayerTrigger") {
			//getRoom().RotateRight();
		
			if ((player.isFacingRight && entrySide == EntrySide.LEFT) || (!player.isFacingRight && entrySide == EntrySide.RIGHT) ||
			    (player.isFacingUp && entrySide == EntrySide.DOWN) || (!player.isFacingUp && entrySide == EntrySide.UP)) {
				
				Room room = getRoom();
				list = connectedEntry.getRoom().getConnectedEntryPoints(connectedEntry);
				//Debug.Log(list.Count);
				foreach(EntryPoint point in list) {
					GameObject r = point.connectedEntry.getRoom().gameObject;

					
					if(r != null) {
						if(r.GetComponent<Room>().inUse) {
							//Debug.Log("deleting");
	                        r.transform.parent.transform.position = new Vector3(-10000, -10000, -10000);
							r.transform.localPosition = Vector3.zero;
	                        r.GetComponent<Room>().inUse = false;
	                        List<EntryPoint> ps = r.GetComponent<Room>().getAllEntryPoints();
	                        foreach (EntryPoint e in ps) {
	                            if(e.connectedEntry != null) {
									e.connectedEntry.connectedEntry = null;
	                            	e.connectedEntry = null;
								}
	                        }
						}
						//point.connectedEntry = null;
                        //Destroy(r.transform.parent.gameObject);
					}
				}
			}
			
			if ((player.isFacingRight && entrySide == EntrySide.RIGHT) || (!player.isFacingRight && entrySide == EntrySide.LEFT) ||
			    (player.isFacingUp && entrySide == EntrySide.UP) || (!player.isFacingUp && entrySide == EntrySide.DOWN)) {
				IRoomManager manager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
				//Debug.Log(getRoom().name + " "+name);
//				//Debug.Log(connectedEntry.ToString());
				list = connectedEntry.getRoom().getFreeEntryPoints();
				foreach(EntryPoint point in list) {
					manager.spawnRoom(point);
				}
			}

		}

	}

	public Room getRoom() {
		return transform.parent.gameObject.GetComponent<Room>();
	}
}
