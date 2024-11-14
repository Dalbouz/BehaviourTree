using Dawud.BT.Actions;
using Dawud.BT.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public class CopBehavior : NPCRoot
    {
        [SerializeField] private List<GameObject> _patrolPoints = new List<GameObject>();
        [SerializeField] private int _numbOfPatrolPoints = default;
        private int _currentPatrolPoint = 0;
        private bool _arePatrolPointsSet = false;

        protected override void CreateBehavior()
        {
            BTSelector MainPatrolSel = new BTSelector("Main Patrol Selector");
            BTInverter SetPatrolPointsInver = new BTInverter("Set Patrol points Inverter");
            BTRepeatedSequence RepeatedPatrolSeq = new BTRepeatedSequence("Repeated Patrol Sequence", -1);
            BTSelector ChaseRobberSel = new BTSelector("Chase Robber Selector");
            BTRepeatNodeFromIndexWithCondition ReturnBTToPatrolNode = new BTRepeatNodeFromIndexWithCondition("Return to patrol node", _tree, 1, true);

            BTLeaf ChaseRobberLeaf = new BTLeaf("Chase Robber", TryToCatchRobber);
            BTLeaf GoToPatrolPointLeaf = new BTLeaf("Go To Patrol Point", GoToPatrolPoint);
            BTLeaf SetPatrolPointsLeaf = new BTLeaf("Set Patrol Points", SetPatrolPoints);

            SetPatrolPointsInver.AddChildren(SetPatrolPointsLeaf);
            RepeatedPatrolSeq.AddChildren(GoToPatrolPointLeaf);

            MainPatrolSel.AddChildren(SetPatrolPointsInver);
            MainPatrolSel.AddChildren(RepeatedPatrolSeq);
            ChaseRobberSel.AddChildren(ChaseRobberLeaf);
            ChaseRobberSel.AddChildren(ReturnBTToPatrolNode);

            if (_tree.Children.Count <= 0)
            {
                _tree.AddChildren(MainPatrolSel);
                _tree.AddChildren(ChaseRobberSel);
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
            return ProcessStatusEnum.RUNNING;//for Testing
        }

        protected override void SawOtherAgent()
        {
            
        }
    }
}
