using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixRoomAnimator : MonoBehaviour
{

    public Transform roomFloor;
    public float floorMaxScale;
    public float scaleRateFloor;

    public Transform[] roomWalls;

    public Transform matrixTable;
    public float offsetBelow;
    public float scaleRateTable;

    private Coroutine currentAnim;

    private void Start()
    {
        roomFloor.localScale = Vector3.zero;
    }

    public bool IsNotAnimating()
    {
        return currentAnim == null;
    }

    private void CancelCurrentFade()
    {
        if (currentAnim != null)
        {
            StopCoroutine(currentAnim);
        }
    }

    public void SpawnRoom(Vector3 playerHeadPos)
    {
        CancelCurrentFade();
        currentAnim = StartCoroutine(SpawningRoom(playerHeadPos));
    }

    public void DespawnRoom()
    {
        CancelCurrentFade();
        currentAnim = StartCoroutine(DespawningRoom());
    }

    private IEnumerator SpawningRoom(Vector3 playerHeadPos)
    {
        Debug.Log("Start room spawn");
        roomFloor.gameObject.SetActive(true);

        float scalingValue = 0;
        while (roomFloor.localScale.x < floorMaxScale - 0.1f)
        {
            float newScale = Mathf.Lerp(roomFloor.localScale.x, floorMaxScale, scalingValue);
            roomFloor.localScale = new Vector3(newScale, 1, newScale);
            scalingValue += Time.deltaTime / scaleRateFloor;
            yield return null;
        }

        Debug.Log("Spawning table");
        matrixTable.gameObject.SetActive(true);
        scalingValue = 0;
        float tableStartY = matrixTable.position.y;
        float targetHeight = playerHeadPos.y - offsetBelow;
        while (matrixTable.position.y < targetHeight - 0.01f)
        {
            float newYpos = Mathf.Lerp(tableStartY, targetHeight, scalingValue);
            matrixTable.position = new Vector3(matrixTable.position.x, newYpos, matrixTable.position.z);
            scalingValue += Time.deltaTime / scaleRateTable;
            yield return null;
            Debug.Log("Moving table");
            Debug.Log(matrixTable.position.y + " : " + targetHeight + " : " + scalingValue);
        }

        Debug.Log("Setting coroutine to null");
        currentAnim = null;
        yield return null;
    }

    private IEnumerator DespawningRoom()
    {
        float scalingValue = 0;
        float tableStartY = matrixTable.position.y;
        while (matrixTable.localPosition.y > -1.5f)
        {
            float newYpos = Mathf.Lerp(tableStartY, -2f, scalingValue);
            matrixTable.position = new Vector3(matrixTable.position.x, newYpos, matrixTable.position.z);
            scalingValue += Time.deltaTime;
            yield return null;
        }

        matrixTable.gameObject.SetActive(false);

        scalingValue = 0;
        while (roomFloor.localScale.x > 0.1f)
        {
            float newScale = Mathf.Lerp(roomFloor.localScale.x, 0, scalingValue);
            roomFloor.localScale = new Vector3(newScale, 1, newScale);
            scalingValue += Time.deltaTime;
            yield return null;
        }

        matrixTable.parent.gameObject.SetActive(false);
        currentAnim = null;
    }
}
