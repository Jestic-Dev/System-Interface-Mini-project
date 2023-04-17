using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPlayerInput : MonoBehaviour
{
    public MatrixRoomManager matrixRoom;

    public SwapHandModel handSwapLeft;
    public SwapHandModel handSwapRight;

    private customInputState inputState;

    private enum customInputState
    {
        Regular,
        Matrix
    }

    public InputActionProperty leftHandMatrixOpener;
    public InputActionProperty rightHandMatrixOpener;
    private bool openerLock = false;

    void Start()
    {
        inputState = customInputState.Regular;

        if(matrixRoom == null)
            matrixRoom = FindAnyObjectByType<MatrixRoomManager>();
    }


    private void Update()
    {
        if (openerLock == false &&
            (leftHandMatrixOpener.action.ReadValue<float>() == 1 ||
            rightHandMatrixOpener.action.ReadValue<float>() == 1))
        {
            matrixRoom.ToggleRoom();
            SwapInputState();
            openerLock = true;
        }

        if (openerLock == true &&
            leftHandMatrixOpener.action.ReadValue<float>() == 0 &&
            rightHandMatrixOpener.action.ReadValue<float>() == 0)
        {
            openerLock = false;
        }
    }

    private void SwapInputState()
    {
        if (inputState == customInputState.Regular)
        {
            inputState = customInputState.Matrix;
            handSwapLeft.ChangeToHand();
            handSwapRight.ChangeToHand();
        }
        else if (inputState == customInputState.Matrix) 
        {
            inputState= customInputState.Regular;
            handSwapLeft.ChangeToController();
            handSwapRight.ChangeToController();
        }
    }
}
