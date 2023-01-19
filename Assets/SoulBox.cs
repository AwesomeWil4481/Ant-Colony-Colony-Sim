using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SoulBox : MonoBehaviour
{
    public SoulBox instance;
    public Queue<Task> incomingRequest = new Queue<Task>();
    private Queue<Task> highPriorityQueue = new Queue<Task>();
    private Queue<Task> medPriorityQueue = new Queue<Task>();
    private Queue<Task> lowPriorityQueue = new Queue<Task>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public List<Task> aquireTask()
    {
        if (incomingRequest.Count != 0)
        {
            List<Task> newTask = new List<Task>();

            int count = 0;
            while (count < 6)
            {
                if (incomingRequest.Count != 0)
                {
                    if (newTask.Count > 0)
                    {
                        if (Vector3.Distance(incomingRequest.Peek().taskPosition, newTask[newTask.Count - 1].taskPosition) <= 1.3f)
                        {
                            newTask.Add(incomingRequest.Dequeue()); 
                        }
                        else
                        {
                            return newTask;
                        }
                    }
                    else
                    {
                        newTask.Add(incomingRequest.Dequeue());
                    }
                }
                count++;
            }

            return newTask;
        }
        else
        {
            return null;
        }
    }
}
