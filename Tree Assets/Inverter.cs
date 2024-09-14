using Dawud.BT.Enums;

namespace Dawud.BT.General
{
    /// <summary>
    /// Node Class that can have only one child process/Node. It Inverts anything that its child note returns.
    /// </summary>
    public class Inverter : Node
    {
        public Inverter()
        {

        }

        public Inverter (string n)
        {
            Name = n;
        }

        public override ProcessStatusEnum Process()
        {
            ProcessStatusEnum childStatus = Children[0].Process();

            switch (childStatus)
            {
                case ProcessStatusEnum.AWAIT:
                    Status = ProcessStatusEnum.RUNNING;
                    break;
                case ProcessStatusEnum.SUCCESS:
                    Status = ProcessStatusEnum.FAILED;
                    break;
                case ProcessStatusEnum.RUNNING:
                    Status = ProcessStatusEnum.AWAIT;
                    break;
                case ProcessStatusEnum.FAILED:
                    Status = ProcessStatusEnum.SUCCESS;
                    break;
            }

            return Status;
        }
    }
}
