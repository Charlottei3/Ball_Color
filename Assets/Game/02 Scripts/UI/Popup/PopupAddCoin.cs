using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAddCoin : SingletonPopup<PopupAddCoin>
{
    public void Show()
    {
        base.canCloseWithOverlay = true;
        base.Show();
    }

    public void Close()
    {
        base.Hide();
    }

    public void OnClickAddCoin()
    {
        //base.Hide(() =>
        //{

        //});
    }
}
