using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterObj_HelicopterMinigame2 : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody2D rb;
    public Vector2 direction;
    public Camera mainCamera;
    public Vector2 maxPosCam;
    public Vector3 lastPos;
    public static event Action<int> Event_QuanTinh;
    public Button btnHut;
    public GameObject currentPuzzle;
    public DayQuanTinh_HelicopterMinigame2 day;
    public GameObject lineSample;


    public DauHut_HelicopterMinigame2 dauHut;
    public bool isHut, isComeBack, isThaDay;



    private void Awake()
    {
        isThaDay = false;
        isHut = false;
        isComeBack = false;
    }

    private void Start()
    {
        speed = 7;
        maxPosCam = new Vector2(mainCamera.orthographicSize * Screen.width * 1.0f / Screen.height, mainCamera.orthographicSize);
        btnHut.onClick.AddListener(OnClickHut);
        lineSample.SetActive(false);
    }

    public void FlipCar()
    {
        if (transform.position.x > lastPos.x)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            Event_QuanTinh?.Invoke(2);
        }
        if (transform.position.x < lastPos.x)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            Event_QuanTinh?.Invoke(1);
        }

        lastPos = transform.position;
    }


    public void OnClickHut()
    {
        if (GameController_HelicopterMinigame2.instance.tutorial2.activeSelf)
        {
            GameController_HelicopterMinigame2.instance.tutorial2.SetActive(false);
        }

        if (dauHut.current_Item == null)
        {
            isHut = true;
            isThaDay = true;
            btnHut.interactable = false;
            lineSample.SetActive(false);
            dauHut.gameObject.SetActive(true);
        }
        else
        {
            dauHut.current_Item.rb.isKinematic = false;
            isComeBack = true;
            btnHut.interactable = false;
            dauHut.current_Item.transform.parent = null;
            dauHut.current_Item.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public void DauHut_Hut()
    {
        day.transform.eulerAngles = Vector3.zero;
        if (dauHut.transform.position.y > -3.9f || (transform.position.y - dauHut.transform.position.y) < 3.98f)
        {
            dauHut.transform.Translate(Vector3.down * 5 * Time.deltaTime);
            day.line.size += new Vector2(3, 0) * 5 * Time.deltaTime;
        }
        if (!dauHut.isHutTrung)
        {
            if (dauHut.transform.position.y <= -3.9f || (transform.position.y - dauHut.transform.position.y) >= 3.98f)
            {
                Debug.Log("quay ve");
                isHut = false;
                isThaDay = false;
                isComeBack = true;
            }
        }
    }

    public void DauHut_ComeBack()
    {
        if (dauHut.transform.localPosition.y < 0.46f)
        {
            dauHut.transform.Translate(Vector3.up * 5 * Time.deltaTime);
            day.line.size += new Vector2(-3, 0) * 5 * Time.deltaTime;
            dauHut.GetComponent<CircleCollider2D>().enabled = false;
        }
        else
        {
            btnHut.interactable = true;
            day.isActive = false;
            isComeBack = false;
            lineSample.SetActive(true);
            dauHut.GetComponent<CircleCollider2D>().enabled = true;
            dauHut.gameObject.SetActive(false);
        }
    }

    


    private void Update()
    {
        if(!GameController_HelicopterMinigame2.instance.isWin && !GameController_HelicopterMinigame2.instance.isLose)
        {
            if (isHut)
            {
                DauHut_Hut();
            }

            if (isComeBack)
            {
                DauHut_ComeBack();
            }
        }      
    }

    public void FixedUpdate()
    {
        if (!GameController_HelicopterMinigame2.instance.isWin && !GameController_HelicopterMinigame2.instance.isLose)
        {
            if (variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0)
            {
                if (GameController_HelicopterMinigame2.instance.tutorial1.activeSelf)
                {
                    GameController_HelicopterMinigame2.instance.tutorial1.SetActive(false);
                }
                FlipCar();
            }
            else
            {
                Event_QuanTinh?.Invoke(3);
            }
            direction = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical * 0.7f);
            rb.velocity = direction * speed;

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -maxPosCam.x + 1.5f, maxPosCam.x - 1.5f), Mathf.Clamp(transform.position.y, -maxPosCam.y + 1, maxPosCam.y - 1));
        }
    }


}
