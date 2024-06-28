using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sistem_Drag : MonoBehaviour
{
    public int IDDrag;
    public SpriteRenderer SR;
    public Vector2 SavePos;
    public BoxCollider2D Collider;
    public bool IsDiatasDrop;
    public bool IsDragBenar;
    private void Start()
    {
        SavePos = transform.position;
    }
    private void OnMouseDown()
    {
        KumpulanSuara.instance.Panggil_Sfx(0);
    }



    private void OnMouseDrag()
    {
        if(!IsDragBenar)
        {
            Vector2 PosTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = PosTarget;
            SR.sortingOrder = 5;
        }
        
    }
    private void OnMouseUp()
    {
        if (!IsDragBenar)
        {
            if (IsDiatasDrop)
            {
                if (SaveTempatDrop != null)
                {
                    if (IDDrag == SaveTempatDrop.IDDrop)
                    {
                        SR.sortingOrder = 1;
                        transform.position = SavePosTarget;
                        transform.localScale = SaveSizeTarget;
                        Collider.enabled = false;
                        IsDragBenar = true;
                        SaveTempatDrop = null;
                        SavePosTarget = Vector2.zero;

                        SistemGame.instance.IDKartu++;
                        SistemGame.instance.v_SetKartuDrag();

                        //ini jika sukses
                        SistemGame.instance.DataSaatIni++;
                        SistemGame.DataScore += 200;
                        KumpulanSuara.instance.Panggil_Sfx(1);
                    }
                    else
                    {
                        SR.sortingOrder = 0;
                        transform.position = SavePos;

                        //jika salah
                        SistemGame.DataDarah--;
                        KumpulanSuara.instance.Panggil_Sfx(2);
                    }
                }
                else
                {
                    SR.sortingOrder = 0;
                    transform.position = SavePos;

                    //jika tempat nya ada
                }
            }
            else
            {
                SR.sortingOrder = 0;
                transform.position = SavePos;
            }
        }
        
    }
    Vector2 SavePosTarget;
    Vector2 SaveSizeTarget;
    Sistem_Drop SaveTempatDrop;
    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiatasDrop = true;
            SavePosTarget = trig.transform.position;
            SaveSizeTarget = trig.transform.localScale;
            SaveTempatDrop = trig.gameObject.GetComponent<Sistem_Drop>();
        }
    }
    private void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiatasDrop = false;
        }
    }
}