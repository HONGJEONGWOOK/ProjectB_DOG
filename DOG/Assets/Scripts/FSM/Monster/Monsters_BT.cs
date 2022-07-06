using System.Collections.Generic;
using BehaviorTree;

public class Monsters_BT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float moveSpeed = 2.0f;
    public static float detectRange = 3.0f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskDetect(transform),
            }),
            new TaskPatrol(transform, waypoints),
        });
        return root;
    }
}
