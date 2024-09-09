using UnityEngine;
using Dawud.BT.General;
using UnityEngine.AI;
using Dawud.BT.Enums;
using Dawud.BT.Actions;

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
        [SerializeField] BehaviourTree _tree = default;

        void Start()
        {
            Agent = this.GetComponent<NavMeshAgent>();

            _tree = new BehaviourTree("Robber Tree Behaviour");
            Sequence steal = new Sequence("Steal something");

            Leaf goToBackDoor = new Leaf("Go To back door", GoToBackDoor);
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
        }

        private void Update()
        {
            if (_treeStatus.Equals(ProcessStatusEnum.RUNNING))
            {
                _treeStatus = _tree.Process();
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
    }
}
