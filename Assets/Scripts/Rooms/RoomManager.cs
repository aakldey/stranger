using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour, IRoomManager {

	public List<GameObject> roomPrefabs;
	private System.Random rand;

	// Use this for initialization
	void Start () {
		rand = new System.Random();

	}
	
	// Update is called once per frame
	void Update () {
		//foreach(Room r in roomPrefabs)
		//	r.Update();
	}

	public GameObject choseRoom(EntryPoint sender) {
		Room newRoom = ((GameObject)(Instantiate(roomPrefabs[rand.Next(0,roomPrefabs.Count)]))).GetComponent<Room>();
		for(int i = 0; i < 8; i++) {
			if (i == 4)
				newRoom.FlipHorizontal();

			List<EntryPoint> list = newRoom.getFreeEntryPoints().FindAll(a => (a.entrySide == getOppositeEntrySide(sender.entrySide)));
			Vector3 oldP = newRoom.gameObject.transform.position;
			foreach(EntryPoint point in list) {
				if (point.entryDir == sender.entryDir) {
					GameObject root = new GameObject();
					root.transform.position = point.transform.position;
					Vector3 tmp = point.gameObject.transform.position;
					newRoom.gameObject.transform.parent = root.transform;
					root.transform.position = point.gameObject.transform.position;
					newRoom.transform.Translate(point.transform.position-tmp);
					root.transform.position = sender.transform.position;

				/*	Vector3 newPos = root.transform.position;
					switch(sender.entrySide) {
						case EntrySide.LEFT:
							newPos.x -=1;
							break;
						case EntrySide.RIGHT:
							newPos.x +=1;
							break;
					}
					root.transform.position = newPos;*/
					return newRoom.gameObject;
				} 


			}
			newRoom.RotateRight();

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

		if (sender.entrySide == EntrySide.RIGHT) {
			GameObject newRoom = choseRoom(sender);
//			newRoom.transform.position = pos;

			List<EntryPoint> list = new List<EntryPoint>(newRoom.GetComponentsInChildren<EntryPoint>());
			EntryPoint point = list.Find(a => (a.entrySide == EntrySide.LEFT));
			point.connectedEntry = sender;
			sender.connectedEntry = point;

			return point;
		} else if (sender.entrySide == EntrySide.LEFT) {
			GameObject newRoom = choseRoom(sender);

			List<EntryPoint> list = new List<EntryPoint>(newRoom.GetComponentsInChildren<EntryPoint>());
			
			EntryPoint point = list.Find(a => (a.entrySide == EntrySide.RIGHT));
			point.connectedEntry = sender;
			sender.connectedEntry = point;
			
			return point;
		}
		return null;
	}

	
}
