using KrampStudio.BT.Actions;
using KrampStudio.BT.General;
using KrampStudio.BT.Misc;
using System;
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

    private int _scoreTest = 0;

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

    /// <summary>
    /// Returns a list of with patrol points. The caller can request any number of patrol points, its going to give back as many patrol points as it can.
    /// </summary>
    /// <param name="numbOfReqPatrolPoints"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Shuffles the Patrol points inside the list into a random order. Sets the <see cref="_currentAvailablePatrolPointIndex"/> to 0.
    /// </summary>
    public void ResetPatrolPoints()
    {
        _patrolPoints = GenericActions.RandomShuffleGameObjectList(_patrolPoints);
        _currentAvailablePatrolPointIndex = 0;
    }
}
