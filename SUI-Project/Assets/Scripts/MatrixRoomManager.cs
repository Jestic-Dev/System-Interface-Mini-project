using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public EnvironmentMiniature environmentMiniature;
    public GameObject activeRoot;

    void Start()
    {
        activeRoot.SetActive(false);
    }

    public void ToggleRoom(Vector3 position, Quaternion rotation)
    {
        //Make the Matrix Room appear with the specified position and rotation
        activeRoot.SetActive(!activeRoot.activeSelf);

        if (activeRoot.activeSelf)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.Translate(Vector3.forward * 3);

            if (environmentMiniature == null)
                environmentMiniature = GetComponentInChildren<EnvironmentMiniature>();

            environmentMiniature.SetupMiniature();
        }
    }
}
