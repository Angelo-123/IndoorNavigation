using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAndPitch : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;

    public int triggered;
    public Vector3 StartVec;
    float startRotation;

    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;
    }

    private void Start()
    {
        Input.compass.enabled = true;
        triggered = 0;
    }

    private void Update()
    {
        if (triggered < 3)
        {
            StartVec = GlobalValues.pathScript.StartVec;
            startRotation = Input.compass.magneticHeading;
            Camera.transform.position = new Vector3(StartVec.x, 200, StartVec.z);
            Camera.transform.rotation = Quaternion.Euler(70, startRotation, 0);
            triggered += 1;
        }


        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount >= 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                Camera.transform.Translate(Delta1, Space.World);
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            
            //edge case
            if (zoom == 0 || zoom > 10)
                return;
            if ((Camera.transform.position.y < 420) && (Camera.transform.position.y > 40))
            {
                //Move cam amount the mid ray
                Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);                
            }
            else
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, 200, Camera.transform.position.z);
            }

            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }

    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}
