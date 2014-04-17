using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class SnapToGrid : ScriptableObject
{
    [MenuItem ("Edit/Snap to Grid %g")]
    static void MenuSnapToGrid()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
       
        float gridx = EditorPrefs.GetFloat("MoveSnapX");
        float gridy = EditorPrefs.GetFloat("MoveSnapY");
        float gridz = EditorPrefs.GetFloat("MoveSnapZ");
       
        foreach (Transform transform in transforms)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            transform.position = newPosition;
        }
    }

    [MenuItem("Edit/Change World State %t")]
    static void ChangeWorldState()
    {
        State state = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().CurrentState;

        switch (state)
        {
            case State.World1:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().CurrentState = State.World2;
                break;
            case State.World2:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().CurrentState = State.World1;
                break;
        }
    }
}