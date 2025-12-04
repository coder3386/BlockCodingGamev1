using UnityEngine;

public class CodingUIToggle : MonoBehaviour
{
    [Header("UI Control")]
    public CanvasGroup codingCanvasGroup; // [변경] GameObject 대신 CanvasGroup을 씁니다.
    public Transform wireParent;          // [변경] 선들이 모여있는 부모 오브젝트 (WireManager)

    private bool isVisible = true;
    public GameObject player;

    // [추가된 부분 1] 제외하고 싶은 캔버스의 이름을 적는 칸
    [Header("Settings")]
    public string excludeCanvasName = "ButtonCanvas"; // 예: 말풍선 등

    void Start()
    {
        // 시작 시 상태 업데이트
        isVisible = false;
        UpdateUI();
    }

    public void ToggleWindow()
    {
        isVisible = !isVisible;

        // 창이 열리면 시간 정지, 닫히면 시간 흐름 (원하시면 유지하세요)
        if (isVisible) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        UpdateUI();
    }

    void UpdateUI()
    {
        // 1. 노드 UI 처리 (CanvasGroup 사용)
        if (codingCanvasGroup != null)
        {
            if (isVisible)
            {
                codingCanvasGroup.alpha = 1f;           // 완전 불투명 (보임)
                codingCanvasGroup.interactable = true;  // 클릭 가능
                codingCanvasGroup.blocksRaycasts = true;// 마우스 감지 함
            }
            else
            {
                codingCanvasGroup.alpha = 0f;           // 완전 투명 (안 보임)
                codingCanvasGroup.interactable = false; // 클릭 불가능
                codingCanvasGroup.blocksRaycasts = false;// 마우스 통과 (뒤에 게임 화면 클릭 가능)
            }
        }

        // 2. 선(Wire) 처리 (Renderer만 끄기!)
        if (wireParent != null)
        {
            // WireManager 아래에 있는 모든 선들의 '그림 그리는 기능'만 껐다 켰다 합니다.
            // 이렇게 하면 스크립트(Update)는 계속 돌아갑니다!
            LineRenderer[] lines = wireParent.GetComponentsInChildren<LineRenderer>();
            foreach (var line in lines)
            {
                line.enabled = isVisible;
            }
        }
        if (player != null)
        {
            // 플레이어 본체뿐만 아니라, 자식으로 딸린 무기나 모자 등도 다 같이 숨김
            Renderer[] allRenderers = player.GetComponentsInChildren<Renderer>();

            foreach (var r in allRenderers)
            {
                // 코딩창이 보이면(isVisible이 true면) -> 플레이어는 안 보여야 함(!isVisible)
                // 코딩창이 닫히면(isVisible이 false면) -> 플레이어는 보여야 함
                r.enabled = !isVisible;
            }
            // [추가된 부분 3] UI 캔버스(하트 체력바) 숨기기
            // 플레이어 자식 중에 'Canvas' 컴포넌트가 있으면 싹 다 찾습니다.
            Canvas[] childCanvases = player.GetComponentsInChildren<Canvas>();
            foreach (var c in childCanvases)
            {
                // 코딩창이 보이면(isVisible) -> 캔버스는 꺼야 함(false)
                // 코딩창이 안 보이면(!isVisible) -> 캔버스는 켜야 함(true)
                if (c.name == excludeCanvasName)
                {
                    continue;
                }
                c.enabled = !isVisible;
            }
        }
    }
}