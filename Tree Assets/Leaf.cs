using Dawud.BT.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawud.BT.General
{
    /// <summary>
    /// Class that inherits from the Node class and can hold the Name and Action/Method of the currently running process. This is called a Leaf node and its the last node of the Behaviour tree.
    /// </summary>
    public class Leaf : Node
    {
        public delegate ProcessStatusEnum Tick(); //One execution of the the behaviour tree. A delegate that returns the Process status and its a value called Tick.
        public Tick ProcessMethod = default;//Holds the current method that is getting processed.

        /// <summary>
        /// Empty Constructor for creating a Leaf Node.
        /// </summary>
        public Leaf()
        {

        }

        /// <summary>
        /// Contructor for creating a leaf and setting the Leafs name and its Method that needs to be processed.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pm"></param>
        public Leaf(string n, Tick pm)
        {
            Name = n;
            ProcessMethod = pm;
        }

        /// <summary>
        /// Method that starts the Method that is saved inside the delegate, if the Method(ProcessMethod) is empty it will return Failed. This method is called by the Behaviour tree Node. It will return the current status of the Method/Process.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            if(ProcessMethod != null)
            {
                return ProcessMethod();
            }
            return ProcessStatusEnum.FAILED;
        }
    }
}
