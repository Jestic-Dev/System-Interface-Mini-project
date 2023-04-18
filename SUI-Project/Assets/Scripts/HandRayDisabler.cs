using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandRayDisabler : MonoBehaviour
{
    public GameObject handRaycaster;
    public InputActionProperty rayActivateInput;

    void Update()
    {
        handRaycaster.SetActive(rayActivateInput.action.ReadValue<float>() > 0.8f);        
    }
}
