using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SistemGame : MonoBehaviour
{
    public static SistemGame instance;
    public static bool NewGame = true;
    public static int IDGame;
    public int IDKartu;
    public int TargetKartu;

    [Header("Data Permainan")]
    public bool GameAktif;
    public bool GameFinish;
    public int DataTargetKartu;
    public int Target, DataSaatIni;
    public int SistemAcak;
    public static int DataLevel, DataScore, DataWaktu, DataDarah;

    [Header("Komponen UI")]
    public Text teks_Level;
    public Text teks_Waktu;
    public Text teks_Score;
    public RectTransform Ui_Darah;

    [Header("Obj GUI")]
    public GameObject Gui_Pause;
    public GameObject Gui_Transisi;

    [Header("Sistem Kartu")]
    public Transform TempatKartu;
    public GameObject KartuIkan;
    public Sistem_Drop[] KartuDrop;
    public Sprite[] GambarIkan;

    [Header("Sistem Acaknya")]
    public List<int> AcakSoal = new List<int>();
    public List<int> AcakUrutanMuncul = new List<int>();


    [Space]
    public Sistem_Drop[] Sistem_Drop;

    float s;

    private void OnEnable()
    {
        instance = this;
        v_SetDataAwal();
        v_AcakSoal();
    }

    private void Start()
    {
        GameAktif = false;
        GameFinish = false;
        ResetData();
        Target = Sistem_Drop.Length;
        if (AcakSoal.Count > 0)
            GameAktif = true;
    }

    void ResetData()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameTebak0")
        {
            DataWaktu = 60 * 3;
            DataScore = 0;
            DataDarah = 5;
            DataLevel = 0;
        }
    }

    private void Update()
    {
        SetInfoUI();

        if (GameAktif && !GameFinish)
        {
            if (DataWaktu > 0)
            {
                s += Time.deltaTime;
                if (s >= 1)
                {
                    DataWaktu--;
                    s = 0;
                }
            }
            if (DataWaktu <= 0)
            {
                GameAktif = false;
                GameFinish = true;
                // Game kalah
                KumpulanSuara.instance.Panggil_Sfx(4);
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameFinish");
            }
            if (DataDarah <= 0)
            {
                GameFinish = true;
                GameAktif = false;

                //Fungsi Kalah
                KumpulanSuara.instance.Panggil_Sfx(4);
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameFinish");
            }
        }
    }

    public void v_SetDataAwal()
    {
        IDKartu = 0;
        DataTargetKartu = KartuDrop.Length;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameTebak0")
        {
            DataScore = 0;
            DataDarah = 5;
            IDGame = 0;
        }
        SetInfoUI();
    }

    public void v_AcakSoal()
    {
        AcakSoal.Clear();
        AcakSoal = new List<int>(new int[KartuDrop.Length]);
        int Rand = 0;
        for (int i = 0; i < AcakSoal.Count; i++)
        {
            Rand = Random.Range(1, GambarIkan.Length);
            while (AcakSoal.Contains(Rand))
            {
                Rand = Random.Range(1, GambarIkan.Length);
            }
            AcakSoal[i] = Rand;
            KartuDrop[i].IDDrop = Rand - 1;
            KartuDrop[i].SR.sprite = GambarIkan[Rand - 1];
        }

        v_AcakUrutanMuncul();
    }

    public void SetInfoUI()
    {
        teks_Level.text = (IDGame + 1).ToString();

        int Menit = Mathf.FloorToInt(DataWaktu / 60);
        int Detik = Mathf.FloorToInt(DataWaktu % 60);
        teks_Waktu.text = Menit.ToString("00") + ":" + Detik.ToString("00");

        teks_Score.text = DataScore.ToString();

        Ui_Darah.sizeDelta = new Vector2(46f * DataDarah, 39f);
    }


    public void v_AcakUrutanMuncul()
    {
        AcakUrutanMuncul.Clear();
        AcakUrutanMuncul = new List<int>(new int[KartuDrop.Length]);
        int Rand = 0;
        for (int i = 0; i < AcakUrutanMuncul.Count; i++)
        {
            Rand = Random.Range(1, AcakSoal.Count + 1);

            while (AcakUrutanMuncul.Contains(Rand))
                Rand = Random.Range(1, AcakSoal.Count + 1);

            AcakUrutanMuncul[i] = Rand;
        }

        v_SetKartuDrag();
    }

    public void v_SetKartuDrag()
    {
        if (IDKartu < DataTargetKartu)
        {
            GameObject Kartu = Instantiate(KartuIkan);
            Kartu.transform.position = TempatKartu.position;
            Kartu.transform.localScale = TempatKartu.localScale;
            Sistem_Drag SistemKartuDrag = Kartu.GetComponent<Sistem_Drag>();
            int DataKartu = AcakSoal[AcakUrutanMuncul[IDKartu] - 1] - 1;
            SistemKartuDrag.IDDrag = DataKartu;
            SistemKartuDrag.SR.sprite = GambarIkan[DataKartu];
            SistemKartuDrag.SavePos = TempatKartu.position;
        }
        else
        {
            KumpulanSuara.instance.Panggil_Sfx(3);
            IDGame++;

            int TargetLevel = 5;
            if (IDGame >= TargetLevel) //Max 5 Level
            {
                Debug.Log("GameFinish");
                IDGame = TargetLevel - 1;
                SceneManager.LoadScene("GameFinish");
                KumpulanSuara.instance.Panggil_Sfx(5);
                KumpulanSuara.instance.Panggil_Sfx(6);
                SetInfoUI();
            }
            else
            {
                string TargetSceneSelanjutnya = "GameTebak" + IDGame;
                SceneManager.LoadScene(TargetSceneSelanjutnya);
            }
            SetInfoUI();

        }
    }

    public void Btn_Pause(bool pause)
    {
        if (pause)
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
