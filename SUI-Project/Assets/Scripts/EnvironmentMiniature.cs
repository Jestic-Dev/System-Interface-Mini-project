using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMiniature : MonoBehaviour
{
    public GameObject environmentRoot;
    public GameObject miniaturePlayerPrefab;
    private GameObject miniatureEnvironment;
    private GameObject miniaturePlayer;

    public void SetupMiniature()
    {
        float scaleFactor = 0.05f;

        //Create the miniature environment
        miniatureEnvironment = Instantiate(environmentRoot);
        miniatureEnvironment.transform.SetParent(transform);

        //Set transform values for the miniature environment
        miniatureEnvironment.transform.position = transform.position;
        miniatureEnvironment.transform.rotation = transform.rotation;
        miniatureEnvironment.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        //Create the miniature player figure
        miniaturePlayer = Instantiate(miniaturePlayerPrefab);
        miniaturePlayer.transform.SetParent(transform);

        //Use the position of the player relative to the environment to calculate position difference for the miniature player figure
        GameObject playerRoot = FindObjectOfType<CharacterController>().gameObject;
        Vector3 playerRelativePosition = playerRoot.transform.position - (environmentRoot.transform.position + new Vector3(0, 1, 0));
        playerRelativePosition = playerRelativePosition * scaleFactor + new Vector3(0, scaleFactor * 2, 0);

        //Set transform values for the miniature player figure
        miniaturePlayer.transform.position = miniatureEnvironment.transform.position + playerRelativePosition;
        miniaturePlayer.transform.RotateAround(transform.position, Vector3.up, miniatureEnvironment.transform.rotation.eulerAngles.y);
        miniaturePlayer.transform.rotation = playerRoot.transform.rotation * miniatureEnvironment.transform.rotation;
        miniaturePlayer.transform.localScale = new Vector3(scaleFactor * 2, scaleFactor * 2, scaleFactor * 2);
    }

    private void OnDisable()
    {
        //Remove both of the miniature objects
        if (miniatureEnvironment != null)
            Destroy(miniatureEnvironment);

        if (miniaturePlayer != null)
            Destroy(miniaturePlayer);
    }
}
