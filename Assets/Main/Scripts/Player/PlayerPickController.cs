using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickController : MonoBehaviour
{
    [Header("Detection")]
    public float pickDistance = 3f;
    public LayerMask pickLayer;

    [Header("Hold")]
    public Transform holdPoint; // assign empty object (lower center of screen)
    public Vector3 offsetPos = new Vector3(0, -0.5f, 1.5f);

    [Header("Lerp")]
    public float lerpSpeed = 10f;

    private Camera cam;
    private IPickable currentItem;
    private Transform currentTransform;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (currentItem == null)
        {
            TryPick();
        }
    }

    private void TryPick()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickDistance, pickLayer))
        {
            IPickable pickable = hit.collider.GetComponent<IPickable>();

            if (pickable != null)
            {
                currentItem = pickable;
                currentTransform = hit.collider.transform;

                currentItem.OnPick(holdPoint);
            }
        }
    }

    private void Update()
    {
        if (currentTransform != null)
        {
            MoveToHoldPosition();
        }
    }

    private void MoveToHoldPosition()
    {
        Vector3 targetPos = holdPoint.position + holdPoint.TransformDirection(offsetPos);

        currentTransform.position = Vector3.Lerp(
            currentTransform.position,
            targetPos,
            lerpSpeed * Time.deltaTime
        );
    }
}