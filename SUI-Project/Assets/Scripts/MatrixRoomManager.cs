using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatrixRoomManager : MonoBehaviour
{
    //Singleton, ensures we are only ever working with the same MatrixRoomManager
    private static MatrixRoomManager instance;
    public static MatrixRoomManager Instance => GetInstance();
    private static MatrixRoomManager GetInstance()
    {
        if (instance != null)
            return instance;

        instance = FindObjectOfType<MatrixRoomManager>();
        if (instance != null)
            return instance;

        return null;
    }

    public XROrigin xrOrigin;
    public Transform playerPositionTracking;
    public EnvironmentMiniature environmentMiniature;
    public GameObject activeRoot;
    public float forwardOffset;

    void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = FindObjectOfType<XROrigin>();
        }

        if (environmentMiniature == null)
        {
            if (!activeRoot.activeSelf)
                activeRoot.SetActive(true);

            environmentMiniature = GetComponentInChildren<EnvironmentMiniature>();
        }

        activeRoot.SetActive(false);
    }

    public void ToggleRoom()
    {
        //If the Matrix room is active, ask if the player has been moved before turning it off
        if (activeRoot.activeSelf)
        {
            Debug.Log("Test if player was moved");
            if(environmentMiniature.WasFigureMoved())
            {
                Debug.Log("Was moved");
                Vector3 scaledPos = environmentMiniature.GetScaledPosition();
                Vector3 camOffset = xrOrigin.transform.position - Camera.main.transform.position;
                
                Debug.Log(xrOrigin.transform.position);
                xrOrigin.transform.position = scaledPos;// + new Vector3(camOffset.x, 0, camOffset.z);
                Debug.Log(xrOrigin.transform.position);
            }

            activeRoot.SetActive(false);
        }
        else
        {
            //Make the Matrix Room appear with the specified position and rotation
            activeRoot.SetActive(true);

            Transform playerHead = Camera.main.transform;
            Vector3 groundedPlayerPos = playerHead.position - new Vector3(0, playerPositionTracking.localPosition.y, 0);
            Quaternion flattenedPlayerRot = new Quaternion(0, playerHead.rotation.y, 0, playerHead.rotation.w);

            activeRoot.transform.position = groundedPlayerPos + new Vector3(0, 0.1f, 0);
            activeRoot.transform.rotation = flattenedPlayerRot;
            activeRoot.transform.Translate(Vector3.forward * forwardOffset);

            environmentMiniature.SetupMiniature(groundedPlayerPos, flattenedPlayerRot);
        }
    }
}
