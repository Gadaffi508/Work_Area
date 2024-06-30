using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOptimization : MonoBehaviour
{
    public GameObject prefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(prefab);
            Destroy(obj, 2f);
        }
    }
    
    //<------------------------------------Optimize------------------------------------------------->
    
    public GameObject prefab2;
    private Queue<GameObject> objectPool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefab2);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    void Update2()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Dequeue();
                obj.SetActive(true);
                StartCoroutine(DisableAfterTime(obj, 2f));
            }
        }
    }

    private IEnumerator DisableAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}
