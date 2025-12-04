using UnityEngine;

public class WireManager : MonoBehaviour
{
    public GameObject wirePrefab;
    public RectTransform mouseFollower;

    private WireBezier currentWire; // 지금 드래그 중인 선

    void Update()
    {
        // 마우스 위치 따라가기 (기존 코드 유지)
        if (mouseFollower != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            mouseFollower.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    public void StartDrawingWire(RectTransform startPort)
    {
        GameObject newWire = Instantiate(wirePrefab, transform);
        currentWire = newWire.GetComponent<WireBezier>();
        currentWire.startPoint = startPort;
        currentWire.endPoint = mouseFollower; // 처음엔 마우스를 따라감
    }

    // [수정된 부분] 선 그리기 끝 (실패했을 때)
    public void StopDrawingWire()
    {
        // 현재 잡고 있는 선이 있는데, 연결이 안 된 상태라면? -> 삭제!
        if (currentWire != null)
        {
            Destroy(currentWire.gameObject);
            currentWire = null;
        }
    }

    // [추가된 부분] 연결 성공! (InputPort가 호출해 줄 예정)
    // WireManager.cs 수정

    public void CompleteConnection(RectTransform endPort)
    {
        if (currentWire != null)
        {
            currentWire.endPoint = endPort;

            Port inputPortScript = endPort.GetComponent<Port>();
            if (inputPortScript != null)
            {
                // [수정됨] 연결된 선이 진짜 게임 오브젝트인지 확인하고 삭제
                if (inputPortScript.connectedWire != null && inputPortScript.connectedWire.gameObject.scene.name != null)
                {
                    Destroy(inputPortScript.connectedWire.gameObject);
                }

                inputPortScript.connectedWire = currentWire;
            }

            Port outputPortScript = currentWire.startPoint.GetComponent<Port>();
            if (outputPortScript != null) outputPortScript.connectedWire = currentWire;

            currentWire = null;
        }
    }
}