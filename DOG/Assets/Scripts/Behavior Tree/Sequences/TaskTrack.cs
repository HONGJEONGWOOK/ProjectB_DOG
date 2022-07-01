using UnityEngine;
using BehaviorTree;

public class TaskTrack : Node
{
    private Transform _transform = null;

    public TaskTrack(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = MonsterManager.Inst.Target;

        if (Vector3.SqrMagnitude(_transform.position - target.position) > 0.01f)
        {
            _transform.position = Vector2.MoveTowards(_transform.position, target.position
                , Monsters_BT.moveSpeed * Time.fixedDeltaTime);
        }
        state = NodeState.RUNNING;
        return state;
    }

}
