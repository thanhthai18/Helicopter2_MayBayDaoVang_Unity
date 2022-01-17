using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_HelicopterMinigame2 : MonoBehaviour
{
    public static GameController_HelicopterMinigame2 instance;

    public bool isWin, isLose, isBegin;
    public Camera mainCamera;
    public List<Transform> listSpawnPos = new List<Transform>();
    public List<Puzzle_HelicopterMinigame2> allCurrentItem = new List<Puzzle_HelicopterMinigame2>();
    public int level;
    public int time;
    public Coroutine timeCoroutine;
    public Text txtTime;
    public Image imgTime;
    public HelicopterObj_HelicopterMinigame2 helicopterObj;
    public GameObject tutorial1, tutorial2;
    public float startSizeCamera;
    public GameObject VFXShine;
    public GameObject parentVFXNextLv;
    public List<Puzzle_HelicopterMinigame2> listCheckItem = new List<Puzzle_HelicopterMinigame2>();


    [SerializeField]
    public List<ScriptableObjDataPrefab_HelicopterMinigame2> listPrefabPuzzle = new List<ScriptableObjDataPrefab_HelicopterMinigame2>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isBegin = false;
    }

    private void Start()
    {
        SetSizeCamera();
        startSizeCamera = mainCamera.orthographicSize;
        mainCamera.orthographicSize *= 2.0f / 5;
        imgTime.gameObject.SetActive(false);
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutorial1.transform.position = new Vector3(-5.79f, -4.13f, -5);
        tutorial2.transform.position = new Vector3(6.89f, -4.13f, -5);
        helicopterObj.btnHut.interactable = false;
        helicopterObj.variableJoystick.enabled = false;
        Intro();
    }

    void SetSizeCamera()
    {
        float f1, f2;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;
        mainCamera.orthographicSize *= f1 / f2;
    }

    void Intro()
    {
        mainCamera.DOOrthoSize(startSizeCamera, 3).SetEase(Ease.Linear).OnComplete(() =>
        {
            helicopterObj.lineSample.SetActive(true);
            helicopterObj.btnHut.interactable = true;
            helicopterObj.variableJoystick.enabled = true;
            tutorial1.SetActive(true);
            tutorial2.SetActive(true);
            level = 1;
            SetLevel(level);
        });
    }

    public void Begin()
    {
        imgTime.gameObject.SetActive(true);
        timeCoroutine = StartCoroutine(CountTime());
        isBegin = true;
    }

    public IEnumerator CountTime()
    {
        time = 45;
        txtTime.text = time.ToString();
        imgTime.fillAmount = time * 1.0f / 45;

        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            txtTime.text = time.ToString();
            imgTime.fillAmount = time * 1.0f / 45;

            if (time == 0)
            {
                Lose();
            }
        }
    }

    public void SetLevel(int index)
    {
        if (allCurrentItem.Count != 0)
        {
            for (int i = 0; i < allCurrentItem.Count; i++)
            {
                Destroy(allCurrentItem[i].gameObject);
            }
            allCurrentItem.Clear();
        }

        if (listCheckItem.Count != 0)
        {
            for (int i = 0; i < listCheckItem.Count; i++)
            {
                Destroy(listCheckItem[i].gameObject);
            }
            listCheckItem.Clear();
        }


        if (isBegin)
        {
            StopAllCoroutines();
            timeCoroutine = StartCoroutine(CountTime());
        }

        SpawnPuzzle(index - 1);
    }

    public void SpawnPuzzle(int index)
    {
        List<int> listCheckSame = new List<int>();
        for (int x = 0; x < listSpawnPos.Count; x++)
        {
            listCheckSame.Add(x);
        }


        for (int i = 0; i < listPrefabPuzzle[index].arrayPrefab.Length; i++)
        {
            int ran = Random.Range(0, listCheckSame.Count);
            allCurrentItem.Add(Instantiate(listPrefabPuzzle[index].arrayPrefab[i], listSpawnPos[listCheckSame[ran]].position, Quaternion.identity));
            listCheckSame.RemoveAt(ran);
        }
    }

    public void Win()
    {
        Debug.Log("Win");
        isWin = true;
        StopAllCoroutines();
        helicopterObj.btnHut.interactable = false;
        helicopterObj.speed = 0;
        helicopterObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        helicopterObj.variableJoystick.enabled = false;
        helicopterObj.transform.DOMoveX(helicopterObj.transform.position.x + 20, 3);
    }

    public void Lose()
    {
        Debug.Log("Thua");
        isLose = true;
        StopAllCoroutines();
        helicopterObj.speed = 0;
        helicopterObj.btnHut.interactable = false;
        helicopterObj.variableJoystick.enabled = false;
        txtTime.gameObject.SetActive(false);
    }
}
