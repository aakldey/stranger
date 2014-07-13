using UnityEditor;
using UnityEngine;
using System.Collections;

public static class TileLayerEditorAmmendum
{
	static bool keepRealtimeChanges = false;
	static bool wasPlayingLastUpdate = false;
	static Hashtable playModeTiles = new Hashtable();
	
	[MenuItem ("Edit/UniTile/Select Last Active Layer &l")]
	public static void SelectLastActiveLayer()
	{
		if(UniTileManager.instance == null)
			return;
		
		if(UniTileManager.instance.activeLayer)
		{
			Selection.activeObject = UniTileManager.instance.activeLayer.gameObject;
			return;
		}
		
		if(UniTileManager.instance.lastLayer)
		{
			Selection.activeObject = UniTileManager.instance.lastLayer.gameObject;
			return;
		}
		
		TileLayer layer = (TileLayer)GameObject.FindObjectOfType(typeof(TileLayer));
		Selection.activeObject = layer != null ? layer.gameObject:null;
	}
	
	public static void OnSceneGUI_Ammendum(TileLayer layer, UniTileTemplate manager, TileLayerEditor editor)
	{
		Handles.BeginGUI();

		GUILayout.BeginArea(new Rect(20, 40, 150, 40));
		keepRealtimeChanges = GUILayout.Toggle(keepRealtimeChanges, "Keep Playmode Changes", "Button");
		GUILayout.EndArea();
			
		Handles.EndGUI();
		
		TrackPlaymodeChanges(layer, editor);
	}
	
	private static void TrackPlaymodeChanges(TileLayer layer, TileLayerEditor editor)
	{
		TileLayer[] layers = (TileLayer[])GameObject.FindObjectsOfType(typeof(TileLayer));
		if(Application.isPlaying && keepRealtimeChanges)
		{
			playModeTiles[layer] = layer.tileData.Clone();
		}
		else 
		{
			if(wasPlayingLastUpdate && keepRealtimeChanges && !Application.isPlaying)
			{
				foreach(TileLayer l in layers)
				{
					if(playModeTiles.Contains(l))
					{
						l.tileData = (TileInstance[])playModeTiles[l];
						editor.RebuildMap(l.layerSize, l.resizeMode);
						TileLayerEditor.InstantiatePrefabs(l);
					}
				}
				
				Undo.RegisterSceneUndo("Playmode changes to tile layers");
			}
			
			playModeTiles.Clear();
		}
		wasPlayingLastUpdate = Application.isPlaying;
	}
	
	[MenuItem ("GameObject/Create Other/UniTile/Create Tile Layer", false, int.MinValue + 20)]
	public static void CreateTileLayer() {
		UniTileManager.instance.layerCount++;
		GameObject g = new GameObject("Layer " + UniTileManager.instance.layerCount);
		TileLayer tl = g.AddComponent<TileLayer>();
		if(UniTileManager.instance.lastLayer!=null) {
			tl.material = UniTileManager.instance.lastLayer.material;
			tl.tileset = UniTileManager.instance.lastLayer.tileset;
			tl.tileSize = UniTileManager.instance.lastLayer.tileSize;
			tl.tileSpacing = UniTileManager.instance.lastLayer.tileSpacing;
			tl.tileUvSize = UniTileManager.instance.lastLayer.tileUvSize;
			tl.overlap = UniTileManager.instance.lastLayer.overlap;
			tl.overlapUv = UniTileManager.instance.lastLayer.overlapUv;
			tl.groupSize = UniTileManager.instance.lastLayer.groupSize;
			tl.layerSize = UniTileManager.instance.lastLayer.layerSize;
			tl.borderSize = UniTileManager.instance.lastLayer.borderSize;
			tl.borderSizeUv = UniTileManager.instance.lastLayer.borderSizeUv;
			tl.resizeMode =  UniTileManager.instance.lastLayer.resizeMode;
		}
		UniTileManager.instance.lastLayer = tl;
		Selection.activeObject = g;
	}
	
	[MenuItem ("GameObject/Create Other/UniTile/Create Object Layer", false, int.MinValue + 21)]
	public static void CreateObjectLayer() {
		UniTileManager.instance.objectLayerCount++;
		GameObject g = new GameObject("Object Layer " + UniTileManager.instance.objectLayerCount);
		g.AddComponent<ObjectLayer>();
		Selection.activeObject = g;
	}
}