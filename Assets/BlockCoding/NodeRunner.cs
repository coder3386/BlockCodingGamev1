using UnityEngine;
using System.Collections.Generic;

public class NodeRunner : MonoBehaviour
{
    public PlayerController player; // 조종할 인형
    public WireManager wireManager; // 선 관리자 (선 정보를 얻기 위해)

    // 매 프레임마다 검사합니다.
    void Update()
    {
        // 1. 현재 화면에 있는 모든 선(Wire)을 다 가져옵니다.
        // (WireManager에 public List<WireBezier> allWires 같은 게 있다고 가정하거나, 
        //  간단히 씬의 모든 WireBezier를 찾습니다.)
        WireBezier[] allWires = FindObjectsByType<WireBezier>(FindObjectsSortMode.None);

        foreach (WireBezier wire in allWires)
        {
            // 선이 연결되지 않았다면 무시
            if (wire.startPoint == null || wire.endPoint == null) continue;

            // 2. 선의 끝점(도착점)에 있는 노드가 무슨 노드인지 확인
            // (endPoint는 Port입니다. Port의 부모가 바로 Node입니다!)
            NodeInfo actionNode = wire.endPoint.GetComponentInParent<NodeInfo>();

            // 3. 선의 시작점(출발점)에 있는 노드가 무슨 노드인지 확인
            NodeInfo inputNode = wire.startPoint.GetComponentInParent<NodeInfo>();

            // 안전장치
            if (actionNode == null || inputNode == null) continue;

            // 4. 로직 실행: "입력 노드" -> "액션 노드"로 연결된 경우만 처리
            if (inputNode.type == NodeInfo.NodeType.InputKey)
            {
                // 입력 노드에 적힌 글자 가져오기 (예: "D")
                string keyString = inputNode.GetValue();
                if (string.IsNullOrEmpty(keyString)) continue;

                // 키보드 입력 체크
                KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);

                if (Input.GetKey(key)) // 키를 누르고 있다면?
                {
                    ExecuteAction(actionNode);
                }
            }
        }
    }

    void ExecuteAction(NodeInfo node)
    {
        // 노드의 타입에 따라 플레이어에게 명령
        if (node.type == NodeInfo.NodeType.MoveAction)
        {
            player.SetMoveInput(1f); // 오른쪽으로 이동! (왼쪽은 -1로 응용 가능)
        }
        else if (node.type == NodeInfo.NodeType.JumpAction)
        {
            player.DoJump();
        }
    }
}