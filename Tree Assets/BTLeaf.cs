using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// Class that inherits from the Node class and can hold the Name and Action/Method of the currently running process. This is called a Leaf node and its the last node of the Behavior tree.
    /// </summary>
    public class BTLeaf : BTNode
    {
        public delegate ProcessStatusEnum Tick(); //One execution of the behavior tree. A delegate that returns the Process status and its a value called Tick.
    
        public Tick ProcessMethod = default;//Holds the current method that is getting processed.

        public delegate ProcessStatusEnum TickX(int value);
        public TickX ProcessMethodX;
        public int Index;

        /// <summary>
        /// Empty Constructor for creating a Leaf Node.
        /// </summary>
        public BTLeaf()
        {

        }

        /// <summary>
        /// Constructor for creating a leaf and setting the Leafs name and its Method that needs to be processed.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pm"></param>
        public BTLeaf(string n, Tick pm)
        {
            Name = n;
            ProcessMethod = pm;
        }

        /// <summary>
        /// Constructor for creating a leaf and setting the Leafs name and its Method that can take a int value.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pm"></param>
        /// <param name="i"></param>
        public BTLeaf(string n,int i, TickX pm)
        {
            Name = n;
            ProcessMethodX = pm;
            Index = i;
        }

        /// <summary>
        /// Constructor for creating a leaf and setting the Leafs name, Method that needs to be processed and the order of the leaf inside its parent Node.
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
        /// Method that starts the Method that is saved inside the delegate, if the Method(ProcessMethod) is empty it will return Failed. This method is called by the Behavior tree Node. It will return the current status of the Method/Process. It can also call the ProcessMethodX that holds an int value.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            if(ProcessMethod != null)
            {
                Status = ProcessMethod();
            }
            else if(ProcessMethodX != null)
            {
                Status = ProcessMethodX(Index);
            }
            else
            {
                Status = ProcessStatusEnum.FAILED;
            }

            return Status;
        }
    }
}
