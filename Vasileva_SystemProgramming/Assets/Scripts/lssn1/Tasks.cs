using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class Tasks : MonoBehaviour
{   
    void Start()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken ct = new();

        Task task1 = Task.Run(() => Task1Async(ct));
        Task task2 = Task.Run(() => Task2Async(ct));
        cancelTokenSource.Cancel();

        cancelTokenSource.Dispose();
    }    

    async Task Task1Async(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            Debug.Log("Операция прервана токеном");
            return;
        }

        await Task.Delay(1000);
        Debug.Log("Task 1 has waited for 1 second.");
    }

    async Task Task2Async(CancellationToken ct)
    {
        int times = 60;

        while (times > 0)
        {
            if (ct.IsCancellationRequested)
            {
                Debug.Log("Операция прервана токеном");
                return;
            }
            times--;
            await Task.Yield();
        }
        Debug.Log("Task 2 has waited for 60 frames.");
    }

}
