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
    public float maxVel = 10.0f;
    public float vel = 0.0f;
    private bool addingForce;
    private float timerCollision, timerMaxCollision;
    Vector3 velVector = new Vector3(0f, 0f, 0f);

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        addingForce = false;
        timerMaxCollision = 0.3f;
        timerCollision = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool setRight = false;
        bool setLeft = false;
        if ((vel > 0) && (addingForce))
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
        if (timerCollision > 0)
            timerCollision -= Time.deltaTime;
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
            //Debug.Log("FORCE MAGNITUDE:" + force.magnitude);
            velVector = Mathf.Min(maxVel, force.magnitude)*force.normalized;
            vel = velVector.magnitude;
            //Debug.Log("VEL=" + vel);
            //myRigidbody.AddForce(force);
            addingForce = true;
            StartCoroutine(StopForce(force.magnitude/100)); //tiempo en parar
            if (this.left != null)
            {
                this.left.EmpujarCadena(force / 2, 0);
            }
            else if (this.right != null)
            {
                this.right.EmpujarCadena(force / 2, 1);
            }
            //addForceToBrothersLeft(this, force);
            //addForceToBrothersRight(this, force);
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

    IEnumerator StopForce(float time)
    {
        yield return new WaitForSeconds(time);
        addingForce = false;
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if ((collision.gameObject.tag == "Preso") && (timerCollision <= 0))
        {
            if (collision.gameObject.GetComponent<Preso>().enabled)
            {
                Debug.Log("Colisión con " + collision.gameObject.name);
                EncadenarPreso(collision.gameObject);
                timerCollision = timerMaxCollision;
            }            
        }
    }

    public void EncadenarPreso(GameObject preso)
    {        
        if (right == null)
        {
            Debug.Log("EncadenarPreso a " + this.name + " right null");
            Debug.Log(transform.name+" Right NULL --> Nos ponemos a la derecha");
            preso.GetComponent<Preso>().enabled = false;
            preso.GetComponent<Node>().enabled = true;
            right = preso.GetComponent<Node>();
            preso.GetComponent<Node>().left = this.GetComponent<Node>();
            preso.transform.position = transform.Find("Right").position;
            preso.transform.parent = transform.parent;            
        }
        else
        {
            //llamar a encadenarPreso del right de este nodo
            Debug.Log("->EncadenarPreso a " + this.right.name);
            right.EncadenarPreso(preso);
        }        
    }

    public void EmpujarCadena(Vector3 force, int direction)
    {
        velVector = Mathf.Min(maxVel, force.magnitude) * force.normalized;
        vel = velVector.magnitude;
        addingForce = true;
        StartCoroutine(StopForce(force.magnitude / 100));
        if ((direction == 0) && (this.left != null))
        {
            this.left.EmpujarCadena(force / 2, 0);
        }
        else if ((direction==1) && (this.right!=null))
        {
            this.right.EmpujarCadena(force / 2, 1);
        }
    }
}
