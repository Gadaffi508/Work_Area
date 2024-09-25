using UnityEngine;

namespace Script.Steam
{
        public class CalculateScreen
        {
                    private float _maxRotation;

                    private float _rotationSpeed;
                    
                    public CalculateScreen(float maxRotation, float rotationSpeed)
                    {
                                _maxRotation = maxRotation;
                                
                                _rotationSpeed = rotationSpeed;
                                
                                
                    }

                    public Quaternion CalculateRotate(Quaternion rotate ,float target)
                    {
                                Quaternion targetRotation = Quaternion.Euler(0, target, 0);
                                
                                return Quaternion.Lerp(rotate, targetRotation, Time.deltaTime * _rotationSpeed);
                                
                                
                    }

                    public float CalculateOffset(Vector3 offset)
                    {
                                float screenCenter = Screen.width / 2;
                                
                                float offsetX = (offset.x - screenCenter) / screenCenter;
                                
                                return Mathf.Lerp(-_maxRotation, _maxRotation, (offsetX + 1) / 2);
                                
                                
                    }
        }
}