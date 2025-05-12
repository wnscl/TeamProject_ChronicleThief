using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject EnterObject;
    [SerializeField] private GameObject ExitObject;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ExitObject.SetActive(true);
            EnterObject.SetActive(false);
        }
    }
}
