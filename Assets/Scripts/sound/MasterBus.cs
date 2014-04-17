using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MasterBus : MonoBehaviour {

    public List<AudioSource> list = new List<AudioSource>();
	// Use this for initialization
	void Start () {
        foreach (AudioSource s in GetComponentsInChildren<AudioSource>())
        {
            list.Add(s);
           
        }
        list.Reverse();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
