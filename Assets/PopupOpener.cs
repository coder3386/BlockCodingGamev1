using UnityEngine;

public class PopupOpener : MonoBehaviour
{
    // 열고 싶은 창(패널)을 여기에 연결할 겁니다.
    public GameObject targetPopup;

    // 마우스로 이 그림을 클릭하면 실행됨
    private void OnMouseDown()
    {
        if (targetPopup != null)
        {
            targetPopup.SetActive(true); // 창을 켠다!
            Debug.Log("설정 창 열림!");
        }
    }
}