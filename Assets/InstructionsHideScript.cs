using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsHideScript : MonoBehaviour
{
    [SerializeField] Text MovementText;
    [SerializeField] Text WeaponsText;
    [SerializeField] Text BarnacleText;
    float ShowTime = 7.5f;
    float FadeTime = 7.5f;

    float BarnacleTextShowTime = 0f;
    float BarnacleTextFadeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(ShowTime >= 0)
        {
            ShowTime -= Time.deltaTime;
        } else if(FadeTime >= 0)
        {
            FadeTime -= Time.deltaTime;
            Color c = MovementText.color;
            c.a = 1.0f * FadeTime / 7.5f;
            MovementText.color = c;
            WeaponsText.color = c;
        } else if(MovementText && WeaponsText) {
            Destroy(MovementText);
            Destroy(WeaponsText);
        }

        if (BarnacleTextShowTime >= 0)
        {
            BarnacleTextShowTime -= Time.deltaTime;
        }
        else if (BarnacleTextFadeTime >= 0)
        {
            BarnacleTextFadeTime -= Time.deltaTime;
            Color c = BarnacleText.color;
            c.a = 1.0f * BarnacleTextFadeTime / 7.5f;
            BarnacleText.color = c;
        }
    }

    public void ShowBarnacleInfo()
    {
        Debug.Log("barnacle info");
        Color c = BarnacleText.color;
        c.a = 1.0f;
        BarnacleText.color = c;
        BarnacleTextShowTime = 7.5f;
        BarnacleTextFadeTime = 7.5f;
    }
}
