using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DauHut_HelicopterMinigame2 : MonoBehaviour
{
    public bool isHutTrung;
    public HelicopterObj_HelicopterMinigame2 helicopterObj;
    public Puzzle_HelicopterMinigame2 current_Item;
    public DayQuanTinh_HelicopterMinigame2 day;
    public GameObject ground;



    private void Update()
    {
        if (current_Item != null)
        {
            if (current_Item.transform.position.y < ground.transform.position.y)
            {
                helicopterObj.btnHut.interactable = false;
            }
            else if (!helicopterObj.btnHut.interactable)
            {
                helicopterObj.btnHut.interactable = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(current_Item == null)
        {
            if (collision.gameObject.CompareTag("Path") && !collision.GetComponent<Puzzle_HelicopterMinigame2>().isCorrect)
            {
                if (!GameController_HelicopterMinigame2.instance.imgTime.gameObject.activeSelf)
                {
                    GameController_HelicopterMinigame2.instance.Begin();
                }
                isHutTrung = true;
                helicopterObj.isHut = false;
                collision.transform.parent = day.transform;
                current_Item = collision.gameObject.GetComponent<Puzzle_HelicopterMinigame2>();
                current_Item.rb.isKinematic = true;
                current_Item.GetComponent<Collider2D>().isTrigger = true;
                helicopterObj.btnHut.interactable = true;
                day.isActive = true;
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if(current_Item != null)
        {
            if (collision.gameObject.CompareTag("Path"))
            {
                if (!current_Item.rb.isKinematic)
                {                   
                    current_Item.isDangBiHut = false;
                    isHutTrung = false;
                    helicopterObj.btnHut.interactable = false;
                    current_Item = null;
                }               
            }
        }      
    }
}
