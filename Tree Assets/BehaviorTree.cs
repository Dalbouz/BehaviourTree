using KrampStudio.BT.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace KrampStudio.BT.General
{
    /// <summary>
    /// Node class for creating a Main Node(Behavior Tree) that holds all of its children nodes.
    /// </summary>
    public class BehaviorTree : BTNode
    {
        /// <summary>
        /// Struct that holds a node and its current level inside the Behavior tree.
        /// </summary>
        private struct NodeLevel
        {
            public int Level;
            public BTNode node;
        }

        /// <summary>
        /// Constructor for the behavior tree class that sets the name of the node.
        /// </summary>
        public BehaviorTree()
        {
            Name = "Tree";
        }

        /// <summary>
        /// Constructor for the behavior tree class that can set the name of the node.
        /// </summary>
        /// <param name="n"></param>
        public BehaviorTree(string n)
        {
            Name = n;
        }

        /// <summary>
        /// Method that calls the Process method of the current active child inside its Children list. Returns the current Status of the current process running.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            if (Children.Count == 0) return Status = ProcessStatusEnum.SUCCESS;
            return Status = Children[CurrentChild].Process();
        }

        /// <summary>
        /// Use to jump to another child of the Behavior tree caused by some other event.
        /// </summary>
        /// <param name="nodeIndex"></param>
        public void JumpToNode(int nodeIndex)
        {
            CurrentChild = nodeIndex;
        }

        /// <summary>
        /// Sets all of the children nodes to Process status <see cref="ProcessStatusEnum.AWAIT"/> and sets its Current child to 0.
        /// </summary>
        public void ResetTree()
        {
            Stack<BTNode> nodeStack = new Stack<BTNode>();
            nodeStack.Push(this);
            while(nodeStack.Count != 0)
            {
                BTNode nextNode = nodeStack.Pop();
                nextNode.Status = ProcessStatusEnum.AWAIT;
                nextNode.CurrentChild = 0;
                nextNode.SortOrder = 0;

                for (int i = 0; i < nextNode.Children.Count; i++)
                {
                    nodeStack.Push(nextNode.Children[i]);
                }
            }
        }

        /// <summary>
        /// Method for debug logging of the Behavior tree.
        /// </summary>
        public void PrintTree()
        {
            string treePrintout = "";
            Stack<NodeLevel> nodeStack = new Stack<NodeLevel>(); //LIFO (Last in First Out)
            BTNode currentNode = this;
            nodeStack.Push(new NodeLevel { Level = 0, node = currentNode });

            while (nodeStack.Count != 0)
            {
                NodeLevel nextNode = nodeStack.Pop();
                treePrintout += new string('-', nextNode.Level) + nextNode.node.Name + "\n";
                for (int i = nextNode.node.Children.Count - 1; i >= 0; i--)
                {
                    nodeStack.Push(new NodeLevel { Level = nextNode.Level + 1, node = nextNode.node.Children[i] });
                }
            }

            Debug.Log(treePrintout);
        }
    }
}
