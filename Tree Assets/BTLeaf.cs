using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// Class that inherits from the Node class and can hold the Name and Action/Method of the currently running process. This is called a Leaf node and its the last node of the Behaviour tree.
    /// </summary>
    public class BTLeaf : BTNode
    {
        public delegate ProcessStatusEnum Tick(); //One execution of the the behaviour tree. A delegate that returns the Process status and its a value called Tick.

        public Tick ProcessMethod = default;//Holds the current method that is getting processed.

        /// <summary>
        /// Empty Constructor for creating a Leaf Node.
        /// </summary>
        public BTLeaf()
        {

        }

        /// <summary>
        /// Contructor for creating a leaf and setting the Leafs name and its Method that needs to be processed.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pm"></param>
        public BTLeaf(string n, Tick pm)
        {
            Name = n;
            ProcessMethod = pm;
        }

        /// <summary>
        /// Contructor for creating a leaf and setting the Leafs name, Method that needs to be processed and the order of the leaf inside its parent Node.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pm"></param>
        public BTLeaf(string n, Tick pm, int order)
        {
            Name = n;
            ProcessMethod = pm;
            SortOrder = order;
        }

        /// <summary>
        /// Method that starts the Method that is saved inside the delegate, if the Method(ProcessMethod) is empty it will return Failed. This method is called by the Behaviour tree Node. It will return the current status of the Method/Process.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            if(ProcessMethod != null)
            {
                Status = ProcessMethod();
            }
            else
            {
                Status = ProcessStatusEnum.FAILED;
            }

            return Status;
        }
    }
}
