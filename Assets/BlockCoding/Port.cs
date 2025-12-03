using UnityEngine;
using UnityEngine.EventSystems; // 이게 꼭 있어야 함

// IDropHandler가 추가되었습니다!
public class Port : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool isOutput = false;
    public WireManager manager;

    void Start()
    {
        manager = FindObjectOfType<WireManager>();
    }

    // 1. 드래그 시작 (Output 포트만 가능)
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isOutput)
        {
            manager.StartDrawingWire(this.GetComponent<RectTransform>());
        }
    }

    public void OnDrag(PointerEventData eventData) { }

    // 2. 드래그 끝 (허공에 놓으면 삭제됨)
    public void OnEndDrag(PointerEventData eventData)
    {
        // 연결에 성공했다면 이미 currentWire가 null이라서 아무 일도 안 일어남
        // 연결에 실패했다면 여기서 선이 삭제됨
        manager.StopDrawingWire();
    }

    // [새로 추가된 핵심 기능]
    // 누군가 무언가를 드래그하다가 내 위에서 마우스를 놓았을 때 실행됨!
    public void OnDrop(PointerEventData eventData)
    {
        // 나는 Input 포트이고, 드래그 된 것이 Output에서 온 선이라면?
        if (!isOutput)
        {
            // 매니저야! 여기 내 위치(transform) 줄게, 선 끝부분을 나한테 붙여!
            manager.CompleteConnection(this.GetComponent<RectTransform>());
        }
    }
}