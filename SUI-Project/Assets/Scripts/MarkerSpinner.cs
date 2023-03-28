using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSpinner : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(.3f, 3, 0.1f));
    }
}
