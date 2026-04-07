using UnityEngine;

[CreateAssetMenu(menuName = "Ring/Behavior/Normal")]
public class NormalBehavior : RingBehavior
{
    private Vector3 _position;
    private Quaternion _rotation;

    public override void Initialize(Transform ring)
    {
        _position = ring.position;
        _rotation = ring.rotation;

        ring.position = _position;
        ring.rotation = _rotation;
    }
}