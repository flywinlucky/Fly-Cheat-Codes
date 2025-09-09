using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//----Fly Cheat Codes System v1.0----
//Suport - flystudiosassets@gmail.com

public class CheatCode : MonoBehaviour
{
    [Space]
    [Header("Notification Seting")]
    [Tooltip("Add notification")]
    public GameObject CheatNotification; //This notification informs when a cheat code will be activated. If you do not want this feature, do not add the notification to the box.

    [Space]
    [Tooltip("Add text for cheat codes debug")]
    public Text DebugCheatText; //This text component helps you see which button is currently pressed. If you do not want this function do not add the text component in the box.
    private string currentString = "";
    private float ClearDataAfter;
    bool Timer = false;

    [Space]
    [SerializeField]
    private List<CheatCodeInstance> cheatCodeList = new List<CheatCodeInstance>();

    private void Reset_Data()
    {
        ClearDataAfter =+ 1;
    }

    private void Update() //Update is called every frame, if the MonoBehaviour is enabled.
    {
        if (DebugCheatText != null) //Check if the Text box is full or not.
        {
            DebugCheatText.text = currentString.ToUpper();
        }
        
        CheckCheat(currentString);

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) //Check if there is any coincidence with a generated cheat code.
        {
            if (Input.GetKeyDown(vKey))
            {
                currentString += vKey;
                Reset_Data();
            }
        }

        if (currentString == "")
        {
            Timer = false;
        }
        else
        {
            Timer = true;
        }

        if (Timer)
        {
            if (ClearDataAfter > 0)
            {
                ClearDataAfter -= Time.deltaTime;
            }
            else
            {
                Reset_Data();
                Timer = false;
                currentString = "";
            }
        }
    }

    [System.Serializable]
    public class CheatCodeInstance
    {
        public string code;
        public UnityEvent cheatEvent;
    }

    private bool CheckCheat(string _input)
    {
        foreach (CheatCodeInstance code in cheatCodeList)
        {
            if (_input == code.code) //Checks if a cheat code combination has been found.
            {
                if (code.code != "") 
                {
                    if (CheatNotification != null) //Check that the Notification box is full or not.
                    {
                        StartCoroutine(NotifiGame()); //The function is called when a cheat code is activated.

                        IEnumerator NotifiGame()
                        {
                            yield return new WaitForSeconds(0);
                            CheatNotification.SetActive(true);
                            yield return new WaitForSeconds(2);
                            CheatNotification.SetActive(false);
                        }
                    }

                    code.cheatEvent?.Invoke(); //The function assigned to the cheat code is run
                    Reset_Data();
                    currentString = "";
                    return true;
                }
            }
        }

        return false;
    }
}
