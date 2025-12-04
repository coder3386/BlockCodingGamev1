using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems; // [필수 추가] 이게 있어야 UI 선택을 해제할 수 있습니다.

public class CodeInterpreter : MonoBehaviour
{
    public BaseNode startNode;
    public PlayerController player;
    public TextMeshProUGUI playButtonText;

    private Coroutine runningProcess;
    private bool isRunning = false;

    // 버튼을 누르면 실행되는 함수
    public void OnClickPlayButton()
    {
        // [핵심 해결책] 버튼을 누르자마자 UI 선택을 강제로 풀어버립니다.
        // 이제 키보드를 눌러도 버튼이 가로채지 않고, 게임(Player)에게 바로 전달됩니다.
        EventSystem.current.SetSelectedGameObject(null);

        if (isRunning)
        {
            StopCode();
        }
        else
        {
            RunCode();
        }
    }

    void RunCode()
    {
        isRunning = true;
        if (playButtonText != null) playButtonText.text = "STOP";

        if (runningProcess != null) StopCoroutine(runningProcess);
        runningProcess = StartCoroutine(ExecuteGraph(startNode));
    }

    void StopCode()
    {
        isRunning = false;
        if (playButtonText != null) playButtonText.text = "PLAY";

        if (runningProcess != null)
        {
            StopCoroutine(runningProcess);
            runningProcess = null;
        }

        player.SetMoveInput(0);
    }

    IEnumerator ExecuteGraph(BaseNode startNode)
    {
        while (isRunning)
        {
            // 매 프레임 일단 멈춤 (키 떼면 멈추게 하기 위함)
            player.SetMoveInput(0);

            BaseNode currentNode = startNode;
            int safetyCount = 0;

            while (currentNode != null)
            {
                if (currentNode is MoveNode)
                {
                    MoveNode move = (MoveNode)currentNode;
                    player.SetMoveInput(move.direction.x);
                    currentNode = currentNode.GetNextNode("NextPort");
                }
                else if (currentNode is JumpNode)
                {
                    // 플레이어의 DoJump 함수 호출
                    player.DoJump();

                    // 점프 후 다음 블록으로 이동 (없으면 끝남)
                    currentNode = currentNode.GetNextNode("NextPort");
                }
                else if (currentNode is IfNode)
                {
                    IfNode ifNode = (IfNode)currentNode;
                    currentNode = ifNode.GetExecutionPath();
                }
                else
                {
                    currentNode = currentNode.GetNextNode("NextPort");
                }

                safetyCount++;
                if (safetyCount > 1000) break;
            }

            yield return null;
        }
    }
}