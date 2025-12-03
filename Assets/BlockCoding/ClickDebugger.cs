using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ClickDebugger : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때 (새로운/옛날 인풋 시스템 모두 작동하도록 구현)
        if (Input.GetMouseButtonDown(0))
        {
            CheckWhatIsClicked();
        }
    }

    void CheckWhatIsClicked()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            Debug.Log($"============== 클릭 감지됨 ({results.Count}개) ==============");
            // 가장 위에 있는 녀석(범인)부터 순서대로 보여줍니다.
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log($"[{i}순위] 이름: {results[i].gameObject.name} (부모: {results[i].gameObject.transform.parent?.name})");
            }
            Debug.Log("=======================================================");
        }
        else
        {
            Debug.Log(">> 허공을 클릭했습니다 (UI가 잡히지 않음)");
        }
    }
}