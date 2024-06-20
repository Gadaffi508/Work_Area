using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchMarkTest : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private Animator _anim;

    private int _attackTrigger = Animator.StringToHash("attack");

    void Start()
    {
        _anim = enemy.gameObject.GetComponent<Animator>();
    }

    void PerformBenchmarkTest()
    {
        _anim.SetTrigger(_attackTrigger);
    }
}
