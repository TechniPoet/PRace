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

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            if (object1 == null || object2 == null) return;

            // Find the midpoint between the two objects
            Vector3 midpoint = (object1.position + object2.position) / 2;

            // Calculate the distance between the two objects
            float distanceBetweenObjects = Vector3.Distance(object1.position, object2.position);

            // Adjust the camera's distance based on how far apart the objects are
            float adjustedDistance = Mathf.Clamp(distanceBetweenObjects * distanceFactor, minDistance, maxDistance);

            // Determine the desired position of the camera (midpoint + offset + adjust based on distance)
            Vector3 desiredPosition = midpoint - transform.forward * adjustedDistance + offset;

            // Smoothly move the camera to the desired position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
    }

}