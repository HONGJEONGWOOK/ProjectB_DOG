using UnityEngine;
using BehaviorTree;

public class TaskDetect : Node
{
    private Transform _transform = null;

    //private Transform target = null;


    public TaskDetect(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object tar = GetData("target");
        
        if (tar == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            Collider2D collider = MonsterManager.Inst.DetectCollider;
            parent.parent.SetData("target", collider.transform);
            state = NodeState.SUCCESS;
            return state;
        }
    }

}
