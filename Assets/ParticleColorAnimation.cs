using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class ParticleColorAnimation : MonoBehaviour {

    public Camera camera;
    public Color color;
	// Use this for initialization
	void Start () {

        camera = GetComponent<Camera>();
        camera.backgroundColor = color;
	
	}
	
	// Update is called once per frame
	void Update () {

		camera.backgroundColor = color;
	
	}
}
