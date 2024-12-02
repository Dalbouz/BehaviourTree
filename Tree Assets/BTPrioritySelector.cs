using KrampStudio.BT.Enums;

namespace KrampStudio.BT.General
{
    /// <summary>
    /// Node class that can hold multiple children nodes and if only one of them is SUCCESSFULL then the selector will return SUCCESS. It WIll stop when at least one child of the Selector has returned SUCCESS. This has a priority sorting algorithm, so the created Nodes inside the Children list need to have a given sorting order (int) so that it can sort out the child nodes into the correct order for execution.
    /// </summary>
    public class BTPrioritySelector : BTNode
    {
        private BTNode[] _nodeArray = default;
        private bool _ordered = false;

        public BTPrioritySelector()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        public BTPrioritySelector(string n)
        {
            Name = n;
        }

        /// <summary>
        /// Order the nodes inside the Children list so that they match the given order.
        /// </summary>
        public void OrderNodes()
        {
            _nodeArray = Children.ToArray();
            Sort(_nodeArray, 0, Children.Count - 1);
            Children = new System.Collections.Generic.List<BTNode>(_nodeArray);
        }

        /// <summary>
        /// On the first run of this process it will order the child nodes accordingly how they where declared when created.
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            if (!_ordered)
            {
                OrderNodes();
                _ordered = true;
            }

            ProcessStatusEnum childStatus = Children[CurrentChild].Process();
            if (childStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                return Status = ProcessStatusEnum.RUNNING;
            }
            if (childStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                CurrentChild = 0;
                _ordered = false;
                return Status = ProcessStatusEnum.SUCCESS;
            }

            CurrentChild++;
            if(CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                _ordered = false;
                return Status = ProcessStatusEnum.FAILED;
            }

            return Status = ProcessStatusEnum.RUNNING;
        }

        //QuickSort
        //Adapted from: https://exceptionnotfound.net/quick-sort-csharp-the-sorting-algorithm-family-reunion/
        int Partition(BTNode[] array, int low,
                                    int high)
        {
            BTNode pivot = array[high];

            int lowIndex = (low - 1);

            //2. Reorder the collection.
            for (int j = low; j < high; j++)
            {
                if (array[j].SortOrder <= pivot.SortOrder)
                {
                    lowIndex++;

                    BTNode temp = array[lowIndex];
                    array[lowIndex] = array[j];
                    array[j] = temp;
                }
            }

            BTNode temp1 = array[lowIndex + 1];
            array[lowIndex + 1] = array[high];
            array[high] = temp1;

            return lowIndex + 1;
        }

        void Sort(BTNode[] array, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high);
                Sort(array, low, partitionIndex - 1);
                Sort(array, partitionIndex + 1, high);
            }
        }
    }
}
