// Copyright 2011 Sven Magnus

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(UniTileManager))]

public class UniTileManagerEditor : Editor {
	
	private int selectedTemplate;
	
	public override void OnInspectorGUI () {
		
		if(this.target.name!="UniTileManager") this.target.name="UniTileManager";
		
		Rect rect = EditorGUILayout.BeginVertical();
		EditorGUILayout.EndVertical();
		
		if (GUI.Button(new Rect(10, rect.y+rect.height + 10, 100, 30), "Add tile layer")) 
		{
			TileLayerEditorAmmendum.CreateTileLayer();
		}
		
		if (GUI.Button(new Rect(110, rect.y+rect.height + 10, 100, 30), "Add object layer")) 
		{
			TileLayerEditorAmmendum.CreateObjectLayer();
		}
	}
	
}
