using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Make the Matrix Room appear based on the player's position and rotation
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Vector3 roomSpawnPosition = transform.position + new Vector3(0, -characterController.height * 0.5f + 0.001f, 0);
            Quaternion roomSpawnRotation = transform.rotation;
            MatrixRoomManager.Instance.ToggleRoom(roomSpawnPosition, roomSpawnRotation);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RoomRelativeCalculater.Instance.PlaceMarker(transform.position);
        }

        //Simple character movement input
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(transform.forward * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            characterController.Move(-transform.right * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(-transform.forward * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            characterController.Move(transform.right * moveSpeed * Time.deltaTime);
        }

        characterController.Move(new Vector3(0, -0.1f, 0));

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -1, 0) * rotSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 1, 0) * rotSpeed);
        }
    }
}
