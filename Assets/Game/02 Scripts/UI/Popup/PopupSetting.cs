using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : SingletonPopup<PopupSetting>
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

    public void OnclickCollection()
    {
        base.Hide(() =>
        {
            PopupCollection.Instance.Show();
        });
    }
}
