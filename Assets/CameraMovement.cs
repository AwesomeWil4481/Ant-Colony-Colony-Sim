using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int speed = 10;

    void Update()
    {
        var rb = gameObject.GetComponent<Rigidbody>();

        { //Vertical camera movement
            { //Upwards camera movement
                if (Input.GetKeyDown(KeyCode.W))
                {
                    rb.velocity = new Vector3(rb.velocity.x, speed, 0);
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                }
            }

            { //Downwards camera movement
                if (Input.GetKeyDown(KeyCode.S))
                {
                    rb.velocity = new Vector3(rb.velocity.x, -speed, 0);
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                }
            }
        }

        { //Horizontal camera movement
            { //Left camera movement
                if (Input.GetKeyDown(KeyCode.A))
                {
                    rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }

            { //Right camera movement
                if (Input.GetKeyDown(KeyCode.D))
                {
                    rb.velocity = new Vector3(speed, rb.velocity.y, 0);
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }
        }

        //    // mouse left
        //    if (Input.mousePosition.x <= 200)
        //    {
        //        rb.velocity = new Vector3(-10, 0, rb.velocity.z);
        //    }

        //    // mouse right
        //    else if (Input.mousePosition.x >= 1720)
        //    {
        //        rb.velocity = new Vector3(10, 0, rb.velocity.z);
        //    }
        //    //stops movement after leaving the sides
        //    else if (Input.mousePosition.x >= 200 && Input.mousePosition.x <= 1720)
        //    {
        //        rb.velocity = new Vector3(0, 0, rb.velocity.z);
        //    }

        //    // mouse down
        //    if (Input.mousePosition.y <= 120)
        //    {
        //        rb.velocity = new Vector3(rb.velocity.x, 0, -8);
        //    }
        //    // mouse up
        //    else if (Input.mousePosition.y >= 960)
        //    {
        //        rb.velocity = new Vector3(rb.velocity.x, 0, 8);
        //    }
        //    //stops movement after leaving the verticals
        //    else if (Input.mousePosition.y >= 120 && Input.mousePosition.y <= 960)
        //    {
        //        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        //    }
        //}
    }
    IEnumerator FindMousePosition()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (Input.mousePosition.x <= 300)
        {
            print("Move Left");
        }
        if (Input.mousePosition.x >= 1620)
        {
            print("Move Right");
        }

        if (Input.mousePosition.y <= 200)
        {
            print("Move Down");
        }
        if (Input.mousePosition.y >= 880)
        {
            print("Move Up");
        }

        StartCoroutine(FindMousePosition());
    }
}
