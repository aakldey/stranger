using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour, IRoomManager {

	public List<GameObject> roomPrefabs;
	private System.Random rand;

	// Use this for initialization
	void Start () {
		rand = new System.Random();
		GameObject roomsPull = GameObject.FindGameObjectWithTag("RoomsPull");
		List<GameObject> pull = new List<GameObject>();
		foreach(GameObject obj in roomPrefabs) {
			GameObject newRoom = (GameObject)(Instantiate(obj));
			newRoom.transform.parent = roomsPull.transform;
			newRoom.transform.position = new Vector3(-10000,-10000,-10000);
			newRoom.GetComponent<Room>().roomManager = gameObject.GetComponent<RoomManager>();
			pull.Add(newRoom);
		} 
		roomPrefabs = pull;
	}
	
	// Update is called once per frame
	void Update () {

	}

	private Room tryInsert(Room newRoom, EntryPoint sender) {
		int count = newRoom.rotatable?4:1;
		List<EntryPoint> list;
		for (int i = 0; i < count; i++){
			 list = newRoom.getFreeEntryPoints().FindAll(a => (a.entrySide == getOppositeEntrySide(sender.entrySide)));
			//Vector3 oldP = newRoom.gameObject.transform.position;
			foreach(EntryPoint point in list) {
				if (point.entryDir == sender.entryDir && point.connectedEntry == null) {
					GameObject root = new GameObject();
					root.transform.position = point.transform.position;
					Vector3 tmp = point.gameObject.transform.position;
					newRoom.gameObject.transform.parent = root.transform;
					root.transform.position = point.gameObject.transform.position;
					newRoom.transform.Translate(point.transform.position-tmp);
					root.transform.position = sender.transform.position;
					return newRoom;
				} 				
			}
			if(count != 1)
				newRoom.RotateRight();
		}
		return null;
	}

	public GameObject choseRoom(EntryPoint sender) {
		List<GameObject> rooms = roomPrefabs.FindAll(a => (a.GetComponent<Room>().hasToConnect(sender)));
		//Debug.Log(rooms.Count);
		Room newRoom = ((GameObject)(Instantiate(rooms[rand.Next(0,rooms.Count)]))).GetComponent<Room>();
		//while(true){
		if(newRoom.flipHorizontal && newRoom.randomHorizontal) {
			if(rand.Next(1, 16) >= 8)
				newRoom.FlipHorizontal();
		}

		if(newRoom.flipVertical && newRoom.randomVertical) {
			if(rand.Next(1, 16) >= 8)
				newRoom.FlipVertical();
		}

		if(tryInsert(newRoom, sender) == null) {
			if (newRoom.flipHorizontal) {
				newRoom.FlipHorizontal();
				Room room = tryInsert(newRoom, sender);
				if (room != null)
					return room.gameObject;
			}
			if (newRoom.flipVertical) {
				newRoom.FlipVertical();
				Room room = tryInsert(newRoom, sender);
				if (room != null)
					return room.gameObject;
			}
		}
		return newRoom.gameObject;
	}

	private EntrySide getOppositeEntrySide(EntrySide side) {
		switch(side){
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




	public EntryPoint spawnRoom(EntryPoint sender) {

			GameObject newRoom = choseRoom(sender);
//			newRoom.transform.position = pos;
			List<EntryPoint> list = new List<EntryPoint>(newRoom.GetComponentsInChildren<EntryPoint>());
			EntryPoint point = list.Find(a => (a.entrySide == sender.getOppositeSide()));
			point.connectedEntry = sender;
			sender.connectedEntry = point;

			return point;

	}

	
}
