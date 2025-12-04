using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    // 1. FHD (1920 x 1080) - 16:9 비율
    public void SetResolution_1920x1080()
    {
        // (가로, 세로, 전체화면 여부)
        Screen.SetResolution(1920, 1080, true);
        Debug.Log("해상도 변경: 1920x1080 (Full Screen)");
    }

    // 2. HD+ (1600 x 900) - 16:9 비율 (사양 낮을 때용)
    public void SetResolution_1600x900()
    {
        Screen.SetResolution(1600, 900, true);
        Debug.Log("해상도 변경: 1600x900 (Full Screen)");
    }

    // 3. 16:10 비율 (예: 1280 x 800) - 노트북 등에서 많이 씀
    public void SetResolution_1280x800()
    {
        Screen.SetResolution(1280, 800, true);
        Debug.Log("해상도 변경: 1280x800 (16:10)");
    }

    // 4. 창 모드 / 전체 화면 토글 (선택 사항)
    public void SetWindowMode()
    {
        // 현재 해상도는 유지하되, 창 모드로 변경 (false)
        Screen.SetResolution(Screen.width, Screen.height, false);
    }
}