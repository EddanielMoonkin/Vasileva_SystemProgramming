using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public class SimpleJobTest : MonoBehaviour
{
    void Start()
    {               
        NativeArray<int> array = new NativeArray<int>(new int[] {23, 1, 5, 20, 9, 15, 7}, Allocator.TempJob);
        SimpleJob job = new SimpleJob();
        job.array = array;

        JobHandle handle = job.Schedule();
        handle.Complete();

        if (handle.IsCompleted) print("Complete");
        
        for(int i = 0; i < array.Length; i++)
        {
            Debug.Log("array" + i + " = " + array[i]);
        }
        array.Dispose();
    }

    public struct SimpleJob : IJob
    {
        public NativeArray<int> array;

        public void Execute()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > 10)
                {
                    array[i] = 0;
                }
            }
        }
    }    

}
