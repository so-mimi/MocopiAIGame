using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public static void Enqueue(Action action)
    {
        lock (ExecutionQueue)
        {
            ExecutionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        while (ExecutionQueue.Count > 0)
        {
            Action action = null;
            lock (ExecutionQueue)
            {
                if (ExecutionQueue.Count > 0)
                {
                    action = ExecutionQueue.Dequeue();
                }
            }

            action?.Invoke();
        }
    }
}