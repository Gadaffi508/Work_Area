using UnityEngine;

public class RaycastOptimization : MonoBehaviour
{
    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
        foreach (RaycastHit hit in hits)
        {
            // İşlemler...
        }
    }
    
    //<------------------------------------Optimize------------------------------------------------->
    
    private RaycastHit[] hits = new RaycastHit[10];

    void Update2()
    {
        int hitCount = Physics.RaycastNonAlloc(transform.position, transform.forward, hits);
        for (int i = 0; i < hitCount; i++)
        {
            // Do something
        }
    }
}
