using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BoxCollider2D))]
public class BoxColliderEditor : Editor  {

    private Vector3 handlePosition;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
       
    }

    public void OnSceneGUI()
    {
        BoxCollider2D box = target as BoxCollider2D;
        if (box)
        {
            if (box.tag == "ToFirst" || box.tag == "ToSecond")
            {
                Quaternion rot = Quaternion.identity;
                float size = 1;
                //  box.center = Handles.FreeMoveHandle(box.center, rot, size, Vector3.zero, Handles.SphereCap);
                Vector3 pos = box.transform.position;
                Vector3 initpos = pos;
                pos.x += box.size.x;
                pos.y += box.size.y;


                handlePosition = Handles.FreeMoveHandle(pos, rot, size, Vector3.zero, Handles.SphereCap);


                float sizex = handlePosition.x - initpos.x;
                float sizey = handlePosition.y - initpos.y;
               // sizex *= 2;
               // sizey *= 2;

                Vector3 result = new Vector3(sizex, sizey, 20);
                box.size = result;
                box.center = new Vector2(sizex / 2, sizey/2);
            }
  
        }



        // если мы двигали контрольные точки, то мы должны указать редактору,
        // что объект изменился (стал "грязным")
        if (GUI.changed)
            EditorUtility.SetDirty(target);

    }
 
}
