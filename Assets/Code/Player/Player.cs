using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    public float hp = 100;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void LowerHp(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            Death();
        }
    }
    void Death()
    {

    }


}
