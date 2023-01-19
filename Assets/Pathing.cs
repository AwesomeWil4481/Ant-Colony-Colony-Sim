using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour
{
    public List<GameObject> Paths = new List<GameObject>();

    public void Awake()
    {
        foreach (Transform t in gameObject.transform)
        {
            Paths.Add(t.gameObject);
        }
    }
}
