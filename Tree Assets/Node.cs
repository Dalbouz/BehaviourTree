using Dawud.BT.Enums;
using System.Collections.Generic;

namespace Dawud.BT.General
{
    /// <summary>
    /// The base node for for creating each other individual node for the Behaviour Tree. The node holds the current status of the process, a list of all of its children, an integer value of the current active children that is always set to 0, and the Name of the node.
    /// </summary>
    public class Node
    {
        public ProcessStatusEnum Status = ProcessStatusEnum.AWAIT;
        public List<Node> Children = new List<Node>();
        public int CurrentChild = 0;
        public string Name = default;

        /// <summary>
        /// Empty constructor for creating a Node.
        /// </summary>
        public Node()
        {
            
        }

        /// <summary>
        /// Virtual method that returns the current <see cref="ProcessStatusEnum"/> of the process.
        /// </summary>
        /// <returns></returns>
        public virtual ProcessStatusEnum Process()
        {
            return Children[CurrentChild].Process();
        }

        /// <summary>
        /// Constructor for the Node class that can set the name of the Node.
        /// </summary>
        /// <param name="n"></param>
        public Node(string n)
        {
            Name = n;
        }

        /// <summary>
        /// Method for adding children inside the <see cref="Children"/> List.
        /// </summary>
        /// <param name="n"></param>
        public void AddChildren(Node n)
        {
            Children.Add(n);
        }
    }
}
