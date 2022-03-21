using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dialogs : MonoBehaviour
{
    [HideInInspector]
    public int str;
    public Inventory inventoryMain;
    public List<List<string>> dealerStrings;
    public List<List<string>> priestStrings;
    public GameObject dialog;
    public Dialogs dialogs;
    [HideInInspector]
    public string charName;
    [HideInInspector]
    public bool firstTime;
    public bool cabbageTrigger;
    // Start is called before the first frame update
    void Start()
    {
        str = -1;
        dialogs = GetComponent<Dialogs>();
        inventoryMain = GameObject.Find("Inventory").GetComponent<Inventory>();
        dealerStrings = new List<List<string>> {  
                                    new List<string>{ 
                                                        "Oh, hello there!",
                                                        "Want to buy some stuff?", 
                                                        "That's yours", 
                                                        "If you have enough...",
                                                        "Hmm...", 
                                                        "Money!", 
                                                        "Bye"
                                                    },

                                    new List<string>{ 
                                                        "Oh, hello there!",
                                                        "Nice to see you at this place, stranger",
                                                        "You was sent by Our King to help us, huh?",
                                                        "Then, listen here...",
                                                        "Not far from here there is a mountain with a cave.",
                                                        "You may have seen it.",
                                                        "This is the place where the Terrible Golem lives.",
                                                        "We call him 'Terrible' because once a week he comes...",
                                                        "He comes here and terrorizes us until we give three bags of food.",
                                                        "That's a hell of a lot for us!",
                                                        "After that, we have almost nothing to eat for the rest of the week...",
                                                        "...",
                                                        "We send a request to the King and, thanks God, you're finally here.",
                                                        "The Savior has come!!!",
                                                        "...",
                                                        "Ahem...",
                                                        "Well...",
                                                        "Yeah... I... I'll come to the point.",
                                                        "You have a sword or something, yeah?",
                                                        "No? Seriously?",
                                                        "Anyway, it's not a problem. You can buy one right here.",
                                                        "What? Don't you have money too?",
                                                        "What a curious thing!",
                                                        "Okay-okay, I can offer you a deal. If you bring three cabbages, I'm gonna buy them for 30 gold coins, okay?",
                                                        "And you can use it to buy... For example, this sword!",
                                                        "Deal?",
                                                        "Very good! That's the spirit!",
                                                        "So, come back when you'll have enough...",
                                                        "So, come back when you'll have enough... Hmm... ",
                                                        "So, come back when you'll have enough... Hmm... Cabbage!",
                                                        "So, come back when you'll have enough... Hmm... Cabbage! Ha-ha-ha-ha~",
                                                        "...Or money, huh.",
                                                        "See you, friend"
                                                    },
                                    new List<string>{
                                                        "You bring cabbages!",
                                                        "Great, I'll buy them.",
                                                        "And, by the way...",
                                                        "I'll give you an advice...",
                                                        "If you want to prepare for battle with Golem, you can go to a graveyard.",
                                                        "There are our neighbours - skeletons - live",
                                                        "We don't actually like them, so if you want to train your fight skills - visit that place.",
                                                        "And if you don't, you can go straight away to our Priest.",
                                                        "He can teach you some magic tricks or something.",
                                                        "Or at least he can bless you before the battle, he-he~",
                                                        "You have to go, right?",
                                                        "See you next time, a future hero. Goodluck."
                                                    }
                                        };

        priestStrings = new List<List<string>> {
                                new List<string>{
                                                    "Ooooh, greetings, traveler!",
                                                    "You've done a lot of work just to came at this place",
                                                    "But I must disappoint you, brave man...",
                                                    "I have been living here for almost 20 years to keep all unlucky travelers away from this fatal mountain",
                                                    "Why?",
                                                    "Cos a damn old golem lives at this mountain!",
                                                    "Хтьфу, curse on his ass",
                                                    "Oh...",
                                                    "You came here to kill him, don't you?",
                                                    "As I thought, the day will come when some daredevil will decide to do this",
                                                    "Anyway...",
                                                    "You can't kill him with your rusty toothpick, a delicate approach is needed here...",
                                                    "Or maybe not so delicate",
                                                    "I mean in your case, you can only kill him with magic",
                                                    "On a stump next to my house, there is just a scroll of a spell that would be effective",
                                                    "It is the Arcane Missiles that was damn useful stuff on Second Runic War",
                                                    "Try it, and goodluck, brave traveler!"
                                                }
                                        };
            
    }

    // Update is called once per frame
    void Update()
    {

    }
    public string Speech(string charName, int curStrNum)
    {
        if(charName == "Dealer")
        {
            try
            {
                if(inventoryMain.firstTime)
                {
                    return dealerStrings[1][curStrNum];
                }
                else if (inventoryMain.cabbageTrigger && !inventoryMain.firstTime)
                {
                    return dealerStrings[2][curStrNum];
                }
                else 
                {
                    return dealerStrings[0][curStrNum];
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                if (!inventoryMain.firstTime && inventoryMain.cabbageTrigger) inventoryMain.soldCabbage = true;
                if (inventoryMain.firstTime) inventoryMain.firstTime = false;
                dialog.SetActive(false);
                str = -1;
                return "";
            }
        }
        else if(charName == "Priest")
        {
            try
            {
                return priestStrings[0][curStrNum];
            }
            catch (System.ArgumentOutOfRangeException)
            {
                if (!inventoryMain.priest) inventoryMain.priest = true;
                dialog.SetActive(false);
                str = -1;
                return "";
            }
        }
        else
        {
            return "Something went wrong...";
        }
    }

    public void Dialog(string chName)
    {
        charName = chName;
        dialog.SetActive(true);
        dialog.transform.GetChild(0).transform.GetComponent<Text>().text = charName;
        dialog.transform.GetChild(2).transform.GetComponent<Image>().sprite = GameObject.Find(charName).transform.Find(charName+"Icon").transform.GetComponent<SpriteRenderer>().sprite;
        NewString();
    }
    public void NewString()
    {
        str++;
        dialog.transform.GetChild(1).transform.GetComponent<Text>().text = dialogs.Speech(charName, str);
    }
}
