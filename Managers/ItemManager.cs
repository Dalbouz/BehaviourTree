using Dawud.BT.Actions;
using Dawud.BT.General;
using Dawud.BT.Misc;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ItemManager : SingletonRoot<ItemManager>
{
    [SerializeField] private List<ItemGeneric> _pickupableItems = new List<ItemGeneric>();
    [SerializeField] private List<GameObject> _patrolPoints = new List<GameObject>();
    private int _currentAvailablePatrolPointIndex = 0;

    public List<ItemGeneric> PickupableItems
    {
        get { return _pickupableItems; }
    }

    public List<GameObject> PatrolPoints
    {
        get { return _patrolPoints; }
    }

    private void Start()
    {
        ResetPatrolPoints();
    }

    public List<GameObject> GetAvailablePatrolPoints(int numbOfReqPatrolPoints)
    {
        if(_currentAvailablePatrolPointIndex >= _patrolPoints.Count)
        {
            return null;
        }

        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < numbOfReqPatrolPoints; i++)
        {
            list.Add(_patrolPoints[_currentAvailablePatrolPointIndex]);
            _currentAvailablePatrolPointIndex++;
            if(_currentAvailablePatrolPointIndex >= _patrolPoints.Count)
            {
                break;
            }
        }

        return list;
    }

    public void ResetPatrolPoints()
    {
        _patrolPoints =GenericActions.RandomShuffleGameObjectList(_patrolPoints);
        _currentAvailablePatrolPointIndex = 0;
    }
}
