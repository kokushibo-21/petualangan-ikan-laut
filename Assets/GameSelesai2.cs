using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelesai2 : MonoBehaviour
{
    public Text Teks_Score;


    public void Start()
    {
        if(SistemGame.DataScore >= PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", SistemGame.DataScore);
        }


        Teks_Score.text = SistemGame.DataScore.ToString();
    }
}