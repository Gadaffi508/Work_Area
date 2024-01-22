using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseFollow : MonoBehaviour
{
    public Transform obj;
    
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 1f;
        obj.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void LoadMenu(int index)
    {
        SceneManager.LoadScene(index);
    }
}
