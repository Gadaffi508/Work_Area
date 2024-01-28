using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[System.Serializable]
public class Obj
{
    public Transform pos;
    public float moveY;
}
public class JopManager : MonoBehaviour
{
    private bool convertBool = false;
    public Transform pObj;
    private List<Obj> _Obj;
    private void Start()
    {
        _Obj = new List<Obj>();
        for (int i = 0; i < 50; i++)
        {
            Transform objPos = Instantiate(pObj,new Vector3(Random.Range(-8f,8f),1,Random.Range(-5f,5f)),Quaternion.identity);
            _Obj.Add(new Obj
            {
                pos = objPos,
                moveY = Random.Range(1f,2f)
            });
        }
    }

    private void Update()
    {
        float startTme = Time.realtimeSinceStartup;

        if (convertBool)
        {
            NativeArray<float3> positionArray = new NativeArray<float3>(_Obj.Count,Allocator.TempJob);
            NativeArray<float> moveYArray = new NativeArray<float>(_Obj.Count,Allocator.TempJob);

            for (int i = 0; i < _Obj.Count; i++)
            {
                positionArray[i] = _Obj[i].pos.position;
                moveYArray[i] = _Obj[i].moveY;
            }
            
            ReallyToughParalleJob reallyToughParalleJob = new ReallyToughParalleJob
            {
                deltaTime = Time.deltaTime,
                positionArray = positionArray,
                moveYArray = moveYArray
            };

            JobHandle jobHandle = reallyToughParalleJob.Schedule(_Obj.Count,50);
            jobHandle.Complete();

            for (int i = 0; i < _Obj.Count; i++)
            {
                _Obj[i].pos.position = positionArray[i];
                _Obj[i].moveY = moveYArray[i];
            }

            positionArray.Dispose();
            moveYArray.Dispose();
        }
        else
        {
            foreach (Obj obj in _Obj)
            {
                obj.pos.position += new Vector3(0, 0, obj.moveY * Time.deltaTime);

                if (obj.pos.position.z > 5f) obj.moveY = -math.abs(obj.moveY);
                if (obj.pos.position.z < -5f) obj.moveY = +math.abs(obj.moveY);

                float value = 0f;

                for (int i = 0; i < 5000; i++)
                {
                    value = math.exp10(math.sqrt(value));
                }
            }
        }
        
        Debug.Log(((Time.realtimeSinceStartup - startTme) * 1000) + "ms");
    }

    private void ReallyToughTask()
    {
        float value = 0f;

        for (int i = 0; i < 5000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
    
    private JobHandle ReallyToughTaskJob()
    {
        ReallyToughJob job = new ReallyToughJob();
        return job.Schedule();
    }

    public void DoOrNoJobHandle()
    {
        convertBool = !convertBool;
    }
}

[BurstCompatible]
public struct ReallyToughJob : IJob
{
    public void Execute()
    {
        float value = 0f;

        for (int i = 0; i < 5000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}

[BurstCompatible]
public struct ReallyToughParalleJob : IJobParallelFor
{
    public NativeArray<float3> positionArray;
    public NativeArray<float> moveYArray;
    public float deltaTime;
    
    public void Execute(int index)
    {
        positionArray[index] += new float3(0,0,moveYArray[index] * deltaTime);

        if (positionArray[index].z > 5f) moveYArray[index] = -math.abs(moveYArray[index]);
        if (positionArray[index].z < -5f) moveYArray[index] = +math.abs(moveYArray[index]);
            
        float value = 0f;

        for (int i = 0; i < 5000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}
