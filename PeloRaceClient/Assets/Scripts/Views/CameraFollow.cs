using UnityEngine;
namespace Views
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform object1;
        [SerializeField] private Transform object2;
        [SerializeField] private float minDistance = 5f; 
        [SerializeField] private float maxDistance = 20f; 
        [SerializeField] private float distanceFactor = 2f; 
        [SerializeField] private float smoothSpeed = 0.125f; 
        [SerializeField] private Vector3 offset;
        [SerializeField] private float _objectTensionBreak = 15;

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            if (object1 == null || object2 == null) return;

            // Calculate the distance between the two objects
            float distanceBetweenObjects = Vector3.Distance(object1.position, object2.position);

            if (_objectTensionBreak > distanceBetweenObjects)
            {
                return;
            }

            var TensionBroken = _objectTensionBreak > distanceBetweenObjects;
            Vector3 desiredPosition;

            if (TensionBroken)
            {
                // Find the midpoint between the two objects
                Vector3 midpoint = (object1.position + object2.position) / 2;

                // Adjust the camera's distance based on how far apart the objects are
                float adjustedDistance = Mathf.Clamp(distanceBetweenObjects * distanceFactor, minDistance, maxDistance);

                // Determine the desired position of the camera (midpoint + offset + adjust based on distance)
                desiredPosition = midpoint - transform.forward * adjustedDistance + offset;

                // Smoothly move the camera to the desired position
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

                // Have the camera look at the midpoint
                transform.LookAt(midpoint);
            }
            else
            {
                // Focus back on object1 with a specified offset
                desiredPosition = object1.position - transform.forward * minDistance + offset;

                // Smoothly move the camera to follow object1
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

                // Have the camera look at object1
                transform.LookAt(object1);
            }
        }
    }

}