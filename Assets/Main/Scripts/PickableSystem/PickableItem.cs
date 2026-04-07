using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable
{
    private bool isPicked;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void OnPick(Transform holdPoint)
    {
        if (isPicked) return;

        isPicked = true;

        if (rb != null)
        {
            // Reset motion
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();

            rb.isKinematic = true;
        }

        if (col != null)
        {
            col.enabled = false;
        }

        transform.SetParent(holdPoint);
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        if (!isPicked) return;

        isPicked = false;

        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;

            // Restore motion
            rb.linearVelocity = releaseVelocity;
        }

        if (col != null)
        {
            col.enabled = true;
        }
    }
}