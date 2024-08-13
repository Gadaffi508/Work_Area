using System;
using System.Collections.Generic;
using UnityEngine;
public class ConfigurationManager : MonoBehaviour
{
    private static readonly Lazy<ConfigurationManager> lazyInstance =
        new Lazy<ConfigurationManager>(() => new ConfigurationManager());
    public static ConfigurationManager Instance => lazyInstance.Value;
    private Dictionary<string, string> configurations;
    private ConfigurationManager()
    {
        LoadConfigurations();
    }
    private void LoadConfigurations()
    {
        // Load configurations from file
        configurations = new Dictionary<string, string>
        {
            { "serverAddress", "192.168.1.1" },
            { "port", "8080" }
        };
    }
    public string GetConfiguration(string key)
    {
        configurations.TryGetValue(key, out string value);
        return value;
    }
}
// Usage
public class GameManager : MonoBehaviour
{
    void Start()
    {
        string serverAddress = ConfigurationManager.Instance.GetConfiguration("serverAddress");
        Debug.Log("Server Address: " + serverAddress);
    }
}