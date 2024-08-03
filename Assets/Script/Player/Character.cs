using System;
using UnityEngine;

public enum StateType { Empty, Idle, Jump }

public class Character : MonoBehaviour
{
    public CharacterState state;

    private void Start()
    {
        state = new EmptyState();
    }

    private void Update()
    {
        state = state.handleInput();
    }
}

public class EmptyState : CharacterState
{
    public EmptyState()
    {
        _type = StateType.Empty;
    }

    override public CharacterState handleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return new IdleState();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            return new JumpingState();
        }
        return this;
    }
}

public partial class IdleState : CharacterState
{
    public IdleState()
    {
        _type = StateType.Idle;
    }

    override public CharacterState handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return new JumpingState();
        }else if(Input.GetKeyUp(KeyCode.W))
        {
            return new EmptyState();
        }
        return this;
    }
}

public partial class JumpingState : CharacterState
{
    public JumpingState()
    {
        _type = StateType.Jump;
    }

    override public CharacterState handleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return new IdleState();
        }else if(Input.GetKeyUp(KeyCode.Space))
        {
            return new EmptyState();
        }
        return this;
    }
}

[System.Serializable]
public class CharacterState
{
    public StateType _type;
    virtual public CharacterState handleInput()
    {
        Debug.Log(_type);
        return this;
    }
}