using UnityEngine;

public class Spike_trap : MonoBehaviour
{
    [Header("함정 설정")]
    public int damage = 1;             // 하트 시스템이므로 보통 1로 설정
    public float knockbackForce = 5f;  // 튕겨내는 힘 (숫자가 클수록 멀리 날아감)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 플레이어 태그 확인
        if (collision.CompareTag("Player"))
        {
            // 2. 플레이어의 HeartHealth 스크립트 가져오기
            HeartHealth playerHealth = collision.GetComponent<HeartHealth>();

            // 스크립트가 존재한다면 데미지 주기
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // 3. 넉백 (튕겨내기) 로직
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 기존 속도를 0으로 만들어야 넉백이 일정하게 들어갑니다.
                rb.linearVelocity = Vector2.zero;

                // 방향 계산: (플레이어 위치 - 함정 위치) = 함정 반대 방향
                Vector2 direction = (collision.transform.position - transform.position).normalized;

                // 위쪽으로 조금 더 잘 튀게 Y축 보정 (선택사항)
                direction.y = 0.5f;

                // 순간적인 힘(Impulse)을 가함
                rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
