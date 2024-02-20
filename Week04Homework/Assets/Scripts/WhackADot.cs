using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhackADot : MonoBehaviour
{
    //When you click down on the collider of the object this script is attached to, the object transforms to a new location
    void OnMouseDown()
    {
        Debug.Log("You whacked a Dot!!!");

        //new vector 2 rather than 3 to limit the range to 2D space, or X & Y only
        transform.position = new Vector2(
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f)
            );

        //Score referenced in game manager script goes up by 1 each success
        GameManager.instance.Score++;
    }
}
