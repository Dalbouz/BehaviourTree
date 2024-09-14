using Dawud.BT.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NPCMain : MonoBehaviour
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

            TreeCheckEvery = new WaitForSeconds(Random.Range(0.1f, 2));

            CreateBehaviour();

            if (_printOutTree)
            {
                _tree.PrintTree();
            }
        }

        protected virtual void Update()
        {
            if (_tree.Status.Equals(ProcessStatusEnum.AWAIT))
            {
                return;
            }
        }

        protected virtual void CreateBehaviour() { }

        protected IEnumerator BehaveCoroutine()
        {
            _tree.Process();
            while (_tree.Status.Equals(ProcessStatusEnum.RUNNING))
            {
                _tree.Process();
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

        public void StartBehave()
        {
            _behaveCoroutine = StartCoroutine(BehaveCoroutine());
        }
    }
}
