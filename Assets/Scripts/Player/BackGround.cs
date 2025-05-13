using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public List<GameObject> stages; // Stage1~4 Tilemap 오브젝트들
    public float switchInterval = 2f;

    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(LoopTilemaps());
    }

    private IEnumerator LoopTilemaps()
    {
        while (true)
        {
            for (int i = 0; i < stages.Count; i++)
                stages[i].SetActive(i == currentIndex);

            currentIndex = (currentIndex + 1) % stages.Count;
            yield return new WaitForSeconds(switchInterval);
        }
    }
}
