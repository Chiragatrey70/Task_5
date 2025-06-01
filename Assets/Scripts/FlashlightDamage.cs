using UnityEngine;

public class FlashlightDamage : MonoBehaviour
{
    public Light flashlight;
    public float damagePerSecond = 20f;
    public float beamAngle = 45f;
    public float maxDistance = 15f;

    void Update()
    {
        if (flashlight.enabled)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(ray, Mathf.Tan(beamAngle * Mathf.Deg2Rad / 2) * maxDistance, maxDistance);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damagePerSecond * Time.deltaTime);
                    }
                }
            }
        }
    }
}
