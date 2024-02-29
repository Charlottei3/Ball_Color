using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataItemSkin
{
    public int Index;
    public Sprite Sprite;
    public int Price;
    public int LevelUnlock;

    public DataItemSkin(int index, int price, Sprite sprite, int Levelunlock)
    {
        Index = index;
        Price = price;
        Sprite = sprite;
        this.LevelUnlock = Levelunlock;
    }
}

public class ItemCollection : MonoBehaviour
{
    public DataItemSkin dataBall;
    [SerializeField] TypeItemCollection type;
    [SerializeField] Image iconItem;
    [SerializeField] TMP_Text textLv, textPrice;
    [SerializeField] GameObject mask, iconReceive;
    [SerializeField] GameObject _buyWithCoinBtn, _buyWithAdsBtn;

    private bool _isUnlock, _isEquip;

    private void OnEnable()
    {
        ActionEvent.OnChangeEquip += ChangeEquip;
    }

    private void Start()
    {
        DisplayItem();
    }

    private void OnDisable()
    {
        ActionEvent.OnChangeEquip -= ChangeEquip;
    }

    public void Init(DataItemSkin data)
    {
        dataBall = data;
    }

    public void DisplayItem()
    {
        iconItem.sprite = dataBall.Sprite;

        _isUnlock = CollectionData.ShopData.getItemCol(type, dataBall.Index).IsUnlock;

        _isEquip = CollectionData.ShopData.getItemCol(type, dataBall.Index).isEquip;

        mask.SetActive(!_isUnlock);
        if (_isUnlock)
        {
            DisplayItemUnlock();
        }
        else
        {
            textLv.gameObject.SetActive(dataBall.Price <= 0);

            textPrice.text = $"{dataBall.Price}";
            textLv.text = $"Lv. {dataBall.LevelUnlock}";

            if (dataBall.Price > 0)
            {
                _buyWithCoinBtn.SetActive(true);
                textLv.gameObject.SetActive(false);
                _buyWithAdsBtn.SetActive(false);
            }
            else
            {
                _buyWithCoinBtn.SetActive(false);
                if (dataBall.LevelUnlock < PlayerData.UserData.HighestLevel)
                {
                    textLv.gameObject.SetActive(false);
                    _buyWithAdsBtn.SetActive(true);
                }
                else
                {
                    textLv.gameObject.SetActive(true);
                    _buyWithAdsBtn.SetActive(false);
                }
            }
        }
    }

    private void DisplayItemUnlock()
    {
        mask.SetActive(!_isUnlock);

        iconReceive.SetActive(_isEquip);

        _buyWithAdsBtn.SetActive(!_isUnlock);

        _buyWithCoinBtn.SetActive(!_isUnlock);
    }

    public void ChangeEquip(bool value)
    {
        if (!_isUnlock) return;
        _isEquip = value;
        iconReceive.SetActive(value);
    }

    public void OnClickEquip()
    {
        if (!_isUnlock)
        {
            Debug.Log("Ban chua mo khoa skin");
            return;
        }
        if (_isEquip) return;
        ActionEvent.OnChangeEquip?.Invoke(false);
        _isEquip = true;
        iconReceive.SetActive(true);
    }

    public void OnClickBuyWithCoin()
    {
        PlayerData.UserData.UseCoin(dataBall.Price, (n) =>
        {
            if (n)
            {
                ActionEvent.OnUpdateCoin?.Invoke();
                ActionEvent.OnChangeEquip?.Invoke(false);
                _isUnlock = true;
                _isEquip = true;
                DisplayItemUnlock();
            }
            else
            {
                Debug.Log("not enought coin");
            }
        });
    }

    public void OnClickBuyWithAds()
    {

    }
}

public enum TypeItemCollection
{
    Tube = 0,
    Theme = 1,
    Ball = 2,
}