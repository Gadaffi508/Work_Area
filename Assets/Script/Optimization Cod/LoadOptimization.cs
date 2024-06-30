using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LoadOptimization : MonoBehaviour
{
    void Start()
    {
        string data = File.ReadAllText("path/to/large/file.txt");
        ProcessData(data);
    }

    void ProcessData(string data)
    {
        // processing data...
    }
    
    //<------------------------------------Optimize------------------------------------------------->
    
    async void Start2()
    {
        string data = await LoadDataAsync("path/to/large/file.txt");
        ProcessData(data);
    }

    async Task<string> LoadDataAsync(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            return await reader.ReadToEndAsync();
        }
    }

    void ProcessData2(string data)
    {
        // processing data...
    }
    
}
