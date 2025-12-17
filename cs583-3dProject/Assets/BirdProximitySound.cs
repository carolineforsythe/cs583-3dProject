using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdProximitySound : MonoBehaviour
{
    public static readonly List<Transform> ActiveBirds = new List<Transform>();

    void OnEnable()
    {
        ActiveBirds.Add(transform);
    }

    void OnDisable()
    {
        ActiveBirds.Remove(transform);
    }

    void OnDestroy()
    {
        ActiveBirds.Remove(transform);
    }
}

