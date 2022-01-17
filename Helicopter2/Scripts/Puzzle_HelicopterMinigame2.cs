using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_HelicopterMinigame2 : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isDangBiHut;
    public int myIndex;
    public Transform correctPos;
    public bool isCorrect = false;
    public GameObject VFXShine;


    private void Start()
    {
        isDangBiHut = false;
        VFXShine = GameController_HelicopterMinigame2.instance.VFXShine;
    }


    void VFXNextLv()
    {
        GameController_HelicopterMinigame2.instance.parentVFXNextLv.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 2, 1, 1).OnComplete(() =>
        {
            var tmpLevel = ++GameController_HelicopterMinigame2.instance.level;
            GameController_HelicopterMinigame2.instance.SetLevel(tmpLevel);
        });
    }

    void CheckLevel(Puzzle_HelicopterMinigame2 obj)
    {
        if (!GameController_HelicopterMinigame2.instance.listCheckItem.Contains(obj))
        {
            GameController_HelicopterMinigame2.instance.listCheckItem.Add(obj);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Path") && (collision.transform.position.y - transform.position.y) > 0.2f && !isDangBiHut)
        {
            if (collision.transform.GetComponent<Puzzle_HelicopterMinigame2>().myIndex - myIndex == 1)
            {
                transform.eulerAngles = Vector3.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                collision.transform.position = correctPos.position;
                rb.gravityScale = 0;           
                var tmpCollisionPuzzle = collision.transform.GetComponent<Puzzle_HelicopterMinigame2>();
                tmpCollisionPuzzle.isCorrect = true;
                tmpCollisionPuzzle.rb.gravityScale = 0;
                tmpCollisionPuzzle.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                collision.transform.eulerAngles = Vector3.zero;
                GetComponent<Collider2D>().isTrigger = true;
                var tmpVFX = Instantiate(VFXShine, collision.contacts[0].point, Quaternion.identity);
                tmpVFX.GetComponent<SpriteRenderer>().DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Destroy(tmpVFX.gameObject);
                });

                isCorrect = true;

                CheckLevel(collision.gameObject.GetComponent<Puzzle_HelicopterMinigame2>());
                
                if (GameController_HelicopterMinigame2.instance.listCheckItem.Count == GameController_HelicopterMinigame2.instance.allCurrentItem.Count - 1)
                {
                    if (GameController_HelicopterMinigame2.instance.level < 5)
                    {
                        Debug.Log("next level");
                        var tmpListItem = GameController_HelicopterMinigame2.instance.allCurrentItem;
                        if (tmpListItem.Count != 0)
                        {
                            for (int i = 0; i < tmpListItem.Count; i++)
                            {
                                tmpListItem[i].transform.parent = GameController_HelicopterMinigame2.instance.parentVFXNextLv.transform;
                            }
                        }
                        VFXNextLv();
                    }
                    else
                    {
                        GameController_HelicopterMinigame2.instance.Win();
                    }
                }
            }
        }
    }
}
