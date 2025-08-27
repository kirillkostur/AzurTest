using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab; // Префаб с Particle System

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (explosionPrefab != null)
            {
                // Создаём копию взрыва в позиции триггера
                GameObject explosion = Instantiate(
                    explosionPrefab,
                    transform.position,
                    Quaternion.identity
                );

                // Берём ParticleSystem из префаба
                ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    // Уничтожаем объект после того, как отработает эффект
                    Destroy(explosion, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }        
        }
    }
}
