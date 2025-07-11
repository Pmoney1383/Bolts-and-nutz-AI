using System.Collections;
using UnityEngine;

public class NutMoverAnim : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float liftHeight = 20f;
    public float nutHeight = 2.62f;
    public int nutIndex; // how many children == stack index

    public IEnumerator MoveToBolt(GameObject nut, GameObject targetBolt)
    {
        nutIndex = 0;
        Vector3 start = nut.transform.position;

        // PHASE 1: Lift to bolt Y + 10
        Vector3 liftPos = new Vector3(start.x, targetBolt.transform.position.y + liftHeight, start.z);
        yield return StartCoroutine(MoveToPosition(nut, liftPos));

        // PHASE 2: Move horizontally (X/Z)
        Vector3 horizontalTarget = new Vector3(
            targetBolt.transform.position.x,
            liftPos.y,
            targetBolt.transform.position.z
        );
        yield return StartCoroutine(MoveToPosition(nut, horizontalTarget));

        // PHASE 3: Drop down to correct Y position
        
        foreach (Transform child in targetBolt.transform)
        {
            if (child.name.Contains("Nut"))
            {
                nutIndex++;
            }
        }
        Vector3 finalPos = targetBolt.transform.position + new Vector3(0, 4.17f + (nutHeight * nutIndex), 0);
        yield return StartCoroutine(MoveToPosition(nut, finalPos));

        // Final snap
        nut.transform.position = finalPos;
        nut.transform.SetParent(targetBolt.transform);
        nut.GetComponent<NutSelectorAnim>().originalPosition = this.transform.localPosition;
    }

    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPos)
    {
        while (Vector3.Distance(obj.transform.position, targetPos) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPos, Time.deltaTime * moveSpeed);
            yield return null;
        }
        obj.transform.position = targetPos;
    }
}
