using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float outerLeft = -10f;
    public float outerRight = 10f;

    public float MIN_X, MAX_X, MIN_Y, MAX_Y, MIN_Z, MAX_Z;

    Vector3 ResetCameraPosition;
    float ResetOrthographicSize;

    Vector3 Origin;
    Vector3 Diference;
    bool Drag = false;

    void Start()
    {
        ResetCameraPosition = Camera.main.transform.position;
        ResetOrthographicSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        ZoomCamera();
    }

    void LateUpdate()
    {
        DragMouse();
    }

    void DragMouse()
    { 
        if (Input.GetMouseButton(0))
        {
            Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            Drag = false;
        }
        if (Drag == true)
        { 
            Camera.main.transform.position = Origin - Diference;
        }

        //возврат камеры на стартовую позицию (измененна кнопка возврата)
        if (Input.GetMouseButton(2))
        {
            Camera.main.transform.position = ResetCameraPosition;
            Camera.main.orthographicSize = ResetOrthographicSize;
        }

        CheckLimitCoordinates(); 
    }

    void CheckLimitCoordinates()
    {
        if (Camera.main.orthographicSize <= 7.3f)
        {
            MIN_X = 85f;  MAX_X = 105f;
            MIN_Y = 37f; MAX_Y = 42f;
            MIN_Z = 85f; MAX_Z = 105f;
        } 

        transform.position = new Vector3(
           Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
           Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
           Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
    }

    void ZoomCamera()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 2.8f)
        {
            Camera.main.orthographicSize -= .3f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < 7.3f)
        {
            Camera.main.orthographicSize += .3f;
        }
    }
}

