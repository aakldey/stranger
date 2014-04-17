using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class ParticleColorAnimation : MonoBehaviour {

    public ParticleSystem particles;
    public Color color;
	// Use this for initialization
	void Start () {

        particles = GetComponent<ParticleSystem>();
        particles.startColor = color;
	
	}
	
	// Update is called once per frame
	void Update () {

        particles.startColor = color;
	
	}
}
