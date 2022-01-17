using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayQuanTinh_HelicopterMinigame2 : MonoBehaviour
{
    public Vector3 angle = new Vector3(0, 0, 10);
    public Vector3 angle1 = new Vector3(0, 0, -10);
    public HelicopterObj_HelicopterMinigame2 helicopterObj;
    public float startPosY;
    public float deltaPosY;
    public bool isActive;
    public SpriteRenderer line;




    private void Start()
    {
        startPosY = transform.position.y;
        deltaPosY = helicopterObj.transform.position.y - startPosY;
        line = transform.GetChild(0).GetComponent<SpriteRenderer>();
        line.size = new Vector2(0, 0.07f);
    }

    public void Handle_EventQuanTinh(int index)
    {
        if (isActive)
        {
            if (index == 1)
            {
                transform.DOKill();
                transform.DORotate(angle, 1).SetEase(Ease.Linear);
            }
            if (index == 2)
            {
                transform.DOKill();
                transform.DORotate(angle1, 1).SetEase(Ease.Linear);
            }
            if (index == 3)
            {
                transform.DOKill();
                transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.Linear);
            }
        }
    }

    private void Update()
    {
        transform.position = new Vector3(helicopterObj.transform.position.x, helicopterObj.transform.position.y - deltaPosY, helicopterObj.transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }

    private void OnEnable()
    {
        HelicopterObj_HelicopterMinigame2.Event_QuanTinh += Handle_EventQuanTinh;
    }
    private void OnDisable()
    {
        HelicopterObj_HelicopterMinigame2.Event_QuanTinh -= Handle_EventQuanTinh;
    }
}
