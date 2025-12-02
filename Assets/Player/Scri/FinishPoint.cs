using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject clearPanel;  // 1단계에서 만든 UI 패널(또는 캔버스)

    [Header("이동할 씬 이름")]
    public string nextSceneName = "StageSelect"; // 이동하고 싶은 씬 이름

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 도착지에 닿았을 때
        if (collision.CompareTag("Player"))
        {
            GameClear();
        }
    }

    void GameClear()
    {
        Debug.Log("게임 클리어!");

        // 1. 숨겨뒀던 클리어 UI를 켭니다.
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }

        // 2. 게임 시간을 멈춥니다. (플레이어가 더 이상 움직이지 못하게)
        Time.timeScale = 0f;
    }

    // 이 함수는 UI 버튼에 연결해서 사용할 겁니다.
    public void LoadNextStage()
    {
        // 멈췄던 시간을 다시 흐르게 해줘야 다음 씬이 정상 작동합니다.
        Time.timeScale = 1f;

        // 지정한 씬으로 이동
        SceneManager.LoadScene(nextSceneName);
    }
}
