using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Musik : MonoBehaviour
{
    public Sprite[] GambarTombol;
    public Image Tombol;

    private void OnEnable()
    {
        if (KumpulanSuara.instance.source_bgm.isPlaying)
        {
            Tombol.sprite = GambarTombol[0];
        }
        else
        {
            Tombol.sprite = GambarTombol[1];
        }
    }
    public void v_BtnMusik()
    {
        if (KumpulanSuara.instance.source_bgm.isPlaying)
        {
            KumpulanSuara.instance.source_bgm.Pause();
            Tombol.sprite = GambarTombol[1];
        }
        else
        {
            KumpulanSuara.instance.source_bgm.UnPause();
            Tombol.sprite = GambarTombol[0];
        }
    }
}