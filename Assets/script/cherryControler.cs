using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cherryControler : MonoBehaviour
{

    public Rigidbody2D rb;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = -4;
        rb.velocity = new Vector2(speed,0);
    }

}
