using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// Node class that can hold multiple children nodes and if only one of them is SUCCESSFULL then the selector will return SUCCESS. It WIll stop when atleast one child of the Selector has returned SUCCESS.
    /// </summary>
    public class BTSelector : BTNode
    {
        public BTSelector()
        {

        }   

        public BTSelector(string n)
        {
            Name = n;
        }

        public override ProcessStatusEnum Process()
        {
            ProcessStatusEnum childStatus = Children[CurrentChild].Process();
            if (childStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                return Status = ProcessStatusEnum.RUNNING;
            }
            if (childStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                CurrentChild = 0;
                return Status = ProcessStatusEnum.SUCCESS;
            }

            CurrentChild++;
            if(CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                return Status = ProcessStatusEnum.FAILED;
            }

            return Status = ProcessStatusEnum.RUNNING;
        }
    }
}
