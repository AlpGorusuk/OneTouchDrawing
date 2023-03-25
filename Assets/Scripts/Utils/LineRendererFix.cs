using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererFix : MonoBehaviour
{
    [ContextMenu("Update LineRenderer NodePos")]
    public void UpdateLineRendererNodePos()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Debug.Log(lineRenderer.positionCount);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 temp = lineRenderer.GetPosition(i);
            int xRounded = Mathf.RoundToInt(temp.x);
            int yRounded = Mathf.RoundToInt(temp.y);
            int zRounded = 0;
            temp = new Vector3(xRounded, yRounded, zRounded);
            lineRenderer.SetPosition(i, temp);
        }
    }
}
