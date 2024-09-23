using UnityEngine;

namespace Script.Steam
{
        public class InputHandler
        {
            
                    public bool IsMouseButtonDown(int button)
                    {
                        
                                return Input.GetMouseButtonDown(button);
                                
                                
                    }

                    public Vector3 GetMousePosition()
                    {
                        
                                return Input.mousePosition;
                                
                                
                    }
                    
                    
        }
}