using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<GameObject> connectedPaths = new List<GameObject> ();

    public void Start()
    {
        var pathObject = GameObject.Find("Pathing").GetComponent<Pathing>();

        foreach(GameObject g in pathObject.Paths)
        {
            float dist = Vector3.Distance(gameObject.transform.position, g.transform.position);

            if (dist <= 1)
            {
                connectedPaths.Add(g);
            }
        }   
    }
}
