using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float rechargeAmount = 50f;

    private void OnTriggerEnter(Collider other)
    {
        FlashlightController flashlight = other.GetComponentInChildren<FlashlightController>();

        if (flashlight != null)
        {
            flashlight.RechargeBattery(rechargeAmount);
            Destroy(gameObject); // destroy battery after pickup
        }
    }
}
