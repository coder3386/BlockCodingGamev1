using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeartHealth : MonoBehaviour
{
    [Header("설정")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("UI 연결")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // ★ 새로 추가된 부분: 게임 오버 글자 연결할 칸
    public GameObject gameOverText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHeartUI()
    {
        // (기존 하트 UI 코드 그대로 두세요)
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }

    void Die()
    {
        Debug.Log("사망!");

        // 1. 숨겨놨던 "GAME OVER" 글자를 켭니다.
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        // 2. 바로 재시작하지 않고, 1.5초 뒤에 "ReloadScene" 함수를 실행합니다.
        Invoke("ReloadScene", 1.5f);
    }

    // 1.5초 뒤에 실행될 재시작 함수
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}