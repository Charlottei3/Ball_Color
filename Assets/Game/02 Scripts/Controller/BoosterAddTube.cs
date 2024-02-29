using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterAddTube : BoosterController
{
    private void Awake()
    {
        ActionEvent.OnResetGamePlay += UpdateQuantily;
    }

    public void Start()
    {
        UpdateQuantily();
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= UpdateQuantily;
    }

    public void OnClickAddTube()
    {
        if (PlayerData.UserData.BoosterAddNumber > 0)
        {
            ActionEvent.OnUserBoosterAdd?.Invoke();
            DisplayBooster(PlayerData.UserData.BoosterAddNumber);
        }
        else
        {
            Debug.Log("Show ads");
        }

    }

    private void UpdateQuantily()
    {
        int quatily = PlayerData.UserData.BoosterAddNumber;
        PlayerData.UserData.UpdateValueBooster(this.Type, 4 - quatily);
        base.DisplayBooster(PlayerData.UserData.BoosterAddNumber);
    }
}
