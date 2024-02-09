using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you add this to an object with a collider the interaction system will work


//Notice the comma with the interface this will allow the player to see its interactable
//If you add the IInteractSystem to your script you wanna be able to interact with it wont let you run without Interact() func and the prompt Text.
public class ExampleInteractionScript : MonoBehaviour, IInteractSystem
{
    
    
    //This if the box in the editor with the label text if left empty then it wont display any prompt else it will display whats in the editor text box.
    [SerializeField] private string text = string.Empty;

    //This you dont really need to know about but it sends the text to the player and is nessassary.
    public string promptText => text;
    //Also not that line 16 isnt nessassary if you dont want it in the editor remove line 16 and replace text in here with string.Empty


    //This is function that will be called by the player and will run the code you write in here.
    public void Interact(InteractionSystem player)
    {
        Debug.Log("Do shit");
        //if you need to access infomation such as player position you can use player.transform and you should be able to get everything you need from it
    }
}
