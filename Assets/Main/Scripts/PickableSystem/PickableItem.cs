using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable
{
    private bool isPicked = false;

    public void OnPick(Transform holdPoint)
    {
        if (isPicked) return;

        isPicked = true;

        // Disable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Disable collider (optional)
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Parent to hold point
        transform.SetParent(holdPoint);
    }
}