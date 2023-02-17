using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ProjectileReflectionEmitterUnityNative : MonoBehaviour {
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    LineRenderer line;
    void Start()
    {
        line = transform.GetComponent<LineRenderer>();
    }
    void OnDrawGizmos()
    {
//        Handles.color = Color.red;
//        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);

        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
    }
    void Update()
    {
//       
//        DrawPredictedReflectionPattern(line.transform.position + line.transform.forward * 0.75f, line.transform.forward, maxReflectionCount);
        if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
        {
            line.SetPosition(0,Movement.Instance.Player.transform.position);
        }
        else if (ControlFreak2.CF2Input.GetMouseButton(0))
        {
//            line.SetPosition(1,position);
        }
    }
     void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
//            line.SetPosition(0, Movement.Instance.Player.transform.position);
        }
        else
        {
            position += direction * maxStepDistance;

        }
//        line.SetPosition(1,position);
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(startingPosition, position);

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
}
