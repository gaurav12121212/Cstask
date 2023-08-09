using UnityEngine;
using System.Collections.Generic;

public class CircleInteraction : MonoBehaviour
{
    public GameObject circlePrefab;
    public LineRenderer lineRenderer;
    public UnityEngine.UI.Button restartButton;

    private List<GameObject> circles = new List<GameObject>();
    private List<Vector3> linePoints = new List<Vector3>();

    private void Start()
    {
        restartButton.onClick.AddListener(Restart);
        SpawnCircles(Random.Range(5, 11));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearLineAndPoints();
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            linePoints.Add(mousePos);
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
        }

        if (Input.GetMouseButtonUp(0))
        {
            RemoveIntersectedCircles();
            ClearLineAndPoints();
        }
    }

    private void SpawnCircles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
            GameObject circle = Instantiate(circlePrefab, randomPos, Quaternion.identity);
            circles.Add(circle);
        }
    }

    private void RemoveIntersectedCircles()
    {
        foreach (var circle in circles)
        {
            if (IntersectsLine(circle.transform.position))
            {
                Destroy(circle);
            }
        }
        circles.RemoveAll(circle => circle == null);
    }

    private bool IntersectsLine(Vector3 circlePos)
    {
        for (int i = 0; i < linePoints.Count - 1; i++)
        {
            if (IsPointOnLine(circlePos, linePoints[i], linePoints[i + 1]))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        float d1 = Vector3.Distance(point, lineStart);
        float d2 = Vector3.Distance(point, lineEnd);
        float lineLength = Vector3.Distance(lineStart, lineEnd);
        float buffer = 0.1f;
        return Mathf.Abs(d1 + d2 - lineLength) < buffer;
    }

    private void ClearLineAndPoints()
    {
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }

    private void Restart()
    {
        foreach (var circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();
        ClearLineAndPoints();
        SpawnCircles(Random.Range(5, 11));
    }
}
