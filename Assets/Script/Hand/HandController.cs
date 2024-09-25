using Script.Steam;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCamera;
    
    public LayerMask layerMask;
    
    public Vector3 minPosition;
    
    public Vector3 maxPosition;
    
    public float maxRotation = 40f;
    
    public float rotationSpeed = 2f; 
    
    public float scrollRotationSpeed = 10000f; 
    
    
    private Animator _animator;
    
    private InputHandler _inputHandler;
    
    private Raycaster _raycaster;
    
    private HandMover _handMover;

    private CalculateScreen _calculateScreen;
    
    private float _currentZRotation = 0f;
    
    private bool escape = false;
    
    private void Start()
    {
        
             _animator = GetComponentInChildren<Animator>();
             
             _inputHandler = new InputHandler();
             
             _raycaster = new Raycaster(mainCamera, layerMask);

             _calculateScreen = new CalculateScreen(maxRotation, rotationSpeed);
             
             _handMover = new HandMover();

    }


    private void Update()
    {
                
        
                MoveObjectToMousePosition();

                RotateToHand();
                
                HandleZAxisRotation();  
        
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
                
                        _handMover.MoveToPosition(transform, hit.point, 4f, minPosition, maxPosition);
            }
            
    }

    void RotateToHand()
    {
        
            Quaternion targetRotation = _calculateScreen.CalculateRotate(transform.rotation,
                        
                        _calculateScreen.CalculateOffset(_raycaster.HandScreenPos(transform.position)));
            
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, _currentZRotation);
            
    }
    
    

    void HandleZAxisRotation()
    {
        
            _currentZRotation += _inputHandler.HandleZAxisRotation(scrollRotationSpeed);
            
    }
}
