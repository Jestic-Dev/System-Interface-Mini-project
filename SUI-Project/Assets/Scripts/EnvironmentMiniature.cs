using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class EnvironmentMiniature : MonoBehaviour
{
    public GameObject environmentRoot;
    public Transform miniatureRoot;

    public Transform miniaturePlayer;
    private Transform miniatureEnvironment;

    public float scaleFactor;

    private Vector3 initialFigurePos;
    private XRGrabInteractable figurineInteractable;

    private Vector3 storedFigurePos;
    private Quaternion storedFigureRot;

    public bool IsGrabbingFigure { get { return isGrabbingFigure; } }
    private bool isGrabbingFigure = false;
    private Vector3 figurePlacementPoint;
    public Transform figureTeleportMarker;

    public LineRenderer figurePlacementLaser;
    public Color canPlaceColor;
    public Color cannotPlaceColor;

    public bool IsOpen { get { return isOpen; } }
    [SerializeField]
    private bool isOpen = false;

    void Start()
    {
        figurePlacementLaser.startWidth = 0.01f;
        figurePlacementLaser.endWidth = 0f;
    }

    public void SetupMiniature(Vector3 playerPos, Quaternion playerRot)
    {
        if (miniatureEnvironment == null)
        {
            //Create the miniature environment
            miniatureEnvironment = Instantiate(environmentRoot).transform;
            miniatureEnvironment.SetParent(miniatureRoot);
            foreach(Transform obj in miniatureEnvironment.GetComponentInChildren<Transform>(includeInactive: true))
            {
                obj.gameObject.layer = gameObject.layer;
            }
            miniatureEnvironment.gameObject.layer = gameObject.layer;
        }

        if(figurineInteractable == null)
        {
            figurineInteractable = miniaturePlayer.GetComponent<XRGrabInteractable>();
        }

        if (!IsOpen)
        {
            //Set transform values for the miniature environment
            miniatureEnvironment.position = miniatureRoot.transform.position;
            miniatureEnvironment.rotation = miniatureRoot.transform.rotation;
            miniatureEnvironment.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            //Use the position of the player relative to the environment to calculate position difference for the miniature player figure
            Vector3 playerRelativePosition = playerPos - environmentRoot.transform.position;
            playerRelativePosition *= scaleFactor;

            //Set transform values for the miniature player figure
            miniaturePlayer.position = miniatureEnvironment.position + playerRelativePosition;
            miniaturePlayer.RotateAround(miniatureRoot.transform.position, Vector3.up, miniatureEnvironment.rotation.eulerAngles.y);
            miniaturePlayer.rotation = playerRot * miniatureEnvironment.rotation;
            miniaturePlayer.localScale = new Vector3(scaleFactor * 2, scaleFactor * 2, scaleFactor * 2);

            initialFigurePos = miniaturePlayer.localPosition;

            figurineInteractable.enabled = true;
            figurineInteractable.selectEntered.AddListener(GrabFigure);
            figurineInteractable.selectExited.AddListener(ReleaseFigure);

            isOpen = true;
        }
    }

    public void StartClosing()
    {
        figurineInteractable.enabled = false;

        figurineInteractable.selectEntered.RemoveListener(GrabFigure);
        figurineInteractable.selectExited.RemoveListener(ReleaseFigure);

        isOpen = false;
    }

    public void GrabFigure(SelectEnterEventArgs a)
    {
        isGrabbingFigure = true;

        Transform playerFigure = a.interactableObject.transform;
        storedFigurePos = playerFigure.position;
        storedFigureRot = playerFigure.rotation;
        Debug.Log("GRAB EVENT");

        StartCoroutine(FigureRaycastRoutine());
    }

    public void ReleaseFigure(SelectExitEventArgs a)
    {
        isGrabbingFigure = false;

        if (figureTeleportMarker.gameObject.activeSelf)
        {
            figurineInteractable.transform.position = figurePlacementPoint;
            figurineInteractable.transform.rotation = storedFigureRot;
            figureTeleportMarker.gameObject.SetActive(false);
        }
        else
        {
            figurineInteractable.transform.position = storedFigurePos;
            figurineInteractable.transform.rotation = storedFigureRot;
        }
        Debug.Log("RELEASE EVENT");
    }

    public bool WasFigureMoved()
    {
        return initialFigurePos != figurineInteractable.transform.localPosition;
    }

    public Vector3 GetScaledPosition()
    {
        Vector3 playerRelativePosition = miniaturePlayer.localPosition /= scaleFactor;

        return playerRelativePosition;
    }

    private IEnumerator FigureRaycastRoutine()
    {
        figurePlacementLaser.enabled = true; 

        while(isGrabbingFigure)
        {
            RaycastHit raycastHit;

            if (Physics.Raycast(figurineInteractable.transform.position, Vector3.down, out raycastHit, 10))
            {
                figurePlacementPoint = raycastHit.point;

                Vector3[] laserPositions = new Vector3[] { figurineInteractable.transform.position, figurePlacementPoint };
                figurePlacementLaser.SetPositions(laserPositions);

                GameObject rayHit = raycastHit.collider.gameObject;
                if (rayHit.GetComponent<TeleportationArea>())
                {
                    figurePlacementLaser.startColor = canPlaceColor;
                    figurePlacementLaser.endColor = canPlaceColor;

                    figureTeleportMarker.gameObject.SetActive(true);
                    figureTeleportMarker.position = figurePlacementPoint;
                    figureTeleportMarker.up = raycastHit.normal;
                }
                else
                {
                    figurePlacementLaser.startColor = cannotPlaceColor;
                    figurePlacementLaser.endColor = cannotPlaceColor;

                    figureTeleportMarker.gameObject.SetActive(false);
                }
            }


            yield return new WaitForSeconds(0);
        }

        figurePlacementLaser.enabled = false;
    }
}
