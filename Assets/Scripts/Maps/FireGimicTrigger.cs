using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireGimicTrigger : MonoBehaviour
{
    public GameObject[] fireObjects;

    void Start()
    {
        FindObject();
    }

    void FindObject()
    {
        GameObject parentObject = GameObject.Find("FireGimicTrigger");
        if (parentObject != null)
        {
            fireObjects = parentObject.GetComponentsInChildren<Transform>(true)
                .Select(t => t.gameObject)
                .Where(go => go.CompareTag("FirePoint"))
                .ToArray();
            Debug.Log("fireObjects: " + fireObjects.Length + " 등록");
        }
        else
        {
            Debug.Log("부모 오브젝트를 찾을 수 없음.");
        }
    }

    public void FireChildren()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true)
            .OrderBy(child => {
                string numberPart = child.name.Replace("FirePoint", "");
                return int.TryParse(numberPart, out int result) ? result : int. MaxValue;
            })
            .ToArray();

        foreach (Transform child in children)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                Debug.Log($"활성화: {child.gameObject.name}");
                break;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FireChildren();
        }
    }
}
