using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class TheFastestTask : MonoBehaviour
{   
    async void Start()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken ct = cancelTokenSource.Token;

        Task<bool> check = WhatTaskFasterAsync(ct, Task1Async(ct), Task2Async(ct));

        await Task.WhenAll(check);
        Debug.Log("bool check is " + check.Result);

        cancelTokenSource.Dispose();
    }

    public static async Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
    {
        CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);        
        CancellationToken linkedCt = linkedCts.Token;
        bool result;

        //task1 = Task1Async(linkedCt);
        //task2 = Task2Async(linkedCt);
        Task finishedTask = await Task.WhenAny(task1, task2);    
        linkedCts.Cancel();

        result = (finishedTask == task1);

        return result;        
    }


    async Task Task1Async(CancellationToken ct)
    {
        await Task.Delay(1000);

        if (ct.IsCancellationRequested)
        {
            Debug.Log("Операция task1 прервана токеном");
            return;
        }

        Debug.Log("Task 1 has waited for 1 second.");
    }

    async Task Task2Async(CancellationToken ct)
    {
        int times = 60;

        while (times > 0)
        {
            if (ct.IsCancellationRequested)
            {
                Debug.Log("Операция task2 прервана токеном");
                return;
            }
            times--;
            await Task.Yield();
        }
        Debug.Log("Task 2 has waited for 60 frames.");
    }

}
