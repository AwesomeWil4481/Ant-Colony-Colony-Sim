using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSelectionBox : MonoBehaviour
{
    public SoulBox soulBox;

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.GetComponent<GodBox>().currentActionState == GodBox.actionState.Digging)
        {
            print("Triggered");

            soulBox.incomingRequest.Enqueue(new Task { name = "Dig Task", taskPosition = other.transform.position, taskType = Task.taskTypes.Dig, targetObject = other.gameObject});

            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
