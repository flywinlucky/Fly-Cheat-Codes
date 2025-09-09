using UnityEngine;


/*
 * This script is an example, and by no means a basic script for the Fly Cheat Codes System.
 * It is adapted to say an object in a certain position. From this script is taken only the function that will run then when a combination will be found for each function.
 */

public class CheatManager : MonoBehaviour
{
    public Transform[] BoxesSpawnpoint; 
    public GameObject[] Boxes;

    public void Spawn_Red_Box() //What we put in this function will run when the function-related combination is found.
    {
        Debug.Log("The Red Box is spawned"); //Shows in the console which function is running (For each function it can be individual).
        Instantiate(Boxes[0], BoxesSpawnpoint[0].transform.position, Quaternion.identity); //The Red box is generated
    }

    public void Spawn_Yellow_Box() //What we put in this function will run when the function-related combination is found.
    {
        Debug.Log("The Yellow Box is spawned"); //Shows in the console which function is running (For each function it can be individual).
        Instantiate(Boxes[1], BoxesSpawnpoint[1].transform.position, Quaternion.identity); //The Yellow box is generated
    }

    public void Spawn_Blue_Box() //What we put in this function will run when the function-related combination is found.
    {
        Debug.Log("The Blue Box is spawned"); //Shows in the console which function is running (For each function it can be individual).
        Instantiate(Boxes[2], BoxesSpawnpoint[2].transform.position, Quaternion.identity); //The Blu box is generated
    }

    public void Spawn_Gray_Box() //What we put in this function will run when the function-related combination is found.
    {
        Debug.Log("The Gray Box is spawned"); //Shows in the console which function is running (For each function it can be individual).
        Instantiate(Boxes[3], BoxesSpawnpoint[3].transform.position, Quaternion.identity); //The Gray box is generated
    }

    //public void Spawn_Your_Function() //What we put in this function will run when the function-related combination is found.
    //{
        //Debug.Log("The Your_Function is spawned");
    //}
}

