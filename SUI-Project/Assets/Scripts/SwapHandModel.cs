using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHandModel : MonoBehaviour
{
    public GameObject vrHand;
    public GameObject vrController;

    private void Start()
    {
        ChangeToController();
    }

    public void ChangeToHand()
    {
        vrController.SetActive(false);
        vrHand.SetActive(true);
    }

    public void ChangeToController()
    {
        vrController.SetActive(true);
        vrHand.SetActive(false);
    }
}
