using UnityEngine;

public class KeyCheckNode : BaseNode
{
    public override object GetValue()
    {
        // 1. 연결된 앞쪽 노드(StringNode)에서 키 이름을 가져옴
        BaseNode inputNode = GetConnectedInputNode("KeyNamePort"); // 밑에서 구현할 함수

        string keyName = "";
        if (inputNode != null) keyName = (string)inputNode.GetValue();
        else if (inputField != null) keyName = inputField.text; // 연결 없으면 자기 칸 읽기

        // 2. 실제 유니티 입력 체크
        try
        {
            KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName.ToUpper());
            return Input.GetKey(code); // 누르고 있으면 True 반환
        }
        catch { return false; }
    }

    // 내 포트에 연결된 데이터 노드 찾아오는 헬퍼 함수
    BaseNode GetConnectedInputNode(string portID)
    {
        Port myPort = transform.Find(portID)?.GetComponent<Port>();
        if (myPort != null && myPort.connectedWire != null)
            return myPort.connectedWire.startPoint.GetComponentInParent<BaseNode>();
        return null;
    }
}