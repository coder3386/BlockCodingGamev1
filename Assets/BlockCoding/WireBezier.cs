using UnityEngine;

public class WireBezier : MonoBehaviour
{
    public RectTransform startPoint; // 시작점 (UI 요소)
    public RectTransform endPoint;   // 끝점 (마우스 또는 다른 UI)

    private LineRenderer lr;
    private int segmentCount = 30; // 곡선 부드러움 정도

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = segmentCount;
    }

    void Update()
    {
        // 시작점과 끝점이 있을 때만 그립니다.
        if (startPoint != null && endPoint != null)
        {
            DrawCurve();
        }
    }

    void DrawCurve()
    {
        Vector3 p0 = startPoint.position; // 시작 위치
        Vector3 p3 = endPoint.position;   // 끝 위치

        // 중간 제어점 계산 (선을 휘게 만듦)
        float distance = Vector3.Distance(p0, p3);
        Vector3 p1 = p0 + Vector3.right * (distance * 0.5f); // 시작점에서 오른쪽
        Vector3 p2 = p3 + Vector3.left * (distance * 0.5f);  // 끝점에서 왼쪽

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            // 베지어 곡선 공식
            Vector3 point = CalculateBezier(t, p0, p1, p2, p3);

            // Z축을 0으로 강제 맞춤 (UI 평면에 그리기 위해)
            point.z = 0;
            lr.SetPosition(i, point);
        }
    }

    Vector3 CalculateBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }
}