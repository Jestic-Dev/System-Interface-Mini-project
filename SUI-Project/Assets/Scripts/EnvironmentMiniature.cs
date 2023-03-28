using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMiniature : MonoBehaviour
{
    public GameObject environmentRoot;
    private GameObject miniature;

    private void OnEnable()
    {
        miniature = Instantiate(environmentRoot);
        miniature.transform.SetParent(transform);

        miniature.transform.position = transform.position;
        miniature.transform.rotation = transform.rotation;
        miniature.transform.localScale = new Vector3(.05f, .05f, .05f);

    }

    private void OnDisable()
    {
        if (miniature != null)
            Destroy(miniature);
    }
}
