using UnityEngine;
using System.Collections;

public class TriggerOffCallback : MonoBehaviour {

    public Collider2D toSecond;
    public Collider2D toFirst;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "ToSecond")
        {
            toSecond = collider;
        }

        if (collider.tag == "ToFirst")
        {
            toFirst = collider;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "ToSecond")
        {
            toSecond = null;
        }

        if (collider.tag == "ToFirst")
        {
            toFirst = null;
        }
    }
}
