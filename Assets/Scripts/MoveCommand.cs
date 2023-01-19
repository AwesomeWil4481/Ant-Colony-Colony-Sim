using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveCommand : MonoBehaviour
{
    public List<GameObject> approachingObjects;

    void Start()
    {
        
    }

    void Update()
    {
       if (approachingObjects.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
