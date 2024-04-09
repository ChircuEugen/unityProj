using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManagerScript : MonoBehaviour
{
    public TMP_Text bubbleStatus;
    public TMP_Text bubbleFeedback;
    public TMP_Text bubbleTime;

    public TMP_Text selectionStatus;
    public TMP_Text selectionFeedback;
    public TMP_Text selectionTime;

    public TMP_Text kruskalStatus;
    public TMP_Text kruskalFeedback;
    public TMP_Text kruskalTime;

    public TMP_Text primStatus;
    public TMP_Text primFeedback;
    public TMP_Text primTime;




    private void Start()
    {
        // FOR BUBBLE SORT
        //PlayerPrefs.DeleteAll();
            bubbleStatus.text = PlayerPrefs.GetString("bubbleStatus");
            if(bubbleStatus.text == "")
            {
                bubbleStatus.text = "N/A";
                bubbleFeedback.text = "N/A";
                bubbleTime.text = "N/A";
            }
            else if (bubbleStatus.text == "Fail")
            {
                bubbleStatus.color = Color.red;
                bubbleFeedback.color = Color.red;
            bubbleFeedback.text = PlayerPrefs.GetString("bubbleFeedback");
            bubbleFeedback.fontSize = 27;

            bubbleTime.text = PlayerPrefs.GetString("bubbleTime");
            bubbleTime.fontSize = 27;
        }
            else
            {
                bubbleStatus.color = Color.green;
                bubbleFeedback.color = Color.green;
            bubbleFeedback.text = PlayerPrefs.GetString("bubbleFeedback");
            bubbleFeedback.fontSize = 27;

            bubbleTime.text = PlayerPrefs.GetString("bubbleTime");
            bubbleTime.fontSize = 27;
        }

            
            // FOR BUBBLE SORT


            // FOR SELECTION SORT
            selectionStatus.text = PlayerPrefs.GetString("selectionStatus");
            if (selectionStatus.text == "")
            {
                selectionStatus.text = "N/A";
                selectionFeedback.text = "N/A";
                selectionTime.text = "N/A";
            }
            else if (selectionStatus.text == "Fail")
            {
                selectionStatus.color = Color.red;
                selectionFeedback.color = Color.red;
            selectionFeedback.text = PlayerPrefs.GetString("selectionFeedback");
            selectionFeedback.fontSize = 27;

            selectionTime.text = PlayerPrefs.GetString("selectionTime");
            selectionTime.fontSize = 27;
        }
            else
            {
                selectionStatus.color = Color.green;
                selectionFeedback.color = Color.green;
            selectionFeedback.text = PlayerPrefs.GetString("selectionFeedback");
            selectionFeedback.fontSize = 27;

            selectionTime.text = PlayerPrefs.GetString("selectionTime");
            selectionTime.fontSize = 27;
        }

            
            // FOR SELECTION SORT


            // FOR KRUSKAL MST
            kruskalStatus.text = PlayerPrefs.GetString("kruskalStatus");
            if (kruskalStatus.text == "")
            {
                kruskalStatus.text = "N/A";
                kruskalFeedback.text = "N/A";
                kruskalTime.text = "N/A";
            }
            else if (kruskalStatus.text == "Fail")
            {
                kruskalStatus.color = Color.red;
                kruskalFeedback.color = Color.red;
            kruskalFeedback.text = PlayerPrefs.GetString("kruskalFeedback");
            kruskalFeedback.fontSize = 27;

            kruskalTime.text = PlayerPrefs.GetString("kruskalTime");
            kruskalTime.fontSize = 27;
        }
            else
            {
                kruskalStatus.color = Color.green;
                kruskalFeedback.color = Color.green;
            kruskalFeedback.text = PlayerPrefs.GetString("kruskalFeedback");
            kruskalFeedback.fontSize = 27;

            kruskalTime.text = PlayerPrefs.GetString("kruskalTime");
            kruskalTime.fontSize = 27;
        }

            
            // FOR KRUSKAL MST


            // FOR PRIM MST
            primStatus.text = PlayerPrefs.GetString("primStatus");
            if (primStatus.text == "")
            {
                primStatus.text = "N/A";
                primFeedback.text = "N/A";
                primTime.text = "N/A";
            }
            else if (primStatus.text == "Fail")
            {
                primStatus.color = Color.red;
                primFeedback.color = Color.red;
            primFeedback.text = PlayerPrefs.GetString("primFeedback");
            primFeedback.fontSize = 27;

            primTime.text = PlayerPrefs.GetString("primTime");
            primTime.fontSize = 27;
        }
            else
            {
                primStatus.color = Color.green;
                primFeedback.color = Color.green;
            primFeedback.text = PlayerPrefs.GetString("primFeedback");
            primFeedback.fontSize = 27;

            primTime.text = PlayerPrefs.GetString("primTime");
            primTime.fontSize = 27;
        }

            
            // FOR PRIM MST
        
    }

    bool CheckEmpty()
    {
        bool allEmpty = false;

        if (bubbleStatus.text == "")
        {
            bubbleStatus.text = "N/A";
            bubbleFeedback.text = "N/A";
            bubbleTime.text = "N/A";
            allEmpty = true;
        }
        else allEmpty = false;

        if (selectionStatus.text == "")
        {
            selectionStatus.text = "N/A";
            selectionFeedback.text = "N/A";
            selectionTime.text = "N/A";
            allEmpty = true;
        }
        else allEmpty = false;

        if (kruskalStatus.text == "")
        {
            kruskalStatus.text = "N/A";
            kruskalFeedback.text = "N/A";
            kruskalTime.text = "N/A";
            allEmpty = true;
        }
        else allEmpty = false;

        if (primStatus.text == "")
        {
            primStatus.text = "N/A";
            primFeedback.text = "N/A";
            primTime.text = "N/A";
            allEmpty = true;
        }
        else allEmpty = false;

        Debug.Log(allEmpty);
        return allEmpty;
    }

}
