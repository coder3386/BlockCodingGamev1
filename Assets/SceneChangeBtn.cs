using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeBtn : MonoBehaviour
{
    public enum ButtonType { MoveScene, ExitGame }
    public ButtonType actionType;

    [Header("설정")]
    public string sceneName;

    [Tooltip("이 버튼을 누르기 위해 클리어해야 하는 스테이지 번호 (0이면 조건 없음)")]
    public int needClearStage = 0;

    private bool isLocked = false;

    void Start()
    {
        // 종료 버튼이거나, 조건이 0이면 잠금 검사 안 함
        if (actionType == ButtonType.ExitGame || needClearStage == 0)
        {
            isLocked = false;
            return;
        }

        // 저장된 데이터 확인
        int isCleared = PlayerPrefs.GetInt("Stage_" + needClearStage, 0);

        if (isCleared == 0) // 아직 안 깼다면
        {
            isLocked = true;

            // 스프라이트(그림)일 경우 색깔 변경
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.color = Color.gray;
            }
        }
    }

    // [기존 방식] 게임 속 물체(Collider)를 클릭했을 때
    private void OnMouseDown()
    {
        ExecuteAction();
    }

    // ★ [새로운 방식] UI 버튼(Button)을 클릭했을 때 (이걸 연결할 겁니다!)
    public void OnUIClick()
    {
        ExecuteAction();
    }

    // 실제 동작을 수행하는 함수 (중복 방지용으로 따로 뺌)
    void ExecuteAction()
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