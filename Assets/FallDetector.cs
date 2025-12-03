using UnityEngine;

public class FallDetector : MonoBehaviour
{
    // 부활할 위치 (아까 만든 RespawnPoint 넣기)
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 닿았을 때
        if (collision.CompareTag("Player"))
        {
            // 1. 플레이어 몸에 붙어있는 'HeartHealth' 스크립트를 찾아옵니다.
            HeartHealth healthScript = collision.GetComponent<HeartHealth>();

            // 2. 스크립트가 있다면 데미지 1을 줍니다. (UI도 자동으로 깎임!)
            if (healthScript != null)
            {
                healthScript.TakeDamage(1);
                Debug.Log("으악! 떨어졌다! 하트 감소!");
            }

            // 3. 위치를 부활 지점으로 이동
            collision.transform.position = respawnPoint.position;

            // 4. 떨어지던 가속도 멈추기 (안 하면 부활하자마자 쿵 찍음)
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}