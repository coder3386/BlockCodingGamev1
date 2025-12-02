using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeBtn : MonoBehaviour
{
    // 버튼의 종류를 고르는 목록 (이동 vs 종료)
    public enum ButtonType { MoveScene, ExitGame }
    public ButtonType actionType;

    // 이동할 씬 이름 (종료 버튼일 때는 비워도 됨)
    public string sceneName;

    private void OnMouseDown()
    {
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