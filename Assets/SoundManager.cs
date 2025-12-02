using UnityEngine;
using UnityEngine.UI; // UI 기능을 쓰기 위해 필수!

public class SoundManager : MonoBehaviour
{
    public GameObject soundPopup; // 소리 조절 창 (패널)

    // 1. 소리 버튼 누르면 창 켜기
    public void OpenPopup()
    {
        soundPopup.SetActive(true);
    }

    // 2. 닫기 버튼 누르면 창 끄기
    public void ClosePopup()
    {
        soundPopup.SetActive(false);
    }

    // 3. 슬라이더를 움직일 때마다 실행되는 함수
    // volume에는 0.0 ~ 1.0 사이의 값이 들어옵니다.
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // 게임 전체 소리 크기 조절
        Debug.Log("현재 볼륨: " + volume);
    }
}