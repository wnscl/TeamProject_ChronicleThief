using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;

public class MainLobbyInter : MapInteraction
{
    protected override void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }


    protected override void Interact()
    {
        Debug.Log("MainLobby: 입력 상호작용 작동.");
    }
}
