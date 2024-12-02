using KrampStudio.BT.Enums;

namespace KrampStudio.BT.General
{
    /// <summary>
    /// Node where it sets the Current child of the <see cref="Node"/> to the <see cref="ChildIndex"/> if the <see cref="IsConditionMeet"/> is true. It will return <see cref="ProcessStatusEnum.RUNNING"/> so that the tree just keep running. If the condition is false it skip the node child setup and return <see cref="ProcessStatusEnum.SUCCESS"/> so that it can go to the second node.
    /// </summary>
    public class BTRepeatNodeFromIndexWithCondition : BTNode
    {
        public BTNode Node = default;
        public int ChildIndex = default;
        public bool IsConditionMeet = default;

        /// <summary>
        /// Constructor to create <see cref="BTRepeatNodeFromIndexWithCondition"/> where it sets the Current child of the <see cref="Node"/> to the <see cref="ChildIndex"/> if the <see cref="IsConditionMeet"/> is true. It will return <see cref="ProcessStatusEnum.RUNNING"/> so that the tree just keep running. If the condition is false it skip the node child setup and return <see cref="ProcessStatusEnum.SUCCESS"/> so that it can go to the second node.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="node"></param>
        /// <param name="i"></param>
        /// <param name="condition"></param>
        public BTRepeatNodeFromIndexWithCondition(string n, BTNode node, int i, bool condition = true)
        {
            Name = n;
            Node = node;
            ChildIndex = i;
            IsConditionMeet = condition;
        }

        public override ProcessStatusEnum Process()
        {
            if (IsConditionMeet)
            {
                Node.CurrentChild = ChildIndex;
                return ProcessStatusEnum.RUNNING;
            }
            return ProcessStatusEnum.SUCCESS;
        }
    }
}
