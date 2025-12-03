using UnityEngine;
using TMPro; // 텍스트 입력을 읽기 위해 필요

public class NodeInfo : MonoBehaviour
{
    // 이 노드의 역할이 무엇인지 선택 (Inspector에서 설정)
    public enum NodeType { InputKey, MoveAction, JumpAction }
    public NodeType type;

    // 만약 InputKey 노드라면, 사용자가 입력한 키(예: "D")를 저장할 곳
    public TMP_InputField inputField;

    // 외부에서 이 노드의 값을 물어볼 때 대답해주는 함수
    public string GetValue()
    {
        if (type == NodeType.InputKey && inputField != null)
        {
            return inputField.text.ToUpper(); // 대문자로 변환해서 줌 ("d" -> "D")
        }
        return "";
    }
}