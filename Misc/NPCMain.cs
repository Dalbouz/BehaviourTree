using Dawud.BT.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NPCMain : MonoBehaviour
    {
        [SerializeField] protected bool _printOutTree = default;
        [HideInInspector] public NavMeshAgent Agent = default;
        [HideInInspector] public BehaviourTree Tree = default;
        [HideInInspector] public AgentStatusEnum AgentStatus = AgentStatusEnum.IDLE;
    }
}
