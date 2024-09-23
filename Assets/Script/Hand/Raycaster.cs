using UnityEngine;

namespace Script.Steam
{
        public class Raycaster
        {
                    private Camera _camera;
                    
                    private LayerMask _layerMask;

                    
                    
                    public Raycaster(Camera camera, LayerMask layerMask)
                    {
                        
                                _camera = camera;
                                
                                _layerMask = layerMask;
                                
                                
                    }

                    public bool RaycastFromScreen(Vector3 screenPosition, out RaycastHit hit)
                    {
                        
                                Ray ray = _camera.ScreenPointToRay(screenPosition);
                                
                                return Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);
                                
                                
                    }
        }
}