using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public Material m_Material;
    private GameObject line;
    private LineRenderer lr;
    public void DrawLine(Vector3 start, Vector3 end)
    {
        line = new GameObject();
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();
        lr.material = m_Material;
        lr.startColor = Color.red;
        lr.startWidth = 0.1f;
        lr.endWidth =0.1f ;
        lr.SetPosition(0,start);
        lr.SetPosition(1, end);
    }
    public void updateStartPoint (Vector3 ass)
    {
        lr.SetPosition(0, ass);
    }
    public void destroyLine()
    {
        Destroy(line);
    }
    public void destroyLineAfterPeriod(float time)
    {
        Destroy(line, time);
    }
    public void setPos(int index, Vector3 position)
    {
        lr.SetPosition(index, position);
    }
}
