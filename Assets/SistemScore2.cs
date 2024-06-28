using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemScore2 : MonoBehaviour
{
    public int DataTotalScore;

    public Image GambarInfoAtas;
    public Sprite[] GambarInfo;


    public Text TeksScore;

    int TargetScore = 4600;

    private void OnEnable()
    {
        DataTotalScore = SistemGame.DataScore;
        TeksScore.text = DataTotalScore.ToString("NO");

        if (DataTotalScore > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", DataTotalScore);
        }
        if (DataTotalScore >= TargetScore)
        {
            GambarInfoAtas.sprite = GambarInfo[0];
        }
        else
        {
            GambarInfoAtas.sprite = GambarInfo[1];
        }
    }
}