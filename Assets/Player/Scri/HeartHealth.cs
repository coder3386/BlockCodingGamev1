using UnityEngine;
using UnityEngine.UI;

public class HeartHealth : MonoBehaviour
{
    [Header("설정")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("UI 연결")]
    public Image[] hearts;       // 하트 이미지 3개를 순서대로 넣을 배열
    public Sprite fullHeart;     // 꽉 찬 하트 그림
    public Sprite emptyHeart;    // 빈 하트 그림 (혹은 깨진 하트)

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;

        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHeartUI()
    {
        // 하트 개수만큼 반복문을 돕니다
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                // 현재 체력보다 인덱스가 작으면 '꽉 찬 하트'
                hearts[i].sprite = fullHeart;
                hearts[i].color = Color.white; // 밝게 표시
            }
            else
            {
                // 체력이 깎인 부분은 '빈 하트'
                hearts[i].sprite = emptyHeart;
                // 만약 빈 하트 이미지가 없다면, 아래 코드를 써서 반투명하게 만드세요
                // hearts[i].color = new Color(1, 1, 1, 0.3f); 
            }

            // 최대 체력(3)보다 하트 슬롯이 더 많다면 불필요한 건 끄기
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망!");
        // 사망 처리 로직 (애니메이션, 게임오버 등)
        gameObject.SetActive(false); // 플레이어 비활성화
    }
}
