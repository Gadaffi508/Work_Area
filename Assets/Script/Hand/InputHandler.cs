using UnityEngine;

namespace Script.Steam
{
        public class InputHandler
        {
            
                    public bool IsMouseButtonDown(int button)
                    {
                        
                                return Input.GetMouseButtonDown(button);
                                
                                
                    }

                    public bool IsEscape()
                    {
                                return Input.GetKeyDown(KeyCode.Escape);
                    }

                    public float GetMouseScrollWheelValue()
                    {
                        
                                return Input.GetAxis("Mouse ScrollWheel");
                                
                                
                    }

                    public float HandleZAxisRotation(float scrollRotationSpeed)
                    {
                        
                                if (GetMouseScrollWheelValue() != 0)
                                {
                                            return GetMouseScrollWheelValue() * scrollRotationSpeed * Time.deltaTime;
                                }

                                return 0;
                                
                                
                    }

                    public Vector3 GetMousePosition()
                    {
                        
                                return Input.mousePosition;
                                
                                
                    }
                    
                    
        }
}