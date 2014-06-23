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





		
	}
	

}
