using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// Node class that can hold multiple children nodes and if only one of them is SUCCESSFULL then the selector will return SUCCESS. It WIll stop when atleast one child of the Selector has returned SUCCESS.
    /// </summary>
    public class Selector : Node
    {
        public Selector()
        {

        }   

        public Selector(string n)
        {
            Name = n;
        }

        public override ProcessStatusEnum Process()
        {
            ProcessStatusEnum childStatus = Children[CurrentChild].Process();
            if (childStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                Status = ProcessStatusEnum.RUNNING;
                return Status;
            }
            if (childStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                Status = ProcessStatusEnum.SUCCESS;
                CurrentChild = 0;
                return Status;
            }

            CurrentChild++;
            if(CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                Status = ProcessStatusEnum.FAILED;
                return Status;
            }

            Status = ProcessStatusEnum.RUNNING;
            return Status;
        }
    }
}
