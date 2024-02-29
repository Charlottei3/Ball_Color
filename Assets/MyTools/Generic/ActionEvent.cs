using System;

public static class ActionEvent
{
    public static Action OnResetGamePlay;
    #region Booster
    public static Action OnUseBoosterRevoke;
    public static Action OnUserBoosterAdd;
    #endregion
    #region CUrrentcy
    public static Action OnUpdateCoin;
    #endregion
    #region Collection
    public static Action<bool> OnChangeEquip;
    #endregion
}
