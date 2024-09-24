using Dawud.BT.Enums;
using Dawud.BT.Misc;

namespace Dawud.BT.General
{
    /// <summary>
    /// Node class that can hold multiple children nodes and if only one of them is SUCCESSFULL then the selector will return SUCCESS. It WIll stop when atleast one child of the Selector has returned SUCCESS. On the first tick of the process it will randomly shuffle up its child nodes.
    /// </summary>
    public class BTRandomSelector : BTNode
    {
        private bool _shuffeled = false;

        public BTRandomSelector()
        {

        }   

        public BTRandomSelector(string n)
        {
            Name = n;
        }

        /// <summary>
        /// On the first time the Process is run it will shuffle its child nodes. It Returns SUCCESS when one of the child Nodes return SUCCESS.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            if (!_shuffeled)
            {
                Children.Shuffle();
                _shuffeled = true;
            }

            ProcessStatusEnum childStatus = Children[CurrentChild].Process();
            if (childStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                return Status = ProcessStatusEnum.RUNNING;
            }
            if (childStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                CurrentChild = 0;
                _shuffeled = false;
                return Status = ProcessStatusEnum.SUCCESS;
            }

            CurrentChild++;
            if(CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                _shuffeled = false;
                return Status = ProcessStatusEnum.FAILED;
            }

            return Status = ProcessStatusEnum.RUNNING;
        }
    }
}
