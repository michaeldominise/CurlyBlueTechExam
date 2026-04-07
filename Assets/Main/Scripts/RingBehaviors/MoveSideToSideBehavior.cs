using UnityEngine;

[CreateAssetMenu(menuName = "Ring/Behavior/Move Side To Side")]
public class MoveSideToSideBehavior : RingBehavior
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 2f;

    private Vector3 _startPos;
    private float _time;

    public override void Initialize(Transform ring)
    {
        _startPos = ring.position;
        _time = Random.value * 10f;
    }

    public override void Execute(Transform ring, float deltaTime)
    {
        _time += deltaTime * speed;

        float offset = Mathf.Sin(_time) * distance;
        ring.position = _startPos + new Vector3(offset, 0f, 0f);
    }
}