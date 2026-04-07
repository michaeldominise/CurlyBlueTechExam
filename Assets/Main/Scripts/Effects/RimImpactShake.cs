using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RimImpactShake : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) 
    {
        if (!collision.gameObject.TryGetComponent<BallScore>(out _))
            return;
        TriggerShake();
    }

    private void TriggerShake()
    {
        if (CameraShake.Instance == null) return;
        
        CameraShake.Instance.Shake();
    }
}