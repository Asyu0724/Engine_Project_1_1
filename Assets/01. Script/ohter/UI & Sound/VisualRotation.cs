using UnityEngine;

public class VisualRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(-75.37f, 0, transform.rotation.eulerAngles.z + rotationSpeed);
    }
}
