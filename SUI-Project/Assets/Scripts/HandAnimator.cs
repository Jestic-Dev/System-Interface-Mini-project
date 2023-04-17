using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    private Animator handAnimator;
    public InputActionProperty pinchAnimAction;
    public InputActionProperty gripAnimAction;

    // Start is called before the first frame update
    void Start()
    {
        handAnimator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float pinchValue = pinchAnimAction.action.ReadValue<float>();
        float gripValue = gripAnimAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", pinchValue);
        handAnimator.SetFloat("Grip", gripValue);
    }
}
