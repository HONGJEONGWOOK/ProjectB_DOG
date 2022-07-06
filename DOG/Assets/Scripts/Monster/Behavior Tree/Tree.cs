using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Root(�θ�)�� �ൿ�� �ϴ� object
    /// Selector ���� � �ൿ�� ������ �������ִ� ���
    /// Sequence ���� �ൿ�� ����. 
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
