using UnityEngine;
using System.Collections;

public class BlurController : MonoBehaviour {

	public float blurRange = 0.0f;
	public float blurSteps = 0.0f;

	public FXPostProcess blur;

	void Start () 
	{
	
	}

	void Update () 
	{
		blur.Material.SetFloat ("_BlurRange", blurRange);
		blur.Material.SetFloat ("_BlurSteps", blurSteps);
	}
}
