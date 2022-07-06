using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children;

        private Dictionary<string, object> _dataContext = new();

        public Node()
        {
            parent = null;
        }
        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        // 노드의 현재 상태 확인을 위한 메서드. 상속받을 클래스를 위해 남겨둠.
        public virtual NodeState Evaluate() => NodeState.FAILURE;


        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            // value를 찾고
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            //value가 없으면 부모 노드의 data를 찾는다. (재귀)
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;

        }
        public bool ClearData(string key)
        {
            // value를 찾고 있으면 제거하고 true 반환.
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            //이미 clear라면 부모 노드의 key를 확인하고 제거한다. (재귀)
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared) 
                {
                    return true;
                }
                node = node.parent;
            }

            return false;
        }
    }
}
