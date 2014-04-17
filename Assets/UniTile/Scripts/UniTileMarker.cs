using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UniTileMarker : MonoBehaviour {
	
	public MeshRenderer meshRenderer;
	public MeshFilter meshFilter;
	
	
	//public bool flippedHorizontally = false;
	//public bool flippedVertically = false;
	//public uint rotation = 0;
	
	TileLayer layer;
	
	public static UniTileMarker CreateMarker(TileLayer layer) {
		Transform t = layer.transform.FindChild("Marker");
		while(t != null) {
			UniTileMarker oldMarker = t.GetComponent<UniTileMarker>();
			if(oldMarker == null) {
				DestroyImmediate(t.gameObject);
			} else {
				oldMarker.Destroy();
			}
			t = layer.transform.FindChild("Marker");
		}
		
		GameObject go = new GameObject("Marker");
		go.transform.parent = layer.transform;
		go.transform.localScale = new Vector3(1, 1, 1);
		Quaternion quaternion = go.transform.localRotation;
		quaternion.eulerAngles = new Vector3(0, 0, 0);
		go.transform.localRotation = quaternion;
		go.tag = "EditorOnly";
		UniTileMarker marker = go.AddComponent<UniTileMarker>();
		marker.meshRenderer = go.AddComponent<MeshRenderer>();
		marker.meshFilter = go.AddComponent<MeshFilter>();
		marker.layer = layer;
		return marker;
	}
	
	public void Start() {
		if(this.layer == null) this.Destroy();
	}
	
	public void Destroy() {
		if(this.meshFilter != null && this.meshFilter.sharedMesh != null) DestroyImmediate(this.meshFilter.sharedMesh);
		DestroyImmediate(this.gameObject);
	}
	
	public void Init(UniTileTemplate template) {
		if(this.meshFilter.sharedMesh!=null) DestroyImmediate(this.meshFilter.sharedMesh);
		
		if(layer.material==null || layer.material.mainTexture == null) return;
		
		if(layer.tileSpacing == new Vector2(-1, -1)) layer.tileSpacing = layer.tileSize;
		
		this.meshRenderer.material = layer.material;
		
		Mesh m = new Mesh();
		
		int count = template.selectedTilesList.Length;
		
		Vector3 [] vertices = new Vector3[4 * count];
		int [] triangles = new int[6 * count];
		Vector2 [] uv = new Vector2[4 * count];
		
		for(int i=0;i<count;i++) {
			int tileX = i % template.selectedTilesWidth;
			int tileY = (int)Mathf.Floor(i / template.selectedTilesWidth);
			int x = (int)((tileX)*layer.tileSpacing.x);
			int y = (int)((-tileY)*layer.tileSpacing.y);
			
			// Triangles
			triangles[(i*6) + 0] = (i*4) + 0;
			triangles[(i*6) + 1] = (i*4) + 1;
			triangles[(i*6) + 2] = (i*4) + 2;
			triangles[(i*6) + 3] = (i*4) + 1;
			triangles[(i*6) + 4] = (i*4) + 3;
			triangles[(i*6) + 5] = (i*4) + 2;
			
			if(template.selectedTilesList[i] != null && template.selectedTilesList[i].id != -1) {
				
				vertices[(i*4) + 0] = new Vector3(0 + (int)x, layer.tileSpacing.y + (int)y, 0);
				vertices[(i*4) + 1] = new Vector3(layer.tileSpacing.x + (int)x, layer.tileSpacing.y + (int)y, 0);
				vertices[(i*4) + 2] = new Vector3(0 + (int)x, 0 + (int)y, 0);
				vertices[(i*4) + 3] = new Vector3(layer.tileSpacing.x + (int)x, 0 + (int)y, 0);
				
				int columns = (int)(layer.material.mainTexture.width / (layer.tileSize.x + layer.borderSize.x * 2f));
				float uvx = Mathf.Floor((int)template.selectedTilesList[i].id % columns) * (layer.tileSize.x + layer.borderSize.x * 2f) + layer.borderSize.x;
				float uvy = Mathf.Floor((int)template.selectedTilesList[i].id / columns) * (layer.tileSize.x + layer.borderSize.y * 2f) + layer.borderSize.y;
				float uvx2 = uvx+layer.tileSize.x;
				float uvy2 = uvy+layer.tileSize.y;
				
				int id0 = (i*4) + 0;
				int id1 = (i*4) + 1;
				int id2 = (i*4) + 2;
				int id3 = (i*4) + 3;
				
				if(template.selectedTilesList[i].flippedHorizontally) {
					int temp = id0;
					id0 = id1;
					id1 = temp;
					temp = id2;
					id2 = id3;
					id3 = temp;
				}
				
				if(template.selectedTilesList[i].flippedVertically) {
					int temp = id0;
					id0 = id2;
					id2 = temp;
					temp = id1;
					id1 = id3;
					id3 = temp;
				}
				
				for(int j = 0; j < (uint)template.selectedTilesList[i].rotation; j++) {
					int temp = id0;
					id0 = id1;
					id1 = id3;
					id3 = id2;
					id2 = temp;
					
				}
				
				// UVS
				uv[id0] = new Vector2(uvx / (float)layer.material.mainTexture.width, 1f - uvy / (float)layer.material.mainTexture.height);		//TL
				uv[id1] = new Vector2(uvx2 / (float)layer.material.mainTexture.width, 1f - uvy / (float)layer.material.mainTexture.height);		//TR
				uv[id2] = new Vector2(uvx / (float)layer.material.mainTexture.width, 1f - uvy2 / (float)layer.material.mainTexture.height);		//BL
				uv[id3] = new Vector2(uvx2 / (float)layer.material.mainTexture.width, 1f - uvy2 / (float)layer.material.mainTexture.height);	//BR
			}
			
			
		}
		
		m.vertices = vertices;
		m.triangles = triangles;
		m.uv = uv;
		
		m.RecalculateNormals();
		
		this.meshFilter.mesh = m;
		
	}
}
