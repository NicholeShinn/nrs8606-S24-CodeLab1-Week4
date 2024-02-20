using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadNameInput : MonoBehaviour
{
    public static string inputText; //identifying name input

    //creating an on input (when you press return/enter) to be recorded
    //and then reflected into the game manager text line.
    public void ReadStringInput(string a)
    {
        inputText = a;
        Debug.Log("New Name :" + inputText);
        GameManager.instance.PlayerName = inputText;
    }
}
