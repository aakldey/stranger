using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour, IRoomManager {

	public List<GameObject> roomPrefabs;
	private System.Random rand;

	// Use this for initialization
	void Start () {
		rand = new System.Random();
		foreach(GameObject obj in roomPrefabs) {
			obj.GetComponent<Room>().roomManager = gameObject;
		}

	}
	
	// Update is called once per frame
	void Update () {
		//foreach(Room r in roomPrefabs)
		//	r.Update();
	}

	private Room tryInsert(Room newRoom, EntryPoint sender) {
		bool rotatable = newRoom.rotatable;
		int count = rotatable?4:1;
		for (int i = 0; i < count; i++){
			List<EntryPoint> list = newRoom.getFreeEntryPoints().FindAll(a => (a.entrySide == getOppositeEntrySide(sender.entrySide)));
			Vector3 oldP = newRoom.gameObject.transform.position;
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
			if(rotatable)
				newRoom.RotateRight();
		}

		return null;
	}

	public GameObject choseRoom(EntryPoint sender) {
		List<GameObject> rooms = roomPrefabs.FindAll(a => (a.GetComponent<Room>().hasToConnect(sender)));
		//foreach(GameObject room in roomPrefabs) {
			//Room newRoom1 = ((GameObject)(Instantiate(room))).GetComponent<Room>();
			//newRoom1.transform.position = new Vector3(-10000,-10000,-1000);

			//if (newRoom1.hasToConnect(sender))
			//    rooms.Add(room);
			//Destroy(newRoom1.gameObject);
		//}
		//Debug.Log("Кандидаты в комнаты: "+rooms.Count);
		Room newRoom = ((GameObject)(Instantiate(rooms[rand.Next(0,rooms.Count)]))).GetComponent<Room>();
		//while(true){
		if(newRoom.flipHorizontal && newRoom.randomHorizontal) {
			System.Random r = new System.Random();
			if(r.Next(1, 16) >= 8)
				newRoom.FlipHorizontal();
		}

		if(newRoom.flipVertical && newRoom.randomVertical) {
			System.Random r = new System.Random();
			if(r.Next(1, 16) >= 8)
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
