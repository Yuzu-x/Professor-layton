using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject cam;
    public GameObject camHolder;
    public float rotation;
    public float movement;
    public float horizontal;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
        #region Rotate
        //Left
        if (Input.GetKey(KeyCode.Q))
        {
            
            camHolder.transform.Rotate(-Vector3.up * rotation * Time.deltaTime);
        }
        //Right
        if (Input.GetKey(KeyCode.E))
        {
            
            camHolder.transform.Rotate(Vector3.up * rotation * Time.deltaTime);
        }
        #endregion
        #region Move
        //Up
        if (Input.GetKey(KeyCode.W))
        {
            camHolder.transform.Translate(Vector3.forward * movement * Time.deltaTime);
        }
        //Left
        if (Input.GetKey(KeyCode.A))
        {
            camHolder.transform.Translate(Vector3.left * movement * Time.deltaTime);
        }
        //Down
        if (Input.GetKey(KeyCode.S))
        {
            camHolder.transform.Translate(-Vector3.forward * movement * Time.deltaTime);
        }
        //Right
        if (Input.GetKey(KeyCode.D))
        {
            camHolder.transform.Translate(Vector3.right * movement * Time.deltaTime);
        }
        #endregion
        #region Zoom
        //Forward
        if (Input.GetKey(KeyCode.Z))
        {
            cam.transform.Translate(Vector3.forward * movement * Time.deltaTime, Camera.main.transform);
        }
        //Backward
        if (Input.GetKey(KeyCode.X))
        {
            cam.transform.Translate(Vector3.back * movement * Time.deltaTime, Camera.main.transform);
        }
        #endregion
    }
}
