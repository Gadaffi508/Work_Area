using UnityEngine;
using System.Text;

public class StringOptimization : MonoBehaviour
{
    void Update()
    {
        string message = "Frame: " + Time.frameCount + " Time: " + Time.time;
        Debug.Log(message);
    }
    
    //<------------------------------------Optimize------------------------------------------------->
    
    private StringBuilder messageBuilder = new StringBuilder();

    void Update2()
    {
        messageBuilder.Clear();
        messageBuilder.Append("Frame: ").Append(Time.frameCount).Append(" Time: ").Append(Time.time);
        Debug.Log(messageBuilder.ToString());
    }
}
