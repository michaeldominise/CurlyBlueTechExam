using UnityEngine;

public class ShootTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Entry, // Trigger 1
        Exit   // Trigger 2
    }

    [SerializeField] private TriggerType triggerType;
    [SerializeField] private ShootSequenceValidator validator;

    [Header("Detection")]
    [SerializeField] private string ballTag = "Ball";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ballTag)) return;

        validator?.RegisterTrigger(triggerType, other.gameObject);
    }
}