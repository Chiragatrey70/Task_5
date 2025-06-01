using UnityEngine;
using UnityEngine.UI; // Needed for optional UI

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    public KeyCode toggleKey = KeyCode.F;

    public float maxBattery = 100f;
    public float batteryDrainRate = 10f;     // per second
    public float batteryRechargeRate = 5f;   // per second

    private float currentBattery;
    private bool isFlashlightOn = false;

    // Optional UI (drag your UI Slider here)
    public Slider batteryBar;

    void Start()
    {
        currentBattery = maxBattery;
        UpdateBatteryUI();
    }

    void Update()
    {
        // Toggle flashlight on/off
        if (Input.GetKeyDown(toggleKey))
        {
            if (!isFlashlightOn && currentBattery > 0f)
            {
                TurnFlashlightOn();
            }
            else if (isFlashlightOn)
            {
                TurnFlashlightOff();
            }
        }

        // Drain or recharge battery
        if (isFlashlightOn)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;

            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                TurnFlashlightOff(); // auto shut off
            }
        }
        else if (currentBattery < maxBattery)
        {
            currentBattery += batteryRechargeRate * Time.deltaTime;
            if (currentBattery > maxBattery)
                currentBattery = maxBattery;
        }

        UpdateBatteryUI();
    }

    void TurnFlashlightOn()
    {
        flashlight.enabled = true;
        isFlashlightOn = true;
    }

    void TurnFlashlightOff()
    {
        flashlight.enabled = false;
        isFlashlightOn = false;
    }

    void UpdateBatteryUI()
    {
        if (batteryBar != null)
        {
            batteryBar.value = currentBattery / maxBattery;
        }
    }
    public void RechargeBattery(float amount)
{
    currentBattery += amount;
    if (currentBattery > maxBattery)
        currentBattery = maxBattery;

    UpdateBatteryUI(); // in case you’re using UI
}

}
