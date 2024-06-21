using System;
using UnityEngine;

public class BenchMarkTest : MonoBehaviour
{
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private int _tintColor = Shader.PropertyToID("_TintColor");
    void Start()
    {
        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    void PerformBenchmarkTest()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor(_tintColor, Color.red);
        }
    }
}
