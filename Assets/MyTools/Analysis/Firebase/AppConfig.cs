using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfig : SingletonMonoBehaviour<AppConfig>
{
    public override void Awake()
    {
        SetDefaultConfigValue();
        base.Awake();
    }

    public void SetDefaultConfigValue()
    {

    }
}

