using System;
using System.Collections;
using System.Collections.Generic;

public class UserData
{
    #region Varriables
    /// <summary>
    /// Achievements
    /// </summary>
    public int HighestLevel;
    public int Coin;
    /// <summary>
    /// Booster
    /// </summary>
    public int BoosterRevokeNumber;
    public int BoosterAddNumber;
    #endregion

    #region Method
    public void UpdateHighestLevel()
    {
        if (HighestLevel < DataManager.Instance.LevelDataSO.getListLevel() - 1)
            this.HighestLevel++;
    }

    public void UpdateValueBooster(TypeBooster type, int value)
    {
        switch (type)
        {
            case TypeBooster.Revoke:
                this.BoosterRevokeNumber += value;
                break;
            case TypeBooster.AddTube:
                this.BoosterAddNumber += value;
                break;
        }
    }

    public void EarnCoin(int value)
    {
        Coin += value;
    }

    public void UseCoin(int value, Action<bool> callBack)
    {
        if (Coin < value)
        {
            callBack?.Invoke(false);
            return;
        }
        Coin -= value;
        callBack?.Invoke(true);
    }
    #endregion
}
