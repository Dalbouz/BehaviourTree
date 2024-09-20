using Dawud.BT.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NPCRoot : MonoBehaviour
    {
        protected BehaviourTree _tree = default;
        protected Coroutine _behaveCoroutine = default;

        [SerializeField] protected bool _printOutTree = default;
        [HideInInspector] public NavMeshAgent Agent = default;
        [HideInInspector] public BehaviourTree Tree = default;
        [HideInInspector] public AgentStatusEnum AgentStatus = AgentStatusEnum.IDLE;
        [HideInInspector] public WaitForSeconds TreeCheckEvery = default;

        protected virtual void Start()
        {
            Agent = this.GetComponent<NavMeshAgent>();
            _tree = new BehaviourTree("Tree Behaviour: " + this.gameObject.name);

            TreeCheckEvery = new WaitForSeconds(Random.Range(0.1f, 1));

            CreateBehaviour();

            if (_printOutTree)
            {
                _tree.PrintTree();
            }

            _tree.Status = ProcessStatusEnum.AWAIT;
        }

        protected virtual void Update() { }

        protected virtual void CreateBehaviour() { }

        private IEnumerator BehaveCoroutine()
        {
            _tree.Status = _tree.Process();
            while (_tree.Status.Equals(ProcessStatusEnum.RUNNING))
            {
                _tree.Status = _tree.Process();
                yield return TreeCheckEvery;
            }

            _tree.SetAllNodesToDefaultValues();
            _behaveCoroutine = null;
        }

        protected void KillBehaveCoroutine()
        {
            if(_behaveCoroutine == null)
            {
                return;
            }
            StopCoroutine(_behaveCoroutine);
            _behaveCoroutine = null;
        }

        /// <summary>
        /// Starts the Behaviour coroutine of the NPC.
        /// </summary>
        public void StartBehave()
        {
            KillBehaveCoroutine();
            _behaveCoroutine = StartCoroutine(BehaveCoroutine());
        }

        public void PrintOutTree()
        {
            _tree.PrintTree();
        }
    }
}
