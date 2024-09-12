using Dawud.BT.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Dawud.BT.General
{
    /// <summary>
    /// Node class for creating a Main Node(Behaviour Tree) that holds all of its children nodes.
    /// </summary>
    public class BehaviourTree : Node
    {
        /// <summary>
        /// Struct that holds a node and its current level inside the Behaviour tree.
        /// </summary>
        private struct NodeLevel
        {
            public int Level;
            public Node node;
        }

        /// <summary>
        /// Constructor for the behaviour tree class that sets the name of the node.
        /// </summary>
        public BehaviourTree()
        {
            Name = "Tree";
        }

        /// <summary>
        /// Constructor for the behaviour tree class that can set the name of the node.
        /// </summary>
        /// <param name="n"></param>
        public BehaviourTree(string n)
        {
            Name = n;
        }

        /// <summary>
        /// Method that calls the Process method of the current active child inside its Children list. Returns the current Status of the current process running.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            Status = Children[CurrentChild].Process();
            return Status;
        }

        /// <summary>
        /// Sets all of the children nodes to Process status <see cref="ProcessStatusEnum.AWAIT"/> and sets its Current child to 0.
        /// </summary>
        public void SetAllToDefaultValues()
        {
            Stack<Node> nodeStack = new Stack<Node>();
            nodeStack.Push(this);
            while(nodeStack.Count != 0)
            {
                Node nextNode = nodeStack.Pop();
                nextNode.Status = ProcessStatusEnum.AWAIT;
                nextNode.CurrentChild = 0;

                for (int i = 0; i < nextNode.Children.Count; i++)
                {
                    nodeStack.Push(nextNode.Children[i]);
                }
            }
        }

        /// <summary>
        /// Method for debug loging of the Behaviour tree.
        /// </summary>
        public void PrintTree()
        {
            string treePrintout = "";
            Stack<NodeLevel> nodeStack = new Stack<NodeLevel>(); //LIFO (Last in First Out)
            Node currentNode = this;
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
