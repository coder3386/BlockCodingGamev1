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
    public void CompleteConnection(RectTransform endPort)
    {
        if (currentWire != null)
        {
            // 1. 선의 끝점을 마우스에서 -> InputPort로 바꿔줌 (자석처럼 딱 붙음)
            currentWire.endPoint = endPort;

            Port inputPortScript = endPort.GetComponent<Port>();
            if (inputPortScript != null)
            {
                // [추가된 부분] 만약 이 포트에 이미 다른 선이 연결되어 있었다면?
                if (inputPortScript.connectedWire != null)
                {
                    // 그 옛날 선은 화면에서 지워버려라! (유령 방지)
                    Destroy(inputPortScript.connectedWire.gameObject);
                }

                // 새 선으로 등록
                inputPortScript.connectedWire = currentWire;
            }
            
            // 2. 출발점(Output) 포트에 등록
            Port outputPortScript = currentWire.startPoint.GetComponent<Port>();
            if (outputPortScript != null) outputPortScript.connectedWire = currentWire;

            // 2. 이제 이 선은 "연결 완료"된 거니까 currentWire 변수를 비워줌
            // (그래야 StopDrawingWire가 실행돼도 삭제되지 않음)
            currentWire = null;
        }
    }
}