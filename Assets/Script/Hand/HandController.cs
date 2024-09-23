using Script.Steam;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCamera;
    
    public LayerMask layerMask;
    
    
    private Animator _animator;
    
    private InputHandler _inputHandler;
    
    private Raycaster _raycaster;
    
    private HandMover _handMover;
    
    
    private void Start()
    {
        
             _animator = GetComponent<Animator>();
             
             _inputHandler = new InputHandler();
             
             _raycaster = new Raycaster(mainCamera, layerMask);
             
             _handMover = new HandMover();

    }


    private void Update()
    {
                MoveObjectToMousePosition();
        
                if (_inputHandler.IsMouseButtonDown(0))
                {
                            _animator.SetTrigger("Pick");
                }
        
    }
    
    void MoveObjectToMousePosition()
    {
        
            Vector3 mousePosition = _inputHandler.GetMousePosition();
            
            
            if (_raycaster.RaycastFromScreen(mousePosition, out RaycastHit hit))
            {
                
                        _handMover.MoveToPosition(transform, hit.point, 4f);
            }
            
    }
}
