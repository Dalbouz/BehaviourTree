using Dawud.BT.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NPCRoot : MonoBehaviour
    {
        protected bool _stopOnTriggerStay = false;
        protected BehaviorTree _tree = default;
        protected Coroutine _behaveCoroutine = default;
        protected float _seeDistance = default;

        [SerializeField] protected bool _printOutTree = default;
        [SerializeField] private float _viewAngle = 45f;

        public AgentTypeEnum ReactWhenSee = default;

        [HideInInspector] public NavMeshAgent Agent = default;
        [HideInInspector] public BehaviorTree Tree = default;
        [HideInInspector] public AgentStatusEnum AgentStatus = AgentStatusEnum.IDLE;
        [HideInInspector] public WaitForSeconds TreeCheckEvery = default;
        [HideInInspector] public List<NPCRoot> SeenAgents = default;

        protected virtual void Start()
        {
            _seeDistance = this.GetComponent<SphereCollider>().radius;
            Agent = this.GetComponent<NavMeshAgent>();
            _tree = new BehaviorTree("Tree Behavior: " + this.gameObject.name);

            TreeCheckEvery = new WaitForSeconds(Random.Range(0.1f, 1));

            CreateBehavior();

            if (_printOutTree)
            {
                _tree.PrintTree();
            }

            _tree.Status = ProcessStatusEnum.AWAIT;
        }

        protected virtual void Update() { }

        private void OnTriggerStay(Collider other)
        {
            // If the tag doesn't match or the agent has already been seen, return
            if (!other.CompareTag(ReactWhenSee.ToString()) || _stopOnTriggerStay)
            {
                return;
            }

            // Calculate the direction to the target
            Vector3 directionToTarget = other.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the other object is within the field of view and distance
            if (angleToTarget <= _viewAngle / 2 && directionToTarget.magnitude <= _seeDistance)
            {
                RaycastHit hitInfo;

                // Perform a raycast to ensure no obstacles are between the two objects
                if (Physics.Raycast(transform.position, directionToTarget.normalized, out hitInfo, _seeDistance))
                {
                    // Check if the ray hit the intended target
                    if (hitInfo.collider.CompareTag(ReactWhenSee.ToString()))
                    {
                        // Successfully detected the other agent
                        Debug.Log(gameObject.name + " saw agent: " + other.gameObject.tag);
                        if(SeenAgents.Count == 0 || SeenAgents == null)
                        {
                            SeenAgents = new List<NPCRoot>();
                        }
                        SeenAgents.Add(other.GetComponent<NPCRoot>());
                        SawOtherAgent();
                    }
                }
            }
        }

        // Draw gizmos to visualize the detection range and FOV in the Scene view
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            // Draw the line forward showing where the object is looking
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * _seeDistance);

            Gizmos.color = Color.red;

            // Draw the left and right edges of the FOV
            Vector3 leftLimit = Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward;
            Vector3 rightLimit = Quaternion.Euler(0, _viewAngle / 2, 0) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position + leftLimit * _seeDistance);
            Gizmos.DrawLine(transform.position, transform.position + rightLimit * _seeDistance);

            // Optionally, draw a sphere to indicate detection range
            Gizmos.color = new Color(0, 0, 1, 0.3f); // Semi-transparent blue
            Gizmos.DrawWireSphere(transform.position, _seeDistance);
        }

        protected virtual void CreateBehavior() { }

        protected virtual void SawOtherAgent() { }

        /// <summary>
        /// Simulate update to run the BehaviorTree.
        /// </summary>
        /// <returns></returns>
        private IEnumerator BehaveCoroutine()
        {
            _tree.Status = _tree.Process();
            while (_tree.Status.Equals(ProcessStatusEnum.RUNNING))
            {
                _tree.Status = _tree.Process();
                yield return TreeCheckEvery;
            }

            _tree.ResetTree();
            _behaveCoroutine = null;
        }

        /// <summary>
        /// Stops the Behave coroutine/Stops the BehaviorTree.
        /// </summary>
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
        /// Starts the Behavior coroutine of the NPC.
        /// </summary>
        public void StartBehave()
        {
            KillBehaveCoroutine();
            _behaveCoroutine = StartCoroutine(BehaveCoroutine());
        }

        /// <summary>
        /// Prints the <see cref="BehaviorTree"/> in the console.
        /// </summary>
        public void PrintOutTree()
        {
            _tree.PrintTree();
        }
    }
}
