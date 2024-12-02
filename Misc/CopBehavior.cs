using KrampStudio.BT.Actions;
using KrampStudio.BT.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KrampStudio.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public class CopBehavior : NPCRoot
    {
        [SerializeField] private int _numbOfPatrolPoints = default;
        [SerializeField] private List<GameObject> _patrolPoints = new List<GameObject>();
        private int _currentPatrolPoint = 0;
        private bool _arePatrolPointsSet = false;
        private bool _isChasing = false;
        private NPCRoot _chasingRobber = default;
        private BTRepeatNodeFromIndexWithCondition _repeateChaseWithCond = default;

        public NPCRoot ChasingRobber
        {
            get { return _chasingRobber; }
        }

        protected override void CreateBehavior()
        {
            BTSelector MainPatrolSel = new BTSelector("Main Patrol Selector");
            BTInverter SetPatrolPointsInver = new BTInverter("Set Patrol points Inverter");
            BTRepeatedSequence RepeatedPatrolSeq = new BTRepeatedSequence("Repeated Patrol Sequence", -1);
            BTSequence ChaseRobberSeq = new BTSequence("Chase Robber Sequence");
            BTRepeatNodeFromIndexWithCondition ReturnBTToPatrolNode = new BTRepeatNodeFromIndexWithCondition("Return to patrol node", _tree, 0, true);
            _repeateChaseWithCond = new BTRepeatNodeFromIndexWithCondition("Repeat chase Robber with condition", ChaseRobberSeq, 0);

            BTLeaf ChaseRobberLeaf = new BTLeaf("Chase Robber", TryToCatchRobber);
            BTLeaf GoToPatrolPointLeaf = new BTLeaf("Go To Patrol Point", GoToPatrolPoint);
            BTLeaf SetPatrolPointsLeaf = new BTLeaf("Set Patrol Points", SetPatrolPoints);

            SetPatrolPointsInver.AddChildren(SetPatrolPointsLeaf);
            RepeatedPatrolSeq.AddChildren(GoToPatrolPointLeaf);

            MainPatrolSel.AddChildren(SetPatrolPointsInver);
            MainPatrolSel.AddChildren(RepeatedPatrolSeq);

            ChaseRobberSeq.AddChildren(ChaseRobberLeaf);
            ChaseRobberSeq.AddChildren(_repeateChaseWithCond);
            ChaseRobberSeq.AddChildren(ReturnBTToPatrolNode);

            if (_tree.Children.Count <= 0)
            {
                _tree.AddChildren(MainPatrolSel);
                _tree.AddChildren(ChaseRobberSeq);
            }
        }

        private ProcessStatusEnum SetPatrolPoints()
        {
            if (!_arePatrolPointsSet)
            {
                _patrolPoints = ItemManager.Instance.GetAvailablePatrolPoints(_numbOfPatrolPoints);
                _arePatrolPointsSet = true;
            }

            return ProcessStatusEnum.SUCCESS;
        }

        private ProcessStatusEnum GoToPatrolPoint()
        {
            if(_currentPatrolPoint >= _patrolPoints.Count)
            {
                _currentPatrolPoint = 0;
            }

            ProcessStatusEnum status = GenericActions.GoToDestination(_patrolPoints[_currentPatrolPoint].transform.position, this);

            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                _currentPatrolPoint++;
            }
            else if (status.Equals(ProcessStatusEnum.FAILED))
            {
                _currentPatrolPoint = 0;
            }

            return status;
        }

        private ProcessStatusEnum TryToCatchRobber()
        {
            ProcessStatusEnum status = GenericActions.GoToDestination(_chasingRobber.transform.position, this);

            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                //he got the robber
                _chasingRobber = null;
                _isChasing = false;
                _repeateChaseWithCond.IsConditionMeet = false;//it wont repeat the process
            }
            else
            {
                _repeateChaseWithCond.IsConditionMeet = true;
            }

            return status;
        }

        protected override void SawOtherAgent()
        {
            Debug.Log(this.name + " Saw: " + SeenAgents[0].name);

            if (_isChasing)
            {
                return;
            }
            _tree.JumpToNode(1);
            _isChasing = true;
            _chasingRobber = SeenAgents[0];
        }
    }
}
