using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data
{
    public static int DataLevel, DataScore, DataWaktu, DataDarah;
}

public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;

    int MaxLevel = 5 ;

    [Header("DataPermainan")]
    public bool GameAktif;
    public bool GameSelesai;
    public bool SistemAcak;
    public int Target,DataSaatIni;
    private float s;
    

    [Header("Komponen UI")]
    public Text teks_Level;
    public Text teks_Waktu, Teks_Score;
    public RectTransform Ui_Darah;

    [Header("Obj GUI")]
    public GameObject Gui_Pause;
    public GameObject Gui_Transisi;

    [System.Serializable]
    public class DataGame
    {
        public string Nama;
        public Sprite Gambar;
    }

    [Header("Settingan Standar")]
    public DataGame[] DataPermainan;

    [Space]
    public Obj_TempatDrop[] Drop_Tempat;
    public Obj_Drag[] Drag_Obj;


    void OnEnable()
    {
        instance = this;
    }

    void Start()
    {
        GameAktif = false;
        GameSelesai = false;
        ResetData();
        Target = Drop_Tempat.Length;
        if(SistemAcak)
        AcakSoal();
        DataSaatIni = 0;
        GameAktif = true;
    }

    void ResetData()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game0")
        {
            Data.DataWaktu = 60 * 3;
            Data.DataScore = 0;
            Data.DataDarah = 5;
            Data.DataLevel = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AcakSoal();

        if (GameAktif && !GameSelesai)
        {
            if (Data.DataWaktu > 0)
            {
                s += Time.deltaTime;
                if (s >= 1)
                {
                    Data.DataWaktu--;
                    s = 0;
                }
            }
            if (Data.DataWaktu <= 0)
            {
                GameAktif = false;
                GameSelesai = true;
                // Game kalah
                KumpulanSuara.instance.Panggil_Sfx(4);
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
            }
            if(Data.DataDarah <= 0) 
            {
                GameSelesai = true;
                GameAktif = false;

                //Fungsi Kalah
                KumpulanSuara.instance.Panggil_Sfx(4);
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
            }

            if (DataSaatIni >= Target)
            {
                GameSelesai = true;
                GameAktif = false;

                // Kita menang
                if (Data.DataLevel < (MaxLevel - 1))
                {
                    Data.DataLevel++;
                    // Pindah ke Next Level

                    UnityEngine.SceneManagement.SceneManager.LoadScene("Game" + Data.DataLevel);
                    //Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("Game + Data.DataLevel");
                    KumpulanSuara.instance.Panggil_Sfx(3);
                }
                else
                {
                    // Game selesai, pindah ke menu selesai
                    Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
                    KumpulanSuara.instance.Panggil_Sfx(5);
                    KumpulanSuara.instance.Panggil_Sfx(6);
                }
            }
        }
        SetInfoUI();
    }

    [HideInInspector] public List<int> _AcakSoal = new List<int>();
    [HideInInspector] public List<int> _AcakPos = new List<int>();
    private int rand;
    private int rand2;

    public void AcakSoal()
    {
        _AcakSoal.Clear();
        _AcakPos.Clear();

        // Acak pertanyaan
        _AcakSoal = new List<int>(new int[Drag_Obj.Length]);

        for (int i = 0; i < Drag_Obj.Length; i++)
        {
            rand = Random.Range(1, DataPermainan.Length);
            while (_AcakSoal.Contains(rand))
            {
                rand = Random.Range(1, DataPermainan.Length);
            }

            _AcakSoal[i] = rand;
            Drag_Obj[i].ID = rand - 1;
            Drag_Obj[i].Teks.text = DataPermainan[rand - 1].Nama;
        }

        // Acak posisi drop
        _AcakPos = new List<int>(new int[Drop_Tempat.Length]);
        for (int i = 0; i < _AcakPos.Count; i++)
        {
            rand2 = Random.Range(0, _AcakSoal.Count+1);
            while (_AcakPos.Contains(rand2))
            {
                rand2 = Random.Range(0, _AcakSoal.Count+1);
            }

            _AcakPos[i] = rand2;
            Drop_Tempat[i].Drop.ID = _AcakSoal[rand2 - 1] - 1;
            Drop_Tempat[i].Gambar.sprite = DataPermainan[Drop_Tempat[i].Drop.ID].Gambar;
        }
    }



    public void SetInfoUI()
    {
        teks_Level.text = (Data.DataLevel + 1).ToString();

        int Menit = Mathf.FloorToInt(Data.DataWaktu / 60);//01
        int Detik = Mathf.FloorToInt(Data.DataWaktu % 60);//30
        teks_Waktu.text = Menit.ToString("00") + ":" + Detik.ToString("00");

        Teks_Score.text = Data.DataScore.ToString();

        Ui_Darah.sizeDelta = new Vector2(46f * Data.DataDarah , 39f);
    }

    public void Btn_Pause(bool pause)
    {
        if(pause)
        {
            GameAktif = false;
            Gui_Pause.SetActive(true);
        }
        else
        {
            GameAktif = true;
            Gui_Pause.SetActive(false);
        }
    }
}