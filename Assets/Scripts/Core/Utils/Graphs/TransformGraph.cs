using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;

namespace Core.Utils.Graphs
{
    public class TransformGraph : Graph<Transform>
    {
        private GraphNode<Transform> _currentNode;

        public TransformGraph(Transform rootData) : base(rootData) { }

        private void Build()
        {
            List<Transform> childsTransforms = root.Data.GetChilds();
            int indention = 1;
            
            _currentNode = root;
            _currentNode.SetChilds(childsTransforms, indention);

            IReadOnlyList<GraphNode<Transform>> childs = _currentNode.Childs;

            foreach (var child in childs)
            {
                indention++;

                _currentNode = child;
                    
                indention--;
            }
        }
    }
}