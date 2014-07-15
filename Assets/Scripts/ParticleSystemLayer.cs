using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleSystemLayer : MonoBehaviour {

	public string layerName;

	public int order;

	// Use this for initialization
	void Start () {
		particleSystem.renderer.sortingLayerName = layerName;
		particleSystem.renderer.sortingOrder = order
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
