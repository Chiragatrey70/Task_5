using UnityEngine;
using Cinemachine;

public class CameraToggle : MonoBehaviour
{
    public Camera firstPersonCamera; // Normal Unity Camera
    public CinemachineFreeLook thirdPersonCamera;

    private bool isFirstPerson = false;

    void Start()
    {
        ActivateThirdPerson();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFirstPerson = !isFirstPerson;
            if (isFirstPerson)
                ActivateFirstPerson();
            else
                ActivateThirdPerson();
        }
    }

    void ActivateFirstPerson()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.gameObject.SetActive(false);
    }

    void ActivateThirdPerson()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.gameObject.SetActive(true);
    }
}
