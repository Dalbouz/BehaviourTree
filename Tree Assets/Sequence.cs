using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// Node class that can hold multiple Processes/Nodes and will return Success if all of its child nodes return Success. It will return Failed when one of its child processes/Nodes return Failed.
    /// </summary>
    public class Sequence : Node
    {
        /// <summary>
        /// Constructor for creating a sequence Node with a name.
        /// </summary>
        /// <param name="n"></param>
        public Sequence(string n)
        {
            Name = n    ;
        }

        /// <summary>
        /// Empty constructor for creating a Sequence node.
        /// </summary>
        public Sequence()
        {

        }

        /// <summary>
        /// The Process Method that "loops" over its child Nodes/Processes and starts the Process methods. If the current active node of the sequence is in running status it will return running. If the current active running node(child of the sequence that is running) return Failed, the sequence will return Failed. If the current active node returns Success it will index up to the next current child and start the Process method again. When it reaches the end of the List the Sequence returns Success.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            ProcessStatusEnum childStatus = Children[CurrentChild].Process();
            if (childStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                return ProcessStatusEnum.RUNNING;
            }
            if (childStatus.Equals(ProcessStatusEnum.FAILED))
            {
                return childStatus;
            }

            CurrentChild++;
            if(CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                return ProcessStatusEnum.SUCCESS;
            }

            return ProcessStatusEnum.RUNNING;
        }
    }
}
