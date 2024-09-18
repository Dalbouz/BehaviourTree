using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    [SerializeField] private float _duration = default;
    [SerializeField] private GameObject _startingPosition = default;
    [SerializeField] private GameObject _topPosition = default; 

    [Button]
    public void StartMoveUpCoroutine()
    {
        StartCoroutine(MoveUpCoroutine());
    }

    private IEnumerator MoveUpCoroutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, _topPosition.transform.position.y, transform.position.z);

        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / _duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Waits for the next frame
        }

        // Ensure the object is at the exact target position at the end
        transform.position = targetPosition;
    }
}
