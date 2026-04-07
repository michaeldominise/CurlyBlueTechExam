using UnityEngine;

public abstract class RingBehavior : ScriptableObject
{
    public virtual void Initialize(Transform ring) { }

    public virtual void Execute(Transform ring, float deltaTime) { }

    public virtual void Cleanup(Transform ring)
    {
        ring.localPosition = Vector3.zero;
        ring.localRotation = Quaternion.identity;
    }

    public virtual void OnBallReleased(Transform ring) { }
}