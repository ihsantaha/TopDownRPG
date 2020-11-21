using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLoop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x > 20.4)
        {
            this.transform.position = new Vector2(-12.5f, this.transform.position.y);
        }
        Vector2 xPosition = this.transform.position;
        xPosition.x += 0.001f;
        this.transform.position = xPosition;
    }
}
