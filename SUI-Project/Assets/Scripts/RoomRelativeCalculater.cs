using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRelativeCalculater : MonoBehaviour
{
    //Singleton, ensures we are only ever working with the same RoomRelativeCalculater
    private static RoomRelativeCalculater instance;
    public static RoomRelativeCalculater Instance => GetInstance();
    private static RoomRelativeCalculater GetInstance()
    {
        if (instance != null)
            return instance;

        instance = FindObjectOfType<RoomRelativeCalculater>();
        if (instance != null)
            return instance;

        return null;
    }

    public GameObject MarkerPrefab;

    public GameObject baseEnvironment;
    public List<GameObject> EnvironmentClones;


    public void PlaceMarker(Vector3 newMarkerPosition)
    {
        GameObject markerSpinner = Instantiate(MarkerPrefab);
        markerSpinner.transform.position = newMarkerPosition;

        foreach(GameObject environment in EnvironmentClones)
        {
            Vector3 newMarkerRelativePosition = newMarkerPosition - baseEnvironment.transform.position;
            PlaceMarkerRelative(newMarkerRelativePosition, environment);
        }
    }

    public void PlaceMarkerRelative(Vector3 newMarkerRelativePosition, GameObject environment)
    {
        GameObject markerSpinner = Instantiate(MarkerPrefab);
        
        //Scale the marker's position so that it appears in the corresponding location on a bigger environment
        markerSpinner.transform.position = environment.transform.position + newMarkerRelativePosition * environment.transform.localScale.x;

        //Rotate the marker around the center of the rotated environment to align it properly
        markerSpinner.transform.RotateAround(environment.transform.position, environment.transform.forward, environment.transform.rotation.eulerAngles.z);
        markerSpinner.transform.RotateAround(environment.transform.position, environment.transform.right, environment.transform.rotation.eulerAngles.x);
        markerSpinner.transform.RotateAround(environment.transform.position, environment.transform.up, environment.transform.rotation.eulerAngles.y);
    }
}
