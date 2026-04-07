using UnityEngine;

[CreateAssetMenu(menuName = "Ring/Behavior/Tilt")]
public class TiltBehavior : RingBehavior
{
    [SerializeField] private Vector3 tiltEuler = new Vector3(0f, 0f, 15f);

    private Quaternion _originalRotation;

    public override void Initialize(Transform ring)
    {
        _originalRotation = ring.rotation;
        ring.rotation = _originalRotation * Quaternion.Euler(tiltEuler);
    }

    public override void Cleanup(Transform ring)
    {
        ring.rotation = _originalRotation;
    }
}