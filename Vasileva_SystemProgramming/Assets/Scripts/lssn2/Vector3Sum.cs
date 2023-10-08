using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public class Vector3Sum : MonoBehaviour
{
    private void Start()
    {
        NativeArray<Vector3> positions = new NativeArray<Vector3>(new Vector3[] { new Vector3(0, 0, 1), new Vector3(20, -1, 0), new Vector3(-1, 15, 1) }, Allocator.TempJob);
        NativeArray<Vector3> velocities = new NativeArray<Vector3>(new Vector3[] { new Vector3(0, -1, 0), new Vector3(0, 3, 3), new Vector3(1, 0, 0) }, Allocator.TempJob);
        NativeArray<Vector3> finalPositions = new NativeArray<Vector3>(new Vector3[3], Allocator.TempJob);

        Vector3Job job = new Vector3Job();
        job.array1 = positions;
        job.array2 = velocities;
        job.sum = finalPositions;

        JobHandle handle = job.Schedule(3, 3);
        handle.Complete();

        if (handle.IsCompleted) print("Complete");

        for(int i = 0; i < finalPositions.Length; i++)
        {
            Debug.Log("finalPositions" + i + " = " + finalPositions[i]);
        }

        positions.Dispose();
        velocities.Dispose();
        finalPositions.Dispose();

    }

    public struct Vector3Job : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> array1;
        public NativeArray<Vector3> array2;

        [WriteOnly]
        public NativeArray<Vector3> sum;

        public void Execute(int i)
        {
            sum[i] = array1[i] + array2[i];
        }
    }

}
