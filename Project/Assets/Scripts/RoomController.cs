using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private float offset;

    private void OnTriggerExit(Collider other)
    {
        other.transform.position = transform.position + Vector3.up * offset;
    }
}
