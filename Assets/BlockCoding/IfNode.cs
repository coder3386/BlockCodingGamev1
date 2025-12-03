// 3. IF 블록 (Flow Control)
// [Input: Condition(Bool)] -> [Output: True Flow, False Flow]
public class IfNode : BaseNode
{
    // If 노드는 실행될 때 조건(Bool)을 확인해서 다음 갈 길을 정함
    public BaseNode GetExecutionPath()
    {
        // 1. 조건 포트에 연결된 노드(KeyCheckNode) 값을 가져옴
        BaseNode conditionNode = GetConnectedInputNode("ConditionPort");
        bool isTrue = false;

        if (conditionNode != null) isTrue = (bool)conditionNode.GetValue();

        // 2. 참이면 "True" 포트, 거짓이면 "False" 포트로 연결된 노드 반환
        if (isTrue) return GetNextNode("TruePort");
        else return GetNextNode("FalsePort");
    }
}