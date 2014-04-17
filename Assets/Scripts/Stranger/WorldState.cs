using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class WorldState : MonoBehaviour
{

    State curPlayerState;
    public State worldState = State.All;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        curPlayerState = GameObject.FindWithTag("Player").GetComponent<PlayerState>().CurrentState;
        if (curPlayerState != worldState && worldState != State.All)
        {
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.renderer.enabled = false;
            }

            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.renderer.enabled = false;
            }



            Rigidbody2D[] rBodies = GetComponentsInChildren<Rigidbody2D>();

            foreach (Rigidbody2D b in rBodies)
            {
                b.gravityScale = 0;
            }

        }
        else
        {


            Rigidbody2D[] rBodies = GetComponentsInChildren<Rigidbody2D>();

            foreach (Rigidbody2D b in rBodies)
            {
                b.gravityScale = 1;
            }

            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.renderer.enabled = true;
            }

            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.renderer.enabled = true;
            }
        }


    }
}
