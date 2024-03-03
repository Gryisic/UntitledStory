using System.Collections.Generic;

namespace Core.Utils.Graphs
{
    public class Graph<T>
    {
        protected readonly GraphNode<T> root;

        public Graph(T rootData)
        {
            root = new GraphNode<T>(rootData, 0);
        }

        public IReadOnlyList<GraphNode<T>> GetNodes()
        {
            List<GraphNode<T>> nodes = new List<GraphNode<T>>();

            nodes.AddRange(root.GetChilds());
            nodes.Add(root);
            
            nodes.Reverse();

            return nodes;
        }
    }
}