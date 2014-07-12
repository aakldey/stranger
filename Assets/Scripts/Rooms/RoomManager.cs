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
		foreach(Room room in roomsPull.GetComponentsInChildren<Room>()) {
			pull.Add(room.gameObject);
		} 
		roomPrefabs = pull;
        
	}
	
	// Update is called once per frame
	void Update () {

	}

	private Room tryInsert(Room newRoom, EntryPoint sender) {
		int count = newRoom.rotatable?4:1;
		List<EntryPoint> list;
		//newRoom.FlipHorizontal();
		for (int i = 0; i < count; i++){
			 list = newRoom.getFreeEntryPoints().FindAll(a => (a.entrySide == getOppositeEntrySide(sender.entrySide)));
			//Vector3 oldP = newRoom.gameObject.transform.position;
			foreach(EntryPoint point in list) {
				if (point.entryDir == sender.entryDir && (sender.pointsToConnectWith.Contains(point) || sender.pointsToConnectWith.Count == 0)) {
                    GameObject root = newRoom.transform.parent.gameObject;
					Debug.Log(root.name);
					Vector3 tmp = root.transform.position - point.transform.position;
					Vector3 tmp2 = point.transform.position;
					root.transform.position = tmp2;
					
					//newRoom.gameObject.transform.parent = root.transform;
					newRoom.transform.Translate(tmp, Space.World);
					root.transform.localPosition = sender.transform.position;   
					point.connectedEntry = sender;
					sender.connectedEntry = point;
					newRoom.inUse = true;
					return newRoom;
				} 				
			}
			if(count != 1)
				newRoom.RotateRight();
		}
		return null;
	}

	public GameObject choseRoom(EntryPoint sender) {
        List<GameObject> rooms = roomPrefabs.FindAll(a => (a.GetComponent<Room>().hasToConnect(sender) && !a.GetComponent<Room>().inUse));
		//Debug.Log(rooms.Count);
		//Room newRoom = ((GameObject)(Instantiate(rooms[rand.Next(0,rooms.Count)]))).GetComponent<Room>();
        Room newRoom = rooms[rand.Next(0, rooms.Count)].GetComponent<Room>();
        //while(true){
		if(newRoom.flipHorizontal && newRoom.randomHorizontal) {
			if(rand.Next(1, 16) >= 8)
				newRoom.FlipHorizontal();
		}

		if(newRoom.flipVertical && newRoom.randomVertical) {
			if(rand.Next(1, 16) >= 8)
				newRoom.FlipVertical();
		}
		Room r1 = tryInsert(newRoom, sender);

		if(r1 == null) {
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
		} else {
			return r1.gameObject;
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




	public void spawnRoom(EntryPoint sender) {

			GameObject newRoom = choseRoom(sender);
//			newRoom.transform.position = pos;
		//	List<EntryPoint> list = new List<EntryPoint>(newRoom.GetComponentsInChildren<EntryPoint>());
		//	EntryPoint point = list.Find(a => (a.entrySide == sender.getOppositeSide()));
		//	point.connectedEntry = sender;
		//	sender.connectedEntry = point;
		//	newRoom.GetComponent<Room>().inUse = true;

			//return point;

	}

	
}
