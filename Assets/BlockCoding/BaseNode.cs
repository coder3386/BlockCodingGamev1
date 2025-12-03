using UnityEngine;
using TMPro;

public class BaseNode : MonoBehaviour
{
    public string nodeName;
    public TMP_InputField inputField;

    // 값을 가져오는 함수
    public virtual object GetValue()
    {
        if (inputField != null) return inputField.text;
        return null;
    }

    // 다음 순서 노드를 찾는 함수 (Flow)
    public BaseNode GetNextNode(string portName)
    {
        // 1. 내 자식들 중에서 이름이 맞는 오브젝트(포트)를 찾는다
        Transform portTrans = transform.Find(portName);
        if (portTrans == null) return null;

        // [에러 해결 포인트] 여기서 port라는 변수를 확실하게 선언합니다.
        Port port = portTrans.GetComponent<Port>();

        // port 변수가 있고, 그 포트에 선(connectedWire)이 연결되어 있다면?
        if (port != null && port.connectedWire != null)
        {
            // 선의 끝점(도착지)에 있는 노드를 반환 (다음 실행할 노드)
            return port.connectedWire.endPoint.GetComponentInParent<BaseNode>();
        }
        return null;
    }

    // 데이터가 들어오는 노드를 찾는 함수 (Data)
    public BaseNode GetConnectedInputNode(string portID)
    {
        // 1. 내 자식들 중에서 이름이 맞는 오브젝트(포트)를 찾는다
        Transform portTrans = transform.Find(portID);
        if (portTrans == null) return null;

        // [에러 해결 포인트] 여기서도 port라는 변수를 선언해야 합니다.
        Port port = portTrans.GetComponent<Port>();

        // port 변수가 있고, 연결된 선이 있다면?
        if (port != null && port.connectedWire != null)
        {
            // 데이터는 선의 시작점(StartPoint)에서 오므로 startPoint의 부모 노드를 찾음
            return port.connectedWire.startPoint.GetComponentInParent<BaseNode>();
        }
        return null;
    }
}