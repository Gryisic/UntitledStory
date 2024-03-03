using System.Collections.Generic;

namespace Core.Utils.Graphs
{
    public class GraphNode<T>
    {
        private readonly List<GraphNode<T>> _childs;

        public IReadOnlyList<GraphNode<T>> Childs => _childs;

        public bool HasChilds => Childs is { Count: > 0 };
        
        public T Data { get; }
        public int Indention { get; }
        
        public GraphNode(T data, int indention)
        {
            Data = data;
            Indention = indention;

            _childs = new List<GraphNode<T>>();
        }

        public void SetChilds(IReadOnlyList<T> childs, int indention)
        {
            foreach (var child in childs)
            {
                GraphNode<T> node = new GraphNode<T>(child, indention);

                _childs.Add(node);
            }
        }

        public IReadOnlyList<GraphNode<T>> GetChilds()
        {
            List<GraphNode<T>> nodes = new List<GraphNode<T>>();
            
            foreach (var node in Childs)
            {
                if (node.HasChilds == false)
                    nodes.Add(node);
                else
                    nodes.AddRange(node.GetChilds());
            }

            return nodes;
        }
    }
}