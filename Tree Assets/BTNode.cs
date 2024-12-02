using KrampStudio.BT.Enums;
using System.Collections.Generic;

namespace KrampStudio.BT.General
{
    /// <summary>
    /// The base node for creating each other individual node for the Behaviour Tree. The node holds the current <see cref="Status"/> of the process, a list of all of its <see cref="Children"/>, an integer value of the <see cref="CurrentChild"/> active that is always set to 0 at the start, and the <see cref="Name"/> of the node.
    /// </summary>
    public class BTNode
    {
        public ProcessStatusEnum Status = ProcessStatusEnum.AWAIT;
        public List<BTNode> Children = new List<BTNode>();
        public int CurrentChild = 0;
        public string Name = default;
        public int SortOrder = default;

        /// <summary>
        /// Empty constructor for creating a Node.
        /// </summary>
        public BTNode()
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
        public BTNode(string n)
        {
            Name = n;
        }

        /// <summary>
        /// Constructor for the Node class that sets the name and the sorting order of the Node.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="order"></param>
        public BTNode(string n, int order)
        {
            Name = n;
            SortOrder = order;
        }

        /// <summary>
        /// Method for adding children inside the <see cref="Children"/> List.
        /// </summary>
        /// <param name="n"></param>
        public void AddChildren(BTNode n)
        {
            Children.Add(n);
        }
    }
}
