using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingScript : MonoBehaviour,IInteractSystem
{
    private DayNightCycle DNscript;

    private void Start()
    {
        DNscript = FindAnyObjectByType<DayNightCycle>();
    }

    public string text
    {
        get
        {
            if(DNscript.Hours < 6 || DNscript.Hours >18)
            {
                return "Press F to Sleep.";
            }
            return "Sorry you cant sleep right now.";
        }
    }
    public string promptText => text;

    public void Interact(InteractionSystem player)
    {
        if(DNscript.Hours < 6 || DNscript.Hours > 18)
        {
            DNscript.Hours = 6;
            DNscript.Minutes = 0;
        }
    }
}
