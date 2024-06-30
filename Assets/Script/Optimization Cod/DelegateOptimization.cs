using System;
using UnityEngine;

public class DelegateOptimization : MonoBehaviour
{
    public delegate void UpdateDelegate();
    public UpdateDelegate OnUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
    
    //<------------------------------------Optimize------------------------------------------------->
    
    public event Action OnUpdate2;

    void Update2()
    {
        OnUpdate?.Invoke();
    }

    public void RegisterUpdate(Action updateMethod)
    {
        OnUpdate2 += updateMethod;
    }

    public void UnregisterUpdate(Action updateMethod)
    {
        OnUpdate2 -= updateMethod;
    }
}
