using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsHideScript : MonoBehaviour
{
    [SerializeField] Text MovementText;
    [SerializeField] Text WeaponsText;
    float ShowTime = 7.5f;
    float FadeTime = 7.5f;

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
            c.a = 1.0f * FadeTime / 5.0f;
            MovementText.color = c;
            WeaponsText.color = c;
        } else if(MovementText && WeaponsText) {
            Destroy(MovementText);
            Destroy(WeaponsText);
        }
    }
}
