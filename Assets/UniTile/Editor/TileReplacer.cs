// Copyright 2011 Sven Magnus

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TileReplacer {
	
	public TileLayer layer;
	public TileReplacerSet replacerSet;
	
	public Vector2 scrollPos;
	
	public TileLayerEditor editor;
	
	public TileReplacer(TileLayer layer, TileLayerEditor editor) {
		this.layer = layer;
		this.editor = editor;
		
		if(layer.replacerGUID != "") {
			this.LoadSet(layer.replacerGUID);
		}
		
	}
	
	public void CreateSet() {
		string path = EditorUtility.SaveFilePanel("Create a new Replacer Set", AssetDatabase.GetAssetPath(this.layer.tileset), this.layer.tileset.name.Replace(".Tileset", ".ReplacerSet"), "asset").Replace("\\", "/");
		if(path == "" || path == null) return;
		
		if(!path.Contains(Application.dataPath.Replace("\\", "/"))) {
			if(EditorUtility.DisplayDialog("Save the Replacer Set in the Assets folder", "The Replacer Set needs to be saved inside the Assets folder of your project.", "Ok", "Cancel")) {
				CreateSet();
			}
		} else {
			
			path = path.Replace(Application.dataPath.Replace("\\", "/"), "Assets");
					
					
			this.replacerSet = ScriptableObject.CreateInstance<TileReplacerSet>();
			this.replacerSet.tileset = this.layer.tileset;
			AssetDatabase.CreateAsset(this.replacerSet, path);
			
			layer.replacerGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this.replacerSet));
		}
		
	}
	
	public void LoadSet(string guid) {
		this.replacerSet = (TileReplacerSet)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(TileReplacerSet));
	}
	
	
	public void OnGUI() {
		
		if(this.layer.material == null) {
			EditorGUILayout.Space();
			GUILayout.Label("No material selected.", GUILayout.ExpandWidth(false));
			EditorGUILayout.Space();
			return;
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		if(this.replacerSet == null) {
			GUILayout.Label("No set selected.", GUILayout.ExpandWidth(false));
		} else if (this.replacerSet.tileset != this.layer.tileset) {
			GUILayout.Label("Replacer Set is for a different tileset.", GUILayout.ExpandWidth(false));
		}
		
		TileReplacerSet rs = (TileReplacerSet)EditorGUILayout.ObjectField(this.replacerSet, typeof(TileReplacerSet), false);
		if(rs != this.replacerSet) {
			this.replacerSet = rs;
			if(rs == null) {
				layer.replacerGUID = "";
			} else {
				layer.replacerGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(rs));
			}
		}
		
		if(GUILayout.Button("Create Set", GUILayout.ExpandWidth(false))) {
			this.CreateSet();
		}
		if(this.replacerSet != null && this.replacerSet.tileset == this.layer.tileset) {
			if(GUILayout.Button("Add Rule", GUILayout.ExpandWidth(false))) {
				this.replacerSet.rules.Add(new TileReplacerRule("New rule"));
			}
			if(GUILayout.Button("Start", GUILayout.ExpandWidth(false))) {
				this.Replace();
			}
		}
		EditorGUILayout.EndHorizontal();
		
		
		if(this.replacerSet != null && this.replacerSet.tileset == this.layer.tileset) {
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Area", GUILayout.Width(60));
			this.replacerSet.entireMap = EditorGUILayout.Toggle(this.replacerSet.entireMap, GUILayout.Width(20));
			GUILayout.Label("Entire Map");
			EditorGUILayout.EndHorizontal();
			
			if(!this.replacerSet.entireMap) {
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Position", GUILayout.Width(60));
				Vector2 pos = new Vector2(
					EditorGUILayout.IntField((int)replacerSet.area.x),
					EditorGUILayout.IntField((int)replacerSet.area.y)
				);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Size", GUILayout.Width(60));
				Vector2 size = new Vector2(
					EditorGUILayout.IntField((int)replacerSet.area.width),
					EditorGUILayout.IntField((int)replacerSet.area.height)
				);
				EditorGUILayout.EndHorizontal();
				
				this.replacerSet.area = new Rect(pos.x, pos.y, size.x, size.y);
			}
			
			
			for(int i = 0; i < this.replacerSet.rules.Count; i++) {
				this.ShowRule(this.replacerSet.rules[i]);
			}
			
			
			
			if(GUI.changed) this.SaveSet();
			
		}
		
		EditorGUILayout.Space();
	}
	
	public void OnSceneGUI() {
		if(this.replacerSet == null) return;
		Transform trans = this.layer.transform;
		Handles.color = Color.red;
		
		Rect rect = new Rect(0, 0, layer.layerSize.x, layer.layerSize.y);
		
		if(!this.replacerSet.entireMap) {
			rect = this.replacerSet.area;
		}
		Handles.DrawLine(trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.x, layer.tileSpacing.y * rect.y, 0)), trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.xMax, layer.tileSpacing.y * rect.y, 0)));
		Handles.DrawLine(trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.x, layer.tileSpacing.y * rect.yMax, 0)), trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.xMax, layer.tileSpacing.y * rect.yMax, 0)));
		Handles.DrawLine(trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.x, layer.tileSpacing.y * rect.y, 0)), trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.x, layer.tileSpacing.y * rect.yMax, 0)));
		Handles.DrawLine(trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.xMax, layer.tileSpacing.y * rect.y, 0)), trans.TransformPoint(new Vector3(layer.tileSpacing.x * rect.xMax, layer.tileSpacing.y * rect.yMax, 0)));
	}
	
	void ShowRule(TileReplacerRule rule) {
		rule.visible = EditorGUILayout.Foldout(rule.visible, rule.name);
		if(rule.visible) {
			
			EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
			
			EditorGUILayout.BeginHorizontal();
			rule.active = EditorGUILayout.Toggle(rule.active, GUILayout.Width(15));
			rule.name = EditorGUILayout.TextField(rule.name);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Chance %", GUILayout.Width(60));
			EditorGUI.BeginDisabledGroup(!rule.active);
			rule.chance = EditorGUILayout.Slider(rule.chance, 0, 100);
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Size", GUILayout.Width(60));
			Vector2 size = new Vector2(
				EditorGUILayout.IntField((int)rule.size.x),
				EditorGUILayout.IntField((int)rule.size.y)
			);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			
			int currentIndex = this.replacerSet.rules.IndexOf(rule);
			
			if(GUILayout.Button("Delete Rule")) {
				this.replacerSet.rules.Remove(rule);
			}
			if(GUILayout.Button("Duplicate")) {
				this.replacerSet.rules.Insert(currentIndex + 1, rule.Clone());
			}
			EditorGUI.BeginDisabledGroup(!rule.active);
			if(GUILayout.Button("Start Rule")) {
				Undo.RegisterSceneUndo("Replace Tiles");
				this.Replace(rule);
				
				for(int i=0;i<=Mathf.Floor(layer.layerSize.x/layer.groupSize.x);i++) {
					for(int j=0;j<=Mathf.Floor(layer.layerSize.y/layer.groupSize.y);j++) {
						TileLayerUtil.RedrawGroup(layer, i, j, editor.propertiesEditor.spriteLayer);
					}
				}
			}
			EditorGUI.EndDisabledGroup();
			
			EditorGUI.BeginDisabledGroup(currentIndex == 0);
			if(GUILayout.Button("Up")) {
				this.replacerSet.rules[currentIndex] = this.replacerSet.rules[currentIndex - 1];
				this.replacerSet.rules[currentIndex - 1] = rule;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUI.BeginDisabledGroup(currentIndex == this.replacerSet.rules.Count - 1);
			if(GUILayout.Button("Down")) {
				this.replacerSet.rules[currentIndex] = this.replacerSet.rules[currentIndex + 1];
				this.replacerSet.rules[currentIndex + 1] = rule;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			
			if(size != rule.size) {
				size.x = Mathf.Max (1, Mathf.Round (size.x));
				size.y = Mathf.Max (1, Mathf.Round (size.y));
				rule.size = size;
				rule.source.Resize((int)size.x, (int)size.y);
				rule.destination.Resize((int)size.x, (int)size.y);
			}
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			
			this.ShowGrid(rule, rule.source);
			
			GUILayout.Label("->");
			
			this.ShowGrid(rule, rule.destination);
			
			GUILayout.FlexibleSpace();
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.Space();
			
			
		}
		EditorGUILayout.Space();
	}
	
	bool mouseDown;
	void ShowGrid(TileReplacerRule rule, UniTileTemplate template) {
		GUIStyle style = new GUIStyle("box");
		style.margin = new RectOffset(0, 0, 0, 0);
		
		EditorGUILayout.BeginVertical("box");
		
		GUILayout.Label(template.name);
		
		if(Event.current.type == EventType.MouseDown) {
			mouseDown = true;
		}
		if(Event.current.type == EventType.MouseUp) {
			mouseDown = false;
		}
		
		for(int y = 0; y < rule.size.y; y++) {
			
			EditorGUILayout.BeginHorizontal();
			
			for(int x = 0; x < rule.size.x; x++) {
				
				TileInstance t = template.selectedTilesList[x + y * template.selectedTilesWidth];
				GUILayout.Label("", style, GUILayout.Width(32), GUILayout.Height(32));
				
				Rect rect = GUILayoutUtility.GetLastRect();
				if(rect.Contains(Event.current.mousePosition)) {
					if(mouseDown) {
						t = template.selectedTilesList[x + y * template.selectedTilesWidth] = layer.selection.selectedTilesList[0].Clone();
					} else {
						t = layer.selection.selectedTilesList[0];
					}
				}
				
				if(t.id > -1 && this.layer.material != null && this.layer.material.mainTexture != null) {
					
					/*rect.x++;
					rect.y++;
					rect.width -= 2;
					rect.height -= 2;*/
					
					//int i = x + y * template.selectedTilesWidth;
					
					
					
					int columns = (int)(layer.material.mainTexture.width / (layer.tileSize.x + layer.borderSize.x * 2f));
					float uvx = (Mathf.Floor((int)t.id % columns) * (layer.tileSize.x + layer.borderSize.x * 2f) + layer.borderSize.x) / (float)layer.material.mainTexture.width;
					float uvy = 1f - (Mathf.Floor(((int)t.id / columns) + 1) * (layer.tileSize.x + layer.borderSize.y * 2f) + layer.borderSize.y) / (float)layer.material.mainTexture.height;
					float uvx2 = layer.tileSize.x / (float)layer.material.mainTexture.width;
					float uvy2 = layer.tileSize.y / (float)layer.material.mainTexture.height;
					
					Rect uvRect = new Rect(uvx, uvy, uvx2, uvy2);
					
					if(t.flippedHorizontally) {
						uvRect.x = uvRect.xMax;
						uvRect.width *= -1;
					}
					
					if(t.flippedVertically) {
						uvRect.y = uvRect.yMax;
						uvRect.height *= -1;
					}
					Matrix4x4 m = GUI.matrix;
					
					GUIUtility.RotateAroundPivot((int)t.rotation * 90, new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f));
	            	//GUI.matrix = TranslationMatrix(dz  - tmp - offset) * GUI.matrix;
					
					
					GUI.DrawTextureWithTexCoords(rect, this.layer.material.mainTexture, uvRect);
					
					GUI.matrix = m;
				}
				
			}
			
			EditorGUILayout.EndHorizontal();
			
			this.editor.Repaint();
		}
		
		if(GUILayout.Button("Paste Selection")) {
			layer.selection.CopyInto(template);
			GUI.changed = true;
		}
		EditorGUILayout.EndVertical();
	}
	
	private static Matrix4x4 TranslationMatrix(Vector3 v)
    {
 
        return Matrix4x4.TRS(v, Quaternion.identity, Vector3.one);
 
    }
	
	void Replace() {
		Undo.RegisterSceneUndo("Replace Tiles");
		for(int i = 0; i < this.replacerSet.rules.Count; i++) {
			this.Replace(this.replacerSet.rules[i]);
		}
		
		for(int i=0;i<=Mathf.Floor(layer.layerSize.x/layer.groupSize.x);i++) {
			for(int j=0;j<=Mathf.Floor(layer.layerSize.y/layer.groupSize.y);j++) {
				TileLayerUtil.RedrawGroup(layer, i, j, editor.propertiesEditor.spriteLayer);
			}
		}
	}
	
	void Replace(TileReplacerRule a) {
		if(!a.active) return;
		int xMin = 0;
		int yMin = 0;
		int xMax = (int)layer.layerSize.x;
		int yMax = (int)layer.layerSize.y;
		
		if(!replacerSet.entireMap) {
			xMin = Mathf.Max (0, (int)replacerSet.area.x);
			yMin = Mathf.Max (0, (int)layer.layerSize.y - (int)replacerSet.area.yMax);
			
			xMax = Mathf.Min (xMax, (int)replacerSet.area.xMax);
			yMax = Mathf.Min (yMax, (int)layer.layerSize.y - (int)replacerSet.area.y);
		}
		
		
		for(int i = xMin; i < xMax; i++) {
			for(int j = yMin; j < yMax; j++) {
				if(i + a.size.x <= xMax && j + a.size.y <= yMax) {
					if(this.Find(a, i, j)) {
						if(Random.Range(0f, 100f) <= a.chance) {
							this.DoReplace(a, i, j);
						}
					}
				}
			}
		}
	}
	
	
	bool Find(TileReplacerRule a, int x, int y) {
		for(int i = 0; i < a.size.x; i++) {
			for(int j = 0; j < a.size.y; j++) {
				TileInstance layerTile = layer.GetTileData(x + i, (int)layer.layerSize.y - 1 - (y + j));
				if(layerTile != null) {
					TileInstance templateTile = a.source.GetTileData(i, j);
					if(templateTile.id != layerTile.id || templateTile.flippedHorizontally != layerTile.flippedHorizontally || templateTile.flippedVertically != layerTile.flippedVertically || templateTile.rotation != layerTile.rotation) {
						return false;
					}
				} else {
					return false;
				}
			}
		}
		return true;
	}
	
	void DoReplace(TileReplacerRule a, int x, int y) {
		
		for(int i = 0; i < a.size.x; i++) {
			for(int j = 0; j < a.size.y; j++) {
				TileInstance layerTile = layer.GetTileData(x + i, (int)layer.layerSize.y - 1 - (y + j));
				if(layerTile != null) {
					TileInstance templateTile = a.destination.GetTileData(i, j);
					
					layerTile.id = templateTile.id;
					layerTile.flippedHorizontally = templateTile.flippedHorizontally;
					layerTile.flippedVertically = templateTile.flippedVertically;
					layerTile.rotation = templateTile.rotation;
				}
			}
		}
	}
	
	
	void SaveSet() {
		EditorUtility.SetDirty(this.replacerSet);
		AssetDatabase.SaveAssets();
		AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this.replacerSet));
	}
	
}
