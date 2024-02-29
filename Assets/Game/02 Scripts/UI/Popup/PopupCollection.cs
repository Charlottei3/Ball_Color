using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PopupCollection : SingletonPopup<PopupCollection>
{
    public SpriteAtlas atlasBG, atlasBall, atlasTube;
    public ItemCollection _itemTubePrefab, _itemBallPrefab, _itemThemePrefab;
    [SerializeField] Transform[] _tabTrans;
    [SerializeField] TabButtonBase[] _btnTrans;
    [SerializeField] Sprite spr, def;
    [SerializeField] Color[] _textColorArr;

    public void Show()
    {
        base.Show();
    }

    private void Start()
    {
        Init();
        OnClickCollection((int)TypeItemCollection.Tube);
    }

    public void Close()
    {
        base.Hide();
    }

    public void Init()
    {
        for (int i = 0; i < atlasTube.spriteCount; i++)
        {
            ItemCollection item = Instantiate(_itemTubePrefab, _tabTrans[0]);

            DataItemSkin data = new DataItemSkin(i, i < 4 ? 0 : 200 * i, atlasTube.GetSprite($"Ui_Rewards_Icon_Card_{i + 1:00}"), i * 12);
            item.Init(data);
        }

        for (int i = 0; i < atlasBall.spriteCount; i++)
        {
            ItemCollection item = Instantiate(_itemBallPrefab, _tabTrans[1]);
            DataItemSkin data = new DataItemSkin(i, i < 6 ? 0 : 150 * i, atlasBall.GetSprite($"Ui_Shop_Ball{i + 1:00}"), i * 14);
            item.Init(data);
        }

        for (int i = 0; i < atlasBG.spriteCount; i++)
        {
            ItemCollection item = Instantiate(_itemThemePrefab, _tabTrans[2]);
            DataItemSkin data = new DataItemSkin(i, i < 5 ? 0 : 100 * i, atlasBG.GetSprite($"Ui_Shop_Theme{i + 1:00}_B"), i * 16);
            item.Init(data);
        }
    }

    public void OnClickCollection(int index)
    {
        for (int i = 0; i < _tabTrans.Length; i++)
        {
            _tabTrans[i].parent.gameObject.SetActive(i == index);
            if (i == index)
            {
                _btnTrans[i].OnClickTabChangeImg(spr, _textColorArr[0]);
            }
            else
            {
                _btnTrans[i].OnClickTabChangeImg(def, _textColorArr[1]);
            }
        }
    }
}
