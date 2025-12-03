using UnityEngine;
using UnityEngine.EventSystems; // UI 이벤트 필수 네임스페이스

public class DraggableNode : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // 부모 중 가장 가까운 Canvas를 찾습니다.
        canvas = GetComponentInParent<Canvas>();
    }

    // 마우스를 드래그할 때 실행되는 함수
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 이동량만큼 패널을 이동시킵니다.
        // canvas.scaleFactor를 나눠줘야 화면 해상도가 바껴도 속도가 일정합니다.
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}