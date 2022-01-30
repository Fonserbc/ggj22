using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementText : MonoBehaviour
{
    public TMPro.TMP_Text text;

    public static AchievementText instance;

    private void Awake()
    {
        instance = this;
    }

    public void ReportOutOfTheWindow(string what)
    {
        text.text = string.Format("You kicked {0} out of the window", what);
        text.CrossFadeAlpha(1f, 0f, false);
        text.CrossFadeAlpha(0, 4f, false);
    }

    public void ReportEaten(string what)
    {
        text.text = string.Format("You ate {0}", what);
        text.CrossFadeAlpha(1f, 0f, false);
        text.CrossFadeAlpha(0, 4f, false);
    }

    public void ReportAchievement(int which) {
        if (which != -1)
        {
            text.text = string.Format("You took a virtual walk");
            text.CrossFadeAlpha(1f, 0f, false);
            text.CrossFadeAlpha(0, 5f, false);
        }
    }
}
