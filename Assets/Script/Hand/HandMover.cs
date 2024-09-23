using UnityEngine;

namespace Script.Steam
{
        public class HandMover
        {
            
                    public void MoveToPosition(Transform transform, Vector3 position, float heightOffset)
                    {
                        
                                Vector3 offset = new Vector3(position.x, position.y + heightOffset, position.z);
                                
                                transform.position = offset;
                                
                                
                    }
                    
                    
        }
}