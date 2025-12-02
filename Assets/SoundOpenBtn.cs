using UnityEngine;

public class SoundOpenBtn : MonoBehaviour
{
    // 여기에 하이어라키에 있는 SoundManager를 끌어다 놓을 겁니다.
    public SoundManager soundManager;

    // 마우스로 이 그림을 클릭하면 실행됨
    private void OnMouseDown()
    {
        // SoundManager야, 팝업창 좀 열어줘!
        if (soundManager != null)
        {
            soundManager.OpenPopup();
        }
    }
}