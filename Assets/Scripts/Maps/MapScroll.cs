using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScroll : MonoBehaviour
{
    public float scrollSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        if (transform.position.x < -36f)
        {
            transform.position += new Vector3(36f * 2f, 0, 0);
        }
    }

    // 적들도 멈추면 화면만 이동해 버린다.
    // 스크롤을 플레이어 기준에 맞춰야...
    //플레이어와 맵이 이동하고 적들이 그걸 따라오는 로직...
    //무대는 x축으로 이동한다.
    //inZone 상태인 플레이어도 무대를 따라 이동한다.
    // 적들은 플레이어와 무대를 따라온다.
}
