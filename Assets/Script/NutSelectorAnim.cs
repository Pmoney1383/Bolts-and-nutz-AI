using UnityEngine;
using System.Collections;

public class NutSelectorAnim : MonoBehaviour
{
    public Vector3 originalPosition;
    public bool isSelected = false;
    private Coroutine moveCoroutine;

    public float moveHeight = 0.5f;
    public float moveSpeed = 5f;

    void Start()
    {
        originalPosition = transform.localPosition; // Local relative to bolt
    }

    public void ToggleSelect()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        if (!isSelected)
        {
            // Move up
            Vector3 targetPos = originalPosition + new Vector3(0, moveHeight, 0);
            moveCoroutine = StartCoroutine(MoveToPosition(targetPos));
        }
        else
        {
            // Move back down
            moveCoroutine = StartCoroutine(MoveToPosition(originalPosition));
        }

        isSelected = !isSelected;
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * moveSpeed);
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
