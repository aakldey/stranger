using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetMeshRendererLayer : MonoBehaviour {
	public string LayerName;
	public int layer = 0;
	// Use this for initialization
	void Start () {
		foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>()) {
			mesh.renderer.sortingLayerName = LayerName;
			//mesh.renderer.sortingLayerID = LayerMask.NameToLayer(LayerName);
			mesh.renderer.sortingOrder = layer;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
