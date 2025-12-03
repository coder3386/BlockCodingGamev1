[System.Serializable]
public class NodeData
{
    public string id;
    public string functionName; // "MoveAction", "JumpAction", "KeyInput"
    public string currentValue; // "D", "Space" 등 사용자가 입력한 값

    public List<PortData> inputs = new List<PortData>();
    public List<PortData> outputs = new List<PortData>();
}