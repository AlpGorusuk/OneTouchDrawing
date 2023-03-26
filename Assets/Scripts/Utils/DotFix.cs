using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DotFix : MonoBehaviour
{
    public Transform[] dots;
    [ContextMenu("Update DotPos")]
    public void UpdateDotPos()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            Vector3 temp = dots[i].position;
            int xRounded = Mathf.RoundToInt(temp.x);
            int yRounded = Mathf.RoundToInt(temp.y);
            int zRounded = 0;
            temp = new Vector3(xRounded, yRounded, zRounded);
            dots[i].position = temp;
        }
    }
    //When Object Create complited Work this for 2d collider replace
    [ContextMenu("Replace Dot Colliders 3D to 2D")]
    public void ReplaceDotColliderThreeToTwo()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            SphereCollider coll3d = dots[i].GetComponent<SphereCollider>();
            DestroyImmediate(coll3d);
            dots[i].AddComponent<CircleCollider2D>();
        }
    }
}
