using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    Vector3 init = new Vector3(0,0,0);

    public Rigidbody myRigidbody;

    public Node right = null, left = null;

    public float maxDistanceBetween = 5.0f;
    public float forceMultiplier = 2.0f;
    public float minForceMagnitude = 10.0f;
    public float maxVel = 20.0f;
    public float vel = 0.0f;
    Vector3 velVector = new Vector3(0f, 0f, 0f);

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool setRight = false;
        bool setLeft = false;
        if (vel > 0)
        {
            transform.position += velVector * Time.deltaTime;
        }
        if (left != null)
        {
            Vector3 dist = left.gameObject.transform.position- transform.position;
            if (dist.magnitude != 0 && dist.magnitude > maxDistanceBetween)
            {
                Vector3 newLocation = dist.normalized * maxDistanceBetween;
                left.gameObject.transform.position = transform.position + newLocation;
                setLeft = true;
            }
        }
        if (right != null)
        {
            Vector3 dist = transform.position - right.gameObject.transform.position;
            if (dist.magnitude != 0 && dist.magnitude > maxDistanceBetween)
            {
                Vector3 newLocation = dist.normalized * maxDistanceBetween;
                transform.position = right.gameObject.transform.position + newLocation;
                setRight = true;
            }
        }
        if (setLeft && setRight)
        {
            vel = 0;
        }
    }

    void OnMouseDown()
    {
        init = Input.mousePosition;
        Debug.Log(init);
    }

    void OnMouseUp()
    {        
        Vector3 end = Input.mousePosition;
        end = end - init;
        Vector3 force = new Vector3(end.x, 0, end.y) * forceMultiplier;
        if (force.magnitude > minForceMagnitude)
        {
            velVector = Mathf.Min(maxVel, force.magnitude)*force.normalized;
            vel = velVector.magnitude;
            //myRigidbody.AddForce(force);

            addForceToBrothersLeft(this, force);
            addForceToBrothersRight(this, force);
        }
        else
        {
            Debug.Log("Stop Game Object");
            vel = 0.0f;
        }
    }

    void addForceToBrothersLeft(Node node, Vector3 force)
    {
        Vector3 newForce = force / 2;

        if (node.left != null)
        {
            Vector3 dist = node.gameObject.transform.position - node.left.gameObject.transform.position;
            if (dist.magnitude != 0 && dist.magnitude > maxDistanceBetween)
            {
                node.left.myRigidbody.AddForce(dist.normalized * newForce.magnitude);
                addForceToBrothersLeft(node.left, newForce);
            }
        }
    }

    void addForceToBrothersRight(Node node, Vector3 force)
    {
        Vector3 newForce = force / 2;
        if (node.right != null)
        {
            Vector3 dist = node.gameObject.transform.position - node.right.gameObject.transform.position;
            if (dist.magnitude != 0 && dist.magnitude > maxDistanceBetween)
            {
                node.right.myRigidbody.AddForce(dist.normalized * newForce.magnitude);
                addForceToBrothersRight(node.right, newForce);
            }
        }
    }
}
