using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeBtn : MonoBehaviour
{
    // 버튼의 종류를 고르는 목록 (이동 vs 종료)
    public enum ButtonType { MoveScene, ExitGame }
    public ButtonType actionType;

    [Header("설정")]
    // 이동할 씬 이름 (종료 버튼일 때는 비워도 됨)
    public string sceneName;

    [Tooltip("이 버튼을 누르기 위해 클리어해야 하는 스테이지 번호 (0이면 조건 없음)")]
    public int needClearStage = 0;

    private bool isLocked = false; // 잠금 상태 확인용 변수

    void Start()
    {
        // 종료 버튼이거나, 조건이 0이면 잠금 검사 안 함
        if (actionType == ButtonType.ExitGame || needClearStage == 0)
        {
            isLocked = false;
            return;
        }

        // 저장된 데이터를 확인 (예: needClearStage가 1이면 "Stage_1"이 1인지 확인)
        int isCleared = PlayerPrefs.GetInt("Stage_" + needClearStage, 0);

        if (isCleared == 0) // 아직 안 깼다면
        {
            isLocked = true; // 잠금 설정

            // 시각적으로 잠긴 걸 보여주기 위해 색깔을 회색으로 변경 (선택사항)
            // 스프라이트 렌더러가 있는 경우에만 작동
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.color = Color.gray;
            }
        }
    }

    private void OnMouseDown()
    {
        if (isLocked == true)
        {
            Debug.Log("잠겨있는 스테이지입니다.");
            return;
        }

        if (actionType == ButtonType.ExitGame)
        {
            Debug.Log("게임 종료!");
            Application.Quit();
        }
        else
        {
            Debug.Log("씬 이동: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }
}