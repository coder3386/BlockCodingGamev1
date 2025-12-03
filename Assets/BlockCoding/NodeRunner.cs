using UnityEngine;
using System.Collections.Generic;

public class NodeRunner : MonoBehaviour
{
    public PlayerController player; // 제어할 플레이어 연결
    public List<NodeData> allNodes; // 씬에 있는 모든 노드들 (수동 할당 혹은 자동 검색)
    public List<ConnectionData> allConnections; // 연결 정보들

    void Update()
    {
        // 매 프레임 모든 노드를 검사합니다.
        foreach (var node in allNodes)
        {
            // "Action" 타입의 노드(Move, Jump)만 실행 로직을 가동합니다.
            if (node.functionName == "MoveAction")
            {
                RunMoveNode(node);
            }
            else if (node.functionName == "JumpAction")
            {
                RunJumpNode(node);
            }
        }
    }

    // [이동 노드 실행 로직]
    void RunMoveNode(NodeData node)
    {
        // 1. 노드의 "KeyInput" 포트에 연결된 데이터를 가져온다.
        string keyCodeStr = GetInputData(node, "KeyInput");
        if (string.IsNullOrEmpty(keyCodeStr)) return; // 연결 안 됨

        // 2. 해당 키를 누르고 있는지 확인
        KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodeStr);

        // 3. 키를 누르고 있으면 플레이어에게 명령 전달
        if (Input.GetKey(code))
        {
            // 노드의 다른 설정값(방향)을 읽어서 1(우) 또는 -1(좌) 전달
            // 여기서는 간단히 'Right' 노드라고 가정하고 1을 보냄
            player.SetInput(1f);
        }
    }

    // [점프 노드 실행 로직]
    void RunJumpNode(NodeData node)
    {
        string keyCodeStr = GetInputData(node, "KeyInput");
        if (string.IsNullOrEmpty(keyCodeStr)) return;

        KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodeStr);

        // 점프는 누르는 순간(GetKeyDown) 한 번만!
        if (Input.GetKeyDown(code))
        {
            player.DoJump();
        }
    }

    // 연결된 선을 타고 가서 값을 가져오는 함수 (이전 강의의 업그레이드 버전)
    string GetInputData(NodeData currentNode, string portName)
    {
        // 1. 현재 노드의 특정 입력 포트 ID 찾기
        var port = currentNode.inputs.Find(p => p.id.Contains(portName));
        if (port == null) return "";

        // 2. 그 포트에 연결된 선(Connection) 찾기
        var connection = allConnections.Find(c => c.inputPortID == port.id);
        if (connection == null) return "";

        // 3. 선의 반대편(Output) 노드 찾기
        var sourceNode = allNodes.Find(n => n.outputs.Exists(o => o.id == connection.outputPortID));
        if (sourceNode == null) return "";

        // 4. 그 노드가 가진 값(변수) 리턴 (예: "D", "Space")
        // 실제로는 NodeData 안에 'currentValue' 같은 변수를 만들어 저장해둬야 함
        return sourceNode.currentValue;
    }
}