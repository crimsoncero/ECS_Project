using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;

[BurstCompile]
public class PrimeChecker : MonoBehaviour
{
    public long Number;
    private Stopwatch stopWatch = new Stopwatch();
    [BurstCompile]
    public void CheckNumberParralel(long number)
    {
        List<long> divisorsList = new List<long>();
        for (long i = 2; i <= number; i++)
        {
            if (i * i <= number)
            {
                divisorsList.Add(i);
            }
            else
            {
                break;
            }
        }
        
        var divisors = new NativeArray<long>(divisorsList.ToArray(), Allocator.Persistent);
        var hasDivised = new NativeArray<bool>(divisors.Length, Allocator.Persistent);
        for (var i = 0; i < divisors.Length; i++)
        {
            divisors[i] = i + 2;
        }

        var job = new PrimeCheckJobFor()
        {
            Number = number,
            HasDivised = hasDivised,
            Divisors = divisors,
        };
        stopWatch.Reset();
        stopWatch.Start();
        JobHandle primeCheckJobHandle = job.ScheduleParallelByRef(divisors.Length, 1, default);


        StartCoroutine(LogCheckResult(primeCheckJobHandle, hasDivised, divisors, number));
        


    }

    public void CheckNumberPrime(long number)
    {
        List<long> divisors = new List<long>();
        for (long i = 2; i <= number; i++)
        {
            if (i * i <= number)
            {
                divisors.Add(i);
            }
            else
            {
                break;
            }
        }
        stopWatch.Reset();
        stopWatch.Start();
        bool isPrime = true;
        for (int i = 0; i < divisors.Count; i++)
        {
            if (number % divisors[i] == 0)
            {
                isPrime = false;
                
            }
        }
        stopWatch.Stop();
        
        if(isPrime)
        {
            Debug.Log($"{number} is a prime");
        }
        else
        {
            Debug.Log($"{number} is NOT a prime");
        }
        Debug.Log($"The check took {stopWatch.ElapsedMilliseconds} ms without jobs");
    }
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Check Number using job"))
        {
            CheckNumberParralel(Number);
        }

        if (GUI.Button(new Rect(10, 50, 100, 30), "Check Number without job"))
        {
            CheckNumberPrime(Number);
        }
    }

    private IEnumerator LogCheckResult(JobHandle checkHandle, NativeArray<bool> hasDivised, NativeArray<long> divisors, long number)
    {
        yield return new WaitUntil(() => checkHandle.IsCompleted);
        
        checkHandle.Complete();
        bool flag = false;
        for (int i = 0; i < hasDivised.Length; i++)
        {
            if (hasDivised[i])
            {
                flag = true;
                break;
            }
        }
        stopWatch.Stop();
        
        if (flag)
        {
            Debug.Log($"{number} is NOT a prime");
        }
        else
        {
            Debug.Log($"{number} is a prime");
        }
        
        Debug.Log($"The check took {stopWatch.ElapsedMilliseconds} ms with jobs");
        hasDivised.Dispose();
        divisors.Dispose();
    }
}
[BurstCompile]
public struct PrimeCheckJobFor : IJobFor
{
    [ReadOnly]
    public NativeArray<long> Divisors;

    public NativeArray<bool> HasDivised;
    public long Number;
    [BurstCompile]
    public void Execute(int index)
    {
        if (Number % Divisors[index] == 0)
        {
            // Not prime
            HasDivised[index] = true;
        }
        else
        {
            // Could be prime
            HasDivised[index] = false;
        }
    }
    
}
