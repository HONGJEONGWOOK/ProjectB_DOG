using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Root(부모)가 행동을 하는 object
    /// Selector 노드는 어떤 행동을 할지를 결정해주는 노드
    /// Sequence 노드는 행동을 뜻함. 
    /// </summary>


    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        protected void Start()
        {
            _root = SetupTree();
        }

        private void FixedUpdate()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}
