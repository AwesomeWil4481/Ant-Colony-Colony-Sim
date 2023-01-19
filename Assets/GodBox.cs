using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class GodBox : MonoBehaviour
{
    public List<GameObject> selected;
    public GameObject targetMarker;
    public GameObject smallGridBox;

    public Queue<Task> workList = new Queue<Task>();

    Vector3 mousePosition;

    int moveNumber;
    public enum actionState
    {
        None,
        Digging,
        Moving
    }

    public actionState currentActionState;

    void Start()
    {

    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Returns action state to nothing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentActionState = actionState.None;
        }

        //Enters dig mode
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentActionState == actionState.Digging)
            {
                currentActionState = actionState.None;
            }
            else
            {
                smallGridBox.SetActive(true);
                currentActionState = actionState.Digging;
                StartCoroutine(Dig());
            }
        }

        if (currentActionState == actionState.Moving)
        {
            
        }
    }

    IEnumerator Dig()
    {
        print("Dig Mode On");

        bool Done()
        {
            if (currentActionState != actionState.Digging)
            {
                smallGridBox.GetComponent<BoxCollider>().enabled = false;
                smallGridBox.SetActive(false);
                return true;
            }
            else if  (currentActionState == actionState.Digging)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    smallGridBox.GetComponent<BoxCollider>().enabled = true;
                }

                Vector3 snapPosition()
                {
                    Vector3 v = Vector3.zero;

                    v.x = Mathf.Round(mousePosition.x);
                    v.y = Mathf.Round(mousePosition.y);

                    return (v);
                }

                smallGridBox.transform.position = snapPosition();
            }
            return false;
        }

        yield return new WaitUntil(Done);
    }
}

public class Task
{
    public string name;
    public Vector3 taskPosition;
    public GameObject targetObject;
    public bool Completed;

    public enum taskPriority
    {
        Low,
        Medium,
        High
    }
    public enum taskTypes
    {
        Dig,
        Build,
        Carry
    }
    public taskPriority priority;
    public taskTypes taskType;
}

public class DigTask : Task
{

}
