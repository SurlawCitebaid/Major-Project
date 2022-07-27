using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public Material m_Material;
    private GameObject line;
    private LineRenderer lr;
    public void DrawLine(Vector3 start, Vector3 end, Transform parent)
    {
        line = new GameObject();
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();
        lr.material = m_Material;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.1f;
        lr.endWidth =0.1f ;
        lr.SetPosition(0,start);
        lr.SetPosition(1, end);
        lr.sortingOrder = 8;
        line.transform.parent = parent;
    }
    public void changeAlpha(bool num)
    {
        if(num)
        {
            lr.startColor = Color.clear;
            lr.endColor = Color.clear;
        } else
        {
            lr.startColor = Color.red;
            lr.endColor = Color.red;
        }
    }
    public void changeSortingOrder(int num)
    {
        lr.sortingOrder = num;
    }
    public void moveLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    public void updateStartPoint (Vector3 ass)
    {
        lr.SetPosition(0, ass);
    }
    public void destroyLine()
    {
        lr.enabled = false;
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
