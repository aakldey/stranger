using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(Ladder))]
public class LadderEditor : Editor {
	string num = "1";

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Ladder ladder = (Ladder)target;

		num = EditorGUILayout.TextField("Звеньев: ", num);
		if(GUILayout.Button("Создать лестницу")) {

			if (ladder.gameObject.GetComponent<BoxCollider2D>() != null) {
				DestroyImmediate(ladder.gameObject.GetComponent<BoxCollider2D>());
			}
			foreach(Transform child in ladder.gameObject.transform) {
				DestroyImmediate(child.gameObject);
			}
			while(true) {
				foreach(Transform child in ladder.transform) {
					DestroyImmediate(child.gameObject);
				}
				if (ladder.transform.childCount == 0) {
					break;
				}

			}
			//Debug.Log(num);
			int n = int.Parse(num);
			Debug.Log(n.ToString());
			ladder.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(0.2f, n);
			ladder.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			float offset = n%2==0?0.5f:0;
			ladder.gameObject.GetComponent<BoxCollider2D>().center = new Vector2(0,n/2-offset);
			for (int i = 0; i < n; i++) {
				GameObject go = (GameObject)Instantiate(Resources.Load("Objects/LadderStep"));
				go.transform.parent = ladder.gameObject.transform;
				go.transform.localPosition = new Vector3(0,i,0);
			}
		}
		
	}
}
