using KrampStudio.BT.Enums;

namespace KrampStudio.BT.General
{
    /// <summary>
    /// Node class that can hold multiple Processes/Nodes and will return Success if all of its child nodes return Success. It will return Failed when one of its child processes/Nodes return Failed. When a child process return FAILED the SEQUENCE STOPS and returns FAILED.
    /// </summary>
    public class BTRepeatedSequence : BTNode
    {
        private int _currentSequence = 1;
        private int _numberOfRepetitions = default;


        /// <summary>
        /// Constructor for creating a sequence Node with a name.
        /// </summary>
        /// <param name="n"></param>
        public BTRepeatedSequence(string n)
        {
            Name = n;
        }

        /// <summary>
        /// Constructor for creating a repeated sequence Node.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="numberOfRepetitions"></param>
        public BTRepeatedSequence(string n, int numberOfRepetitions)
        {
            Name = n;
            _numberOfRepetitions = numberOfRepetitions;
        }

        /// <summary>
        /// Empty constructor for creating a Sequence node.
        /// </summary>
        public BTRepeatedSequence()
        {

        }

        /// <summary>
        /// The Process Method that "loops" over its child Nodes/Processes and starts the Process methods. If the current active node of the sequence is in running status it will return running. If the current active running node(child of the sequence that is running) return Failed, the sequence will return Failed. The Sequence will return Success only if it has run this: <see cref="_numberOfRepetitions"/> many times and the child nodes never returned FAILED (each node on every run returned SUCCESS).
        /// </summary>
        /// <returns></returns>
        public override ProcessStatusEnum Process()
        {
            ProcessStatusEnum childStatus = Children[CurrentChild].Process();
            if (childStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                return Status = ProcessStatusEnum.RUNNING;
            }
            if (childStatus.Equals(ProcessStatusEnum.FAILED))
            {
                _currentSequence = 1;
                CurrentChild = 0;
                return Status = ProcessStatusEnum.FAILED;
            }

            CurrentChild++;
            if(CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                if(_currentSequence == _numberOfRepetitions)
                {
                    _currentSequence = 1;
                    return Status = ProcessStatusEnum.SUCCESS;
                }
                _currentSequence++;
            }

            return Status = ProcessStatusEnum.RUNNING;
        }
    }
}
