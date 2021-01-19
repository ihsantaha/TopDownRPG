using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPosition : MonoBehaviour
{
    public int initialPosition;

    void Start()
    {
        if (StaticClass.PreviousScene == 3)
        {
            transform.position = StaticClass.CurrentPosition;
        }
        if (StaticClass.PreviousScene == 5)
            transform.position = new Vector3(4, -1, 0);  
    }
}
