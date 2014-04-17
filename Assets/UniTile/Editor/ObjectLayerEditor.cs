// Copyright 2011 Sven Magnus

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(ObjectLayer))]

public class ObjectLayerEditor : Editor {
	
	
	Vector3 selectionOffset;
	Vector3 halfPoint;
	
	bool showProperties = true;
	
	void OnEnable() {
#if UNITY_3_5
		Tools.current = Tool.View;
#endif
		
		UniTileManager manager = UniTileManager.instance;
		ObjectLayer layer = target as ObjectLayer;
		if(layer == null) return;
		manager.activeObjectLayer = layer;
	}
	
	void OnDisable() {
		UniTileManager manager = UniTileManager.instance;
		manager.activeObjectLayer = null;
	}
	
	public override void OnInspectorGUI () {
		
		
		EditorGUILayout.BeginVertical("box");
		this.DrawDefaultInspector();
		EditorGUILayout.EndVertical();
		
#if UNITY_3_5
		Tools.current = Tool.View;
#endif
		ObjectLayer layer = target as ObjectLayer;
		
		EditorGUILayout.BeginVertical("box");
		layer.gridSize = TileLayerEditor_LayerProperties.Vector2Field ("Grid Size", layer.gridSize);
		layer.layerSize = TileLayerEditor_LayerProperties.Vector2Field ("Layer Size", layer.layerSize);
		layer.gridOffset = TileLayerEditor_LayerProperties.Vector2Field ("Snap Offset", layer.gridOffset);
		layer.snapToGrid = EditorGUILayout.Toggle("Snap to grid", layer.snapToGrid);
		EditorGUILayout.EndVertical();
		
		Rect rect = GUILayoutUtility.GetLastRect();
		
		EditorGUILayout.BeginVertical("box");
		
		
		
		int x = 10;
		int y = (int)rect.yMax + 10;
		
		
		Rect box = new Rect(rect.x, y - 5, rect.width, 10);
		bool inc = false;
		if(layer.prefabs!=null) {
			for(int i = 0; i<layer.prefabs.Length; i++) {
				if(layer.prefabs[i]!=null) {
					inc = true;
					if (GUI.Button(new Rect(x, y, 100, 30), new GUIContent(layer.prefabs[i].name))) {
						Undo.RegisterSceneUndo("Add object "+layer.prefabs[i].name);
						//Hack for backwards compatibility
#if (UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9)
						Object o = PrefabUtility.InstantiatePrefab(layer.prefabs[i]);
#else
						Object o = EditorUtility.InstantiatePrefab(layer.prefabs[i]);
#endif
						GameObject g = o as GameObject;
						if(g == null) g = ((Transform)o).gameObject;
						Transform t = g.transform;
						t.parent = layer.transform;
						t.localPosition = new Vector3(halfPoint.x, halfPoint.y, 0);
						layer.selected = t;
					}
					x+=105;
					if(x+100>Screen.width - 20) {
						inc = false;
						x=10;
						y+=30;
						box.height+=30;
					}
				}
			}
			if(inc) box.height+=30;
			
		}
		GUILayout.Label("", GUILayout.Height(box.height));
		EditorGUILayout.EndVertical();
		
		if(layer.selected != null) {
			EditorGUILayout.BeginVertical("box");
			showProperties = EditorGUILayout.Foldout(showProperties, "Selected object");
			if(showProperties) {
				
				
				layer.selected.name = EditorGUILayout.TextField("Name", layer.selected.name);
				layer.selected.localPosition = TileLayerEditor_LayerProperties.Vector3Field("Object position", layer.selected.localPosition);
				layer.selected.localScale = TileLayerEditor_LayerProperties.Vector3Field("Object scale", layer.selected.localScale);
				
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.PrefixLabel(" ");
				
				if (GUILayout.Button(new GUIContent("Go to selected object"), GUILayout.Height(30))) {
					Object[] sel = new Object[1];
					sel[0] = layer.selected.gameObject;
					Selection.objects = sel;
				}
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.EndVertical();
			//y+=40;
			
			
		}
	}
	
	
	void OnSceneGUI() {
		
#if UNITY_3_5
		Tools.current = Tool.View;
#endif
		ObjectLayer layer = target as ObjectLayer;
		
		this.DrawHandles();
		
		if(Event.current.type == EventType.keyDown) {
			Vector3 p1 = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector3(0, 0, -SceneView.lastActiveSceneView.camera.transform.position.z));
			Vector3 p2 = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector3(64, 0, -SceneView.lastActiveSceneView.camera.transform.position.z));
			float d = p2.x - p1.x;
			if(Event.current.keyCode == KeyCode.UpArrow) {
				SceneView.lastActiveSceneView.pivot += SceneView.lastActiveSceneView.camera.transform.TransformDirection(new Vector3(0, d, 0));
				Event.current.Use();
			}
			if(Event.current.keyCode == KeyCode.DownArrow) {
				SceneView.lastActiveSceneView.pivot += SceneView.lastActiveSceneView.camera.transform.TransformDirection(new Vector3(0, -d, 0));
				Event.current.Use();
			}
			if(Event.current.keyCode == KeyCode.LeftArrow) {
				SceneView.lastActiveSceneView.pivot += SceneView.lastActiveSceneView.camera.transform.TransformDirection(new Vector3(-d, 0, 0));
				Event.current.Use();
			}
			if(Event.current.keyCode == KeyCode.RightArrow) {
				SceneView.lastActiveSceneView.pivot += SceneView.lastActiveSceneView.camera.transform.TransformDirection(new Vector3(d, 0, 0));
				Event.current.Use();
			}
		}
		
		if(layer == null) return;
		UniTileManager manager = UniTileManager.instance;
		if(manager == null) return;
		
		if(EditorWindow.mouseOverWindow) {
			Vector3 pos = GetCoordinates();
			
			if(Event.current.type==EventType.mouseDown) {
				if(Event.current.button==0) {
					layer.selected = this.Select(pos);
					
					if(layer.selected != null && Event.current.modifiers.ToString()=="Control") {
						Undo.RegisterSceneUndo("Delete object "+layer.selected.name);
						DestroyImmediate(layer.selected.gameObject);
						layer.selected = null;
						
					}
				}
			}
			
			if(Event.current.type==EventType.mouseUp) {
				if(Event.current.button==0) {
					
						if(layer.selected != null && layer.snapToGrid) {
							Undo.RegisterSceneUndo("Move "+layer.selected.name);
							layer.selected.transform.localPosition = new Vector3(Mathf.Round((pos.x - layer.gridOffset.x - selectionOffset.x)/layer.gridSize.x) * layer.gridSize.x + layer.gridOffset.x, Mathf.Round((pos.y - layer.gridOffset.y - selectionOffset.y)/layer.gridSize.y) * layer.gridSize.y + layer.gridOffset.y, layer.selected.transform.localPosition.z);
						}
						this.Repaint();
						
					
				}
			}
			
			if(Event.current.type==EventType.dragPerform || Event.current.type==EventType.DragExited || Event.current.type==EventType.dragUpdated || Event.current.type==EventType.mouseDrag) {
				if(Event.current.button==0 && layer.selected != null) {
					Event.current.Use();
					Undo.RegisterSceneUndo("Move "+layer.selected.name);
					layer.selected.transform.localPosition = new Vector3(pos.x, pos.y, layer.selected.transform.localPosition.z) - selectionOffset;
				}
				
			}
			
			
			HandleUtility.Repaint();
		}
		
		halfPoint = GetCoordinates(new Vector2(Screen.width / 2, Screen.height / 2));
	}
	
	void DrawHandles() {
		ObjectLayer layer = this.target as ObjectLayer;
		
		Handles.color = new Color (1f,1f,1f,1f);
		
		if(this != null) {
			Transform trans = layer.transform;
			Handles.DrawLine(trans.TransformPoint(new Vector3(0,0,0)), trans.TransformPoint(new Vector3(layer.gridSize.x * layer.layerSize.x, 0, 0)));
			Handles.DrawLine(trans.TransformPoint(new Vector3(0,0,0)), trans.TransformPoint(new Vector3(0, layer.gridSize.y * layer.layerSize.y, 0)));
			Handles.DrawLine(trans.TransformPoint(new Vector3(0, layer.gridSize.y * layer.layerSize.y, 0)), trans.TransformPoint(new Vector3(layer.gridSize.x * layer.layerSize.x, layer.gridSize.y * layer.layerSize.y, 0)));
			Handles.DrawLine(trans.TransformPoint(new Vector3(layer.gridSize.x * layer.layerSize.x, layer.gridSize.y * layer.layerSize.y,0)), trans.TransformPoint(new Vector3(layer.gridSize.x * layer.layerSize.x, 0, 0)));
			Handles.color = new Color (1f,1f,1f,0.05f);
			for(int i=0;i<=layer.layerSize.y;i++) {
				Handles.DrawLine(trans.TransformPoint(new Vector3(0, (float)i * layer.gridSize.y, 0)), trans.TransformPoint(new Vector3(layer.gridSize.x * layer.layerSize.x, (float)i * layer.gridSize.y, 0)));
			}
			for(int i=0;i<=layer.layerSize.x;i++) {
				Handles.DrawLine(trans.TransformPoint(new Vector3((float)i * layer.gridSize.x, 0, 0)), trans.TransformPoint(new Vector3((float)i * layer.gridSize.x, layer.gridSize.y * layer.layerSize.y, 0)));
			}
			Handles.color = new Color (1f,1f,1f,1f);
		}
		
		Transform t;
		for(int i = 0; i<layer.transform.childCount; i++) {
			t = layer.transform.GetChild(i);
			Bounds b = layer.FindBounds(t);
			
			Vector3[] verts = new Vector3[4];
			
			verts[0] = new Vector3(b.center.x - b.size.x / 2f, b.center.y - b.size.y / 2f, b.center.z);
			verts[1] = new Vector3(b.center.x - b.size.x / 2f, b.center.y + b.size.y / 2f, b.center.z);
			verts[2] = new Vector3(b.center.x + b.size.x / 2f, b.center.y + b.size.y / 2f, b.center.z);
			verts[3] = new Vector3(b.center.x + b.size.x / 2f, b.center.y - b.size.y / 2f, b.center.z);
			
			
			Handles.DrawSolidRectangleWithOutline(verts, new Color (1f,0f,0f,layer.selected == t?0.2f:0.05f), new Color (1f,0,0,1f));
		}
	}
	
	
	private Vector3 GetCoordinates() {
		return GetCoordinates(Event.current.mousePosition);
	}
	
	private Vector3 GetCoordinates(Vector2 pos) {
		Plane p = new Plane((this.target as MonoBehaviour).transform.TransformDirection(Vector3.forward), (this.target as MonoBehaviour).transform.position);
		Ray ray = HandleUtility.GUIPointToWorldRay(pos);
		
        Vector3 hit = new Vector3();
        float dist;
		
		if (p.Raycast(ray, out dist))
        	hit = ray.origin + ray.direction.normalized * dist;
		
		return (this.target as MonoBehaviour).transform.InverseTransformPoint(hit);
	}
	
	Transform Select(Vector3 pos) {
		ObjectLayer layer = target as ObjectLayer;
		Transform t;
		Bounds b;
		bool keepSelection = false;
		Vector3 pos2 = (this.target as MonoBehaviour).transform.TransformPoint(pos);
		
		for(int i = 0; i<layer.transform.childCount; i++) {
			t = layer.transform.GetChild(i);
			b = layer.FindBounds(t);
			if(t.gameObject.active) {
				pos2.z = pos.z = t.position.z;
				if(b.Contains(pos2)) {
					if(layer.selected == t) {
						keepSelection = true;
						continue;
					}
					selectionOffset = pos - t.localPosition;
					selectionOffset.z = 0;
					return t;
				}
			}
		}
		if(keepSelection) {
			selectionOffset = pos - layer.selected.localPosition;
			selectionOffset.z = 0;
			return layer.selected;
		}
		return null;
	}
	
}
