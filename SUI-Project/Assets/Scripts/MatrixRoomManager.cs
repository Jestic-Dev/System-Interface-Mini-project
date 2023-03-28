using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixRoomManager : MonoBehaviour
{
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

    public GameObject activeRoot;

    // Start is called before the first frame update
    void Start()
    {
        activeRoot.SetActive(false);
    }

    public void ToggleRoom(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Toggle");
        Debug.Log(gameObject.name);
        activeRoot.SetActive(!activeRoot.activeSelf);

        if (activeRoot.activeSelf)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.Translate(Vector3.forward * 3);
        }
    }
}
