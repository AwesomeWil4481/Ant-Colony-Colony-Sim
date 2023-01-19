using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    MeshRenderer defaultRenderer;
    Color defaultColor;

    public float speed;

    public GameObject target;
    public GameObject pathPrefab;

    private SoulBox soulBox;
    private GodBox godBox;
    public GameObject cTarg { get; set; }

    public Task currentTask;
    public Queue<Task> personalTaskList = new Queue<Task>();

    public List<Vector3> path = new List<Vector3>();

    bool startedJourney = false;

    public enum EntityState
    {
        Idle,
        Working,
        Travelling,
        Searching,
        Resting,
        Arrived,
        Dead
    }

    public EntityState currentState;

    void Start()
    {
        defaultRenderer = gameObject.GetComponent<MeshRenderer>();

        defaultColor = defaultRenderer.material.color;

        godBox = GameObject.Find("God Box").GetComponent<GodBox>();
        soulBox = GameObject.Find("Soul Box").GetComponent<SoulBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EntityState.Idle)
        {
            if (soulBox.incomingRequest.Count != 0 || personalTaskList.Count > 0)
            {
                if (personalTaskList.Count ==0)
                {
                    foreach (Task t in soulBox.aquireTask())
                    {
                        personalTaskList.Enqueue(t);
                    }
                }
                if (personalTaskList.Count > 0)
                {
                    //currentTask = soulBox.aquireTask();

                    Task nextTask()
                    {
                        int count = 0;
                        print(personalTaskList.Count);

                        if (personalTaskList.Count > 0)
                        {
                            if (personalTaskList.Peek().Completed)
                            {
                                personalTaskList.Enqueue(personalTaskList.Dequeue());
                                count++;
                            }
                            else if (!personalTaskList.Peek().Completed)
                            {
                                return personalTaskList.Dequeue();
                            }
                            else if (count == personalTaskList.Count)
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }

                        personalTaskList.Clear();
                        return nextTask();
                    }

                    {
                        currentTask = nextTask();

                        if (currentTask != null)
                        {
                            if (currentTask.taskPosition == gameObject.transform.position)
                            {
                                currentState = EntityState.Working;
                            }
                            else
                            {
                                StartCoroutine(StartPathing(currentTask.taskPosition));
                            }
                        }
                    }
                }
            }
        }

        if (currentState == EntityState.Travelling)
        {
            if (!startedJourney)
            {
                startedJourney = true;

                MoveToTarget(path.Count - 1);
            }
        }

        if (godBox.selected.Contains(gameObject))
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = defaultColor;
        }
    }

    public IEnumerator StartPathing(Vector3 taskPos)
    {
        gameObject.GetComponent<Entity>().currentState = EntityState.Searching;
        List<Vector3> availablePaths = new List<Vector3>();
        int count = 0;

        foreach (GameObject g in GameObject.Find("Pathing").GetComponent<Pathing>().Paths)
        {
            if (Vector3.Distance(taskPos, g.transform.position) <= 1.3f)
            {
                availablePaths.Add(g.transform.position);
            }
        }

        if (availablePaths.Count > 0)
        {
            foreach (Vector3 v in availablePaths)
            {
                count++;
                print($"The {count} target has been placed");
                var t = Instantiate(target, v, Quaternion.identity);
                t.transform.SetParent(gameObject.transform);
            }
        }
        else
        {
            currentTask.Completed = false;

            List<Task> newL = new List<Task>();

            soulBox.incomingRequest.Enqueue(currentTask);

            ReQueue();
        }

        yield return null;
    }

    public void MoveToTarget(int pathLeft)
    {
        if (Vector3.Distance(path[0], gameObject.transform.position) > 0.001f)
        {
            Vector3 previousTile = path[pathLeft];
            Vector3 nextTile = path[pathLeft - 1];
            StartCoroutine(MoveTiles(previousTile, nextTile, pathLeft));
            pathLeft--;
            previousTile = nextTile;
            nextTile = path[pathLeft];
        }
        else
        {
            currentState = EntityState.Working;

            StartCoroutine(DigTask());

            print($"Arrived!");
        }
    }

    public IEnumerator MoveTiles(Vector3 startPos, Vector3 endPos, int currentPath)
    {
        var step = speed * Time.deltaTime;

        bool Arrived()
        {
            if (Vector3.Distance(gameObject.transform.position, endPos) < 0.001f)
            {
                return true;
            }
            else
            {
                step += speed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(startPos, endPos, step);
            }

            return false;
        }

        yield return new WaitUntil(Arrived);

        MoveToTarget(currentPath -1);
    }

    public IEnumerator moveCommand(GameObject target)
    {
        target.GetComponent<MoveCommand>().approachingObjects.Add(gameObject);
        var thisEntity = gameObject.GetComponent<Entity>();

        var step = thisEntity.speed * 0.1f;

        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

        bool arrived()
        {
            if (target.name != thisEntity.cTarg.name)
            {
                target.GetComponent<MoveCommand>().approachingObjects.Remove(gameObject);
                return true;
            }
            else if (Vector3.Distance(target.transform.position, transform.position) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
                return false;
            }
            else if (Vector3.Distance(target.transform.position, transform.position) < 0.001f)
            {
                target.GetComponent<MoveCommand>().approachingObjects.Remove(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        yield return new WaitUntil(arrived);

    }

    public IEnumerator DigTask()
    {
        float workSpeed = 0.1f;

        if (currentTask.targetObject != null)
        {
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.black;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.black;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.black;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.black;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.black;
            yield return new WaitForSecondsRealtime(workSpeed);
            currentTask.targetObject.GetComponent<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSecondsRealtime(workSpeed);

            Destroy(currentTask.targetObject);
            var newPath = Instantiate(pathPrefab, currentTask.taskPosition, Quaternion.identity);

            var pathingObject = GameObject.Find("Pathing");
            newPath.transform.SetParent(pathingObject.transform);
            pathingObject.GetComponent<Pathing>().Paths.Add(newPath);

            ClearState();
        }
    }

    public void ClearState()
    {
        currentState = EntityState.Idle;
        startedJourney = false;
        path.Clear();
        currentTask.Completed = true;
    }

    public void ReQueue()
    {
        currentState = EntityState.Idle;
        startedJourney = false;
        path.Clear();
        currentTask = null;
    }
}
