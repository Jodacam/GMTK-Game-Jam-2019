using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUi : MonoBehaviour
{
    public enum RoomType
    {
        Normal,
        Shop,
        Boss,
    }

    float scrollDownDistance;

    public Dictionary<RoomType, GameObject> tiles;

    public GameObject blankSquare;
    public GameObject bossSquare;
    public GameObject shopSquare;

    public List<RectTransform> objects;

    RectTransform rectTransform;

    int pos = 0;

    public float scrollTime;
    float currentScrollTime;

    public bool generated = false;

    public GameObject flecha;

    Vector2 initPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initPos = rectTransform.anchoredPosition;
        tiles = new Dictionary<RoomType, GameObject>();
        tiles.Add(RoomType.Normal, blankSquare);
        tiles.Add(RoomType.Boss, bossSquare);
        tiles.Add(RoomType.Shop, shopSquare);
    }

    public void NextTile(RoomType type)
    {
        if(!generated){
        RectTransform r = Instantiate(tiles[type], Vector3.zero, Quaternion.Euler(0, 0, 0), this.transform).GetComponent<RectTransform>();
        if (objects.Count > 0)
        {
            r.anchoredPosition = objects[objects.Count-1].anchoredPosition + Vector2.up * 150;
            objects.Add(r);
        }else{
            r.anchoredPosition = Vector2.zero;
            objects.Add(r);

            flecha.SetActive(true);
        }
        }
    }

    public void Scroll(){
        StartCoroutine(ScrollOnce());
    }

    // Update is called once per frame
    IEnumerator ScrollOnce()
    {
        currentScrollTime = scrollTime;
        Vector2 initialPos = rectTransform.anchoredPosition;
        Vector2 finalPos = rectTransform.anchoredPosition - Vector2.up * 150;

        pos = pos < 2 ? pos + 1 : pos;

        if (pos >= 2)
        {
            RectTransform t = objects[0];
            objects.RemoveAt(0);
            t.anchoredPosition = objects[objects.Count - 1].anchoredPosition + Vector2.up * 150;
            objects.Add(t);
        }
        
        while (currentScrollTime > 0)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, finalPos, 1 - currentScrollTime);

            yield return null;
            currentScrollTime -= Time.deltaTime;
        }
        
    }

    public void Wipe(){
        flecha.SetActive(false);
        foreach(RectTransform r in objects){
            Destroy(r.gameObject);
        }
        rectTransform.anchoredPosition = initPos;
        objects.Clear();
    }
}
