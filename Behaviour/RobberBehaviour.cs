using UnityEngine;
using Dawud.BT.General;
using UnityEngine.AI;
using Dawud.BT.Enums;
using Dawud.BT.Actions;
using System;

namespace Dawud.BT.Behaviour
{
    /// <summary>
    /// 
    /// </summary>
    public class RobberBehaviour : NPCMain
    {
        [SerializeField] private GameObject _diamond = default;
        [SerializeField] private GameObject _van = default;
        [SerializeField] private GameObject _backDoor = default;
        [SerializeField] private GameObject _frontDoor = default;
        [SerializeField] BehaviourTree _tree = default;

        void Start()
        {
            Agent = this.GetComponent<NavMeshAgent>();

            _tree = new BehaviourTree("Robber Tree Behaviour");
            Sequence steal = new Sequence("Steal something");

            Leaf goToBackDoor = new Leaf("Go To back door", GoToBackDoor);
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            steal.AddChildren(goToBackDoor);
            steal.AddChildren(goToDiamond);
            steal.AddChildren(goToBackDoor);
            steal.AddChildren(goToVan);
            _tree.AddChildren(steal);

            if (_printOutTree)
            {
                _tree.PrintTree();
            }
            _tree.Status = ProcessStatusEnum.RUNNING;
        }

        private void Update()
        {
            if (_tree.Status.Equals(ProcessStatusEnum.RUNNING))
            {
                _tree.Status = _tree.Process();
            }
            if(_tree.Status.Equals(ProcessStatusEnum.SUCCESS) || _tree.Status.Equals(ProcessStatusEnum.FAILED))
            {
                _tree.SetAllToDefaultValues();
            }
        }

        private ProcessStatusEnum GoToDiamond()
        {
            
            return GenericActions.GoToDestination(_diamond.transform.position, this.gameObject, this);
        }

        private ProcessStatusEnum GoToVan()
        {
            return GenericActions.GoToDestination(_van.transform.position, this.gameObject, this);
        }

        private ProcessStatusEnum GoToBackDoor()
        {
            return GenericActions.GoToDestination(_backDoor.transform.position, this.gameObject, this);
        }

        private ProcessStatusEnum GoToFrontDoor()
        {
            return GenericActions.GoToDestination(_frontDoor.transform.position, this.gameObject, this);
        }
    }
}
