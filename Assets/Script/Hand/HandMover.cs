using UnityEngine;

namespace Script.Steam
{
        public class HandMover
        {
            
                    public void MoveToPosition(Transform transform, Vector3 position, float heightOffset, Vector3 minPosition, Vector3 maxPosition)
                    {
                        
                                Vector3 offset = new Vector3(position.x, position.y + heightOffset, position.z);
                                
                                offset.x = Mathf.Clamp(offset.x, minPosition.x, maxPosition.x);
                                offset.y = Mathf.Clamp(offset.y, minPosition.y, maxPosition.y);
                                offset.z = Mathf.Clamp(offset.z, minPosition.z, maxPosition.z);
                                
                                transform.position = offset;
                                
                                
                    }
                    
                    
        }
}