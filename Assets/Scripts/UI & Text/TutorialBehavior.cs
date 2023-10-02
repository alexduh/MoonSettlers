using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TutorialBehavior : MonoBehaviour
{
    int currPage = 1;
    int totalPages;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private TMP_Text tutorialText;

    private Dictionary<int, string> tutorialDict;

    public void ShowTutorial()
    {
        gameObject.SetActive(true);
        UpdateTutorial();
    }

    public void HideTutorial()
    {
        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        currPage++;
        UpdateTutorial();
    }

    public void PrevPage()
    {
        currPage--;
        UpdateTutorial();
    }

    void UpdateTutorial()
    {
        if (currPage == 1)
            prevButton.interactable = false;
        else
            prevButton.interactable = true;

        if (currPage == totalPages)
            nextButton.interactable = false;
        else
            nextButton.interactable = true;

        tutorialText.text = tutorialDict[currPage];

    }

    // Start is called before the first frame update
    void Awake()
    {
        tutorialDict = new Dictionary<int, string> 
        {
            { 1, "You have been charged with a daunting task: to build a settlement and survive on the Moon! " +
            "Resources are limited, but your crew can build structures to generate their own resources. " +
            "Just remember, construction space  -  the most valuable resource of all, is highly limited. " +
            "After all, the moon is only about 2% of the size of Earth!\r\n"},
            { 2, "Fortunately, building and dismantling structures is easy! " +
            "First, select a structure to build from the left-hand menu, and left-click anywhere on the Moon to begin construction. " +
            "Then, using the 'Hammer' tool in the bottom-right, left-click any structure to remove it instantly! " +
            "The settlers can only construct one building at a time, but structures that are in the middle of construction can be dismantled too. "},
            { 3, "The more settlers there are, the faster the construction will be! " +
            "Don't worry about dismantling important buildings, as you can rebuild them later when needed."},
            { 4, "The upper-right hand corner shows most essential info.\n\n" +
            "Food and Water are constantly depleted over time based on population of the settlement, and may be consumed in other ways as well.\n\n" +
            "The oxygen bar on the right is similar to Food and Water, but there is a limit to the storable amount. " +
            "Use your building space efficiently to keep the basic necessities up!"},
            { 5, "Each building has different functions and may grant positive (and negative) effects, mouse-over each one for more detailed information. " +
            "If feeling pressed for time, press 'Space', 'P', or 'Escape' to pause and read at your leisure."},
            { 6, "More settlers will arrive every 3 days to help (and satisfy their curiosity). " +
            "Keep them alive for 30 days to complete the mission; if all the settlers are lost, it's a complete failure!"},
            };

        totalPages = tutorialDict.Count;
    }
}
