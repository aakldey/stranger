using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.


	void Start () 
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 2);
	}


	void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy...

            

        int layer;
        int l = GameObject.FindGameObjectWithTag("Player").layer;
        if (l == LayerMask.NameToLayer("Player1"))
        {
            layer = LayerMask.NameToLayer("World1");
        }
        else
        {
            layer = LayerMask.NameToLayer("World2");
        }

            if (col.gameObject.layer == layer) 
                {
                    if (col.tag == "Crate")
                    {
                        Debug.Log(GetComponent<Rigidbody2D>().velocity);
                        // ... find the Enemy script and call the Hurt function.
                        col.GetComponent<Rigidbody2D>().AddForce(100 * GetComponent<Rigidbody2D>().velocity);

                        // Call the explosion instantiation.
                        OnExplode();

                        // Destroy the rocket.
                        Destroy(gameObject);
                    }
                    else
                    {
                        OnExplode();

                        // Destroy the rocket.
                        Destroy(gameObject);
                    }
                }
	}
}
