using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {

	public List<GameObject> objects = new List<GameObject>(); 


	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		GameObject go = null;
		Room myRoom = (Room)target;
		if(GUILayout.Button("Создать левый вход")) {
			go = myRoom.BuildLeftEntry();

		}

		if(GUILayout.Button("Создать правый вход")) {
			go = myRoom.BuildRightEntry();
		}

		if(GUILayout.Button("Создать верхний вход")) {
			go = myRoom.BuildUpEntry();
		}

		if(GUILayout.Button("Создать нижний вход")) {
			go = myRoom.BuildDownEntry();
		}

		GUI.contentColor = Color.white; 
		GUI.color = Color.Lerp(Color.red, Color.gray, 0.5f);

		if(GUILayout.Button("Вставить в пулл")) {
			if (myRoom.name == "Room") {
				EditorUtility.DisplayDialog("FATAL ERROR!","Задай имя комнате!", "OOkaaay :(");
			} else {
				List<EntryPoint> points = new List<EntryPoint>(myRoom.GetComponentsInChildren<EntryPoint>()); 
				if (points.Count == 0) {
					EditorUtility.DisplayDialog("FATAL ERROR!","У комнаты нет входов!", "OOkaaay :(");
				} else {
					foreach(Transform tr in myRoom.transform) {
						if (tr.gameObject.name == "_LevelParent") {
							tr.gameObject.name = "Tiles";
							tr.gameObject.layer = LayerMask.NameToLayer("Ground");
							foreach(Transform t in tr.gameObject.transform) {
								t.gameObject.layer = LayerMask.NameToLayer("Ground");
							}
						}
					}
					myRoom.gameObject.name = myRoom.name;
					GameObject obj = new GameObject();
					obj.transform.parent = GameObject.FindGameObjectWithTag("RoomsPull").transform;
					obj.name = myRoom.name+"Pivot";
					myRoom.transform.parent = obj.transform;
					obj.transform.position = new Vector3(-1000,-1000,-1000);

				}
			}


		}





		
	}
	

}
