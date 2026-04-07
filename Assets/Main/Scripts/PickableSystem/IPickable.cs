using UnityEngine;

public interface IPickable
{
    void OnPick(Transform holdPoint);
    void OnRelease(Vector3 releaseVelocity);
}
