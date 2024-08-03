using UnityEngine;
public class CharacterAnimationOptimization : MonoBehaviour
{
    public enum CharacterState { Idle, Running, Jumping }
    public CharacterState currentStates;
    
    private ICharacterState currentState;
    void Start()
    {
        SetState(new IdleStates());
    }
    void Update()
    {
        currentState.Execute(this);
        
        switch (currentStates)
        {
            case CharacterState.Idle:
                // Idle behavior
                break;
            case CharacterState.Running:
                // Running behavior
                break;
            case CharacterState.Jumping:
                // Jumping behavior
                break;
        }
    }
    public void SetState(ICharacterState newState)
    {
        currentState = newState;
    }
}
public interface ICharacterState
{
    void Execute(CharacterAnimationOptimization character);
}
public class IdleStates : ICharacterState
{
    public void Execute(CharacterAnimationOptimization character)
    {
    }
}
public class RunningStates : ICharacterState
{
    public void Execute(CharacterAnimationOptimization character)
    {
    }
}
public class JumpingStates : ICharacterState
{
    public void Execute(CharacterAnimationOptimization character)
    {
    }
}
