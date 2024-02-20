using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private LineRenderer line;
    public float startX, startY;
    public int rowCount, colCount;
    public float gridSize = 1;

    private void InitLineRenderer(LineRenderer lr)
    {
        //lr.startWidth = lr.endWidth = 0.01f;
        lr.material.color = Color.white;
    }


    private void DrawGrid(LineRenderer lr, float row, float col, int rowCount, int colCount)
    {
        List<Vector3> gridPos = new List<Vector3>();
        float ec = col + colCount * gridSize;
        gridPos.Add(new Vector3(row, col, transform.position.z));
        gridPos.Add(new Vector3(row, ec, transform.position.z));

        int toggle = -1;
        Vector3 currentPos = new Vector3(row, ec, transform.position.z);

        for (int i = 0; i < rowCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.x += gridSize;
            gridPos.Add(nextPos);


            nextPos.y += (colCount * toggle * gridSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        currentPos.x = row;
        gridPos.Add(currentPos);

        int colToggle = toggle = 1;
        if (currentPos.y == ec) colToggle = -1;

        for(int i = 0; i< colCount; i++)
        {
            Vector3 nextPos = currentPos;
            nextPos.y += (colToggle * gridSize);
            gridPos.Add(nextPos);

            nextPos.x += (rowCount * toggle * gridSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        lr.positionCount = gridPos.Count;
        lr.SetPositions(gridPos.ToArray());

    }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        InitLineRenderer(line);
        DrawGrid(line, startX, startY, rowCount, colCount);
    }

    private void Update()
    {
        if(TestManager.instance.isEditMode)
        {
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }
}
