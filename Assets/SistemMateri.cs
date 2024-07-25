using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemMateri : MonoBehaviour
{
    [System.Serializable]
    public class DataMateri
    {
        public string Materi_Nama;
        public string Materi_Penjelasan;
        public Sprite Materi_Gambar;
        public AudioClip Materi_Suara;
    }

    public List<DataMateri> _Data;
    [Header("Data Component")]
    public int Data_Materi;
    public Image Gambar_Materi;
    public Text Teks_NamaIkan;
    public Text Teks_Nomor;
    public Text Teks_Penjelasan;

    public AudioSource SourceSuara;

    private void Start()
    {
        Data_Materi = 0;
        v_SetMateri();
    }

    public void v_Tombol(bool ArahKanan)
    {
        if (ArahKanan)
        {
            Data_Materi++;

            if (Data_Materi >= _Data.Count - 1)
            {
                Data_Materi = 0;
            }
        }
        else
        {
            Data_Materi--;
            if (Data_Materi <= 0)
            {
                Data_Materi = _Data.Count - 1;
            }
        }

        v_SetMateri();
    }


    public void v_SetMateri()
    {
        Gambar_Materi.GetComponent<Animation>().Play("Animasi Tombol");

        Gambar_Materi.sprite = _Data[Data_Materi].Materi_Gambar;
        Teks_NamaIkan.text = _Data[Data_Materi].Materi_Nama;
        Teks_Penjelasan.text = _Data[Data_Materi].Materi_Penjelasan;


        v_SetSuara();

        Teks_Nomor.text = (Data_Materi + 1) + " / " + (_Data.Count);
    }

    public void v_SetSuara()
    {
        if (SourceSuara.clip != null && SourceSuara.isPlaying)
        {
            SourceSuara.Stop();
        }

        SourceSuara.clip = _Data[Data_Materi].Materi_Suara;
    }

    public void v_PanggilSuara()
    {
        if (SourceSuara.clip == null)
        {
            return;
        }

        if (SourceSuara.isPlaying)
        {
            SourceSuara.Pause();
        }
        else
        {
            SourceSuara.Play();
        }
    }
}
