using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
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
