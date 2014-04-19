using UnityEngine;
using System.Collections;

public class AcidEffect : MonoBehaviour {

    public float shift;
    public float blurStep;
    FXPostProcess post;
	// Use this for initialization
	void Start () {

        post = GetComponent<FXPostProcess>();
	}
	
	// Update is called once per frame
	void Update () {

       // post.Material.SetSetFloat("Shift", shift);
	}
}
