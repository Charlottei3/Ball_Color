﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [Header("REFFERENCE")]
    private Camera _camera;
    private GameManager _gameManager;
    [SerializeField] List<TubeController> _tubes = new List<TubeController>();
    [SerializeField] private TubeController _hodingTube;
    [Space(10)]
    [Header("VALUE")]
    [SerializeField] float _minCamSize;
    [SerializeField] float _maxCamSize;
    private float _spaceHorizontal;
    [SerializeField] float _spaceVertical;
    [SerializeField] float _tubeHorizonlMax;
    private int _cdAddTube;
    private List<KeyValuePair<TubeController, TubeController>> _prevTube = new List<KeyValuePair<TubeController, TubeController>>();

    #region Unity Method
    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        _gameManager = GameManager.Instance;

        ActionEvent.OnResetGamePlay += Reset;

        ActionEvent.OnUseBoosterRevoke += UseRevokeBall;

        ActionEvent.OnUserBoosterAdd += UseAddTube;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();

        InitScreen();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        InitScreen();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Reset();
        }
#endif
    }

    public void Reset()
    {
        foreach (var tube in _tubes)
        {
            SimplePool.Despawn(tube.gameObject);
        }
        _tubes.Clear();

        _prevTube.Clear();

        _hodingTube = null;

        _cdAddTube = 0;

        Init();

        InitScreen();
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= Reset;

        ActionEvent.OnUseBoosterRevoke -= UseRevokeBall;

        ActionEvent.OnUserBoosterAdd -= UseAddTube;
    }
    #endregion

    private void Init()
    {
        int tubeNumber = _gameManager.Level.tube;
        int slotTube = _gameManager.Level.tubeSlot;
        int index = 0;
        if (tubeNumber > _tubeHorizonlMax)
        {
            int top = (int)Mathf.Ceil(tubeNumber / 2f);
            int bot = (int)Mathf.Floor(tubeNumber / 2f);
            //  Debug.Log($"top: {top}---- bot {bot}");
            _spaceHorizontal = (_tubeHorizonlMax / top) * getDistance(top);
            //   Debug.Log($"Space Horizontal: {_spaceHorizontal}");

            for (int i = 0; i < top; i++)
            {
                SpwanTube(i, 0f, slotTube, getBallDatas());
            }

            float _spaccBot = tubeNumber % 2 == 0 ? 0f : 0.5f;
            // Debug.Log($"Space Bot: {_spaccBot}");

            //*****NEED TO IMPROVE******//
            for (int i = 0; i < bot; i++)
            {
                SpwanTube(i + _spaccBot, -_spaceVertical, slotTube, getBallDatas());
            }
        }
        else
        {
            _spaceHorizontal = (_tubeHorizonlMax / tubeNumber) * getDistance(tubeNumber);
            // Debug.Log($"Space Horizontal: {_spaceHorizontal}");
            for (int i = 0; i < tubeNumber; i++)
            {
                SpwanTube(i, 0f, slotTube, getBallDatas());
            }
        }

        List<BallData> getBallDatas()
        {
            List<BallData> dataBalls = new List<BallData>();
            for (int j = 0; j < slotTube; j++)
            {
                if (index >= _gameManager.Level.data.Count)
                {
                    return dataBalls;
                }
                if (_gameManager.Level.data[index] < 0)
                {
                    index++;
                    continue;
                }
                BallData data = _gameManager.getBallDataSO().getBallData(_gameManager.Level.data[index]);
                dataBalls.Add(data);
                index++;
            }
            return dataBalls;
        }
    }

    private float getDistance(int value)
    {
        if (value <= 2)
        {
            return 1f;
        }
        else if (value > 2 && value <= 5)
        {
            return 0.5f;
        }
        else if (value > 5 && value <= 6)
        {
            return 0.3f;
        }
        return 0.2f;
    }

    private void SpwanTube(float x, float y, int value, List<BallData> ballData)
    {
        GameObject tubeObj = SimplePool.Spawn(_gameManager.getTube(), Vector2.zero, Quaternion.identity);

        tubeObj.transform.SetParent(this.transform);

        TubeController tube = tubeObj.GetComponent<TubeController>();

        TubeData tubeData = new TubeData(_gameManager.Level.tubeSlot, ballData);

        Vector2 target = new Vector2(x * (tube.Width + _spaceHorizontal), y);

        tube.Init(target, tubeData, value);

        _tubes.Add(tube);
    }

    private void SortTube(int tubeNumber)
    {
        Vector2 target;
        if (tubeNumber > _tubeHorizonlMax)
        {
            int top = (int)Mathf.Ceil(tubeNumber / 2f);
            _spaceHorizontal = (_tubeHorizonlMax / top) * getDistance(top);
            int i, j;
            for (i = 0; i < top; i++)
            {
                target = new Vector2(i * (_tubes[i].Width + _spaceHorizontal), 0f);
                _tubes[i].SetPosition(target);
            }

            float _spaccBot = tubeNumber % 2 == 0 ? 0f : 0.5f;
            for (j = i; j < tubeNumber; j++)
            {
                target = new Vector2((j - i + _spaccBot) * (_tubes[j].Width + _spaceHorizontal), -_spaceVertical);
                _tubes[j].SetPosition(target);
            }
        }
        else
        {
            _spaceHorizontal = (_tubeHorizonlMax / tubeNumber) * getDistance(tubeNumber);
            for (int i = 0; i < tubeNumber; i++)
            {
                target = new Vector2(i * (_tubes[i].Width + _spaceHorizontal), 0f);
                _tubes[i].SetPosition(target);
            }
        }
    }

    private void InitScreen()
    {
        // Đặt tỷ lệ khung hình của camera.
        var (center, size) = CalculateOrthoSize();
        _camera.transform.position = center;
        _camera.orthographicSize = size;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();
        foreach (var col in FindObjectsOfType<BoxCollider2D>())
        {
            bounds.Encapsulate(col.bounds);
        }
        bounds.Expand(1f);
        var vertical = bounds.size.y /** _camera.pixelWidth / _camera.pixelHeight*/;
        var horizontal = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth;
        var size = Mathf.Clamp(Mathf.Max(horizontal, vertical) * 0.5f, _minCamSize, _maxCamSize);
        var center = bounds.center + new Vector3(0, 0, -10);

        //  Debug.Log($"Center: {center}--- size:{size}");
        return (center, size);
    }

    private void AddPrevTube(TubeController from, TubeController to)
    {
        _prevTube.Add(new KeyValuePair<TubeController, TubeController>(from, to));
    }

    public void OnClick(TubeController newTube)
    {
        if (!_gameManager.StateGameController.State.Equals(StateGame.Playing)) return;

        if (_hodingTube == null)
        {
            if (newTube.isTubeEmty()) return;
            // Debug.Log(newTube.name);
            newTube.ChangeState(StateTube.Active);
            _hodingTube = newTube;
            newTube.GetLastBall().StartMove(newTube, true);
        }
        else
        {
            if (_hodingTube.Equals(newTube)) // cành đang giữ == cành mới 
            {
                //   Debug.Log(newTube.name);
                newTube.ChangeState(StateTube.Active);
                newTube.GetLastBall().StartMove(newTube, false, newTube.Balls.Count - 1);
                _hodingTube = null;
            }
            else
            {
                if (!newTube.CanSortBall(_hodingTube))
                {
                    _hodingTube.ChangeState(StateTube.Deactive);
                    newTube.ChangeState(StateTube.Active);
                    _hodingTube.GetLastBall().StartMove(_hodingTube, false, _hodingTube.Balls.Count - 1);
                    newTube.GetLastBall().StartMove(newTube, true);
                    _hodingTube = newTube;
                    // Debug.LogError("Can't Sort");
                }
                else
                {
                    Debug.Log("Can Sort");
                    newTube.ChangeState(StateTube.Moving);
                    SortBall(_hodingTube, newTube, OnMoveComplete);
                    _hodingTube = null;

                    /*if (_hodingTube.State.Equals(StateTube.Complete))
                    {
                        _hodingTube.ChangeState(StateTube.Emty);
                        newTube.Complete();
                        _hodingTube = null;
                        return;
                    }


                    if (newTube.isDone())
                    {
                        newTube.Complete();
                        VibrationManager.Vibrate(10);
                        SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("complete1"));
                        Debug.LogError($"{newTube.name} is done");
                        if (ConditionWin())
                        {
                            Debug.Log("you win");
                            _gameManager.Win();
                        }
                    }*/
                }
            }
        }
        void OnMoveComplete()
        {
            newTube.ChangeState(StateTube.Deactive);
            if (newTube.isDone())
            {
                newTube.PlayVfx();
                VibrationManager.Vibrate(15);
                SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("complete1"));
                // Debug.LogError($"{newTube.name} is done");
                if (ConditionWin())
                {
                    // Debug.Log("you win");
                    _gameManager.Win();
                }
            }
        }
    }

    private void SortBall(TubeController from, TubeController to, Action callBack)
    {
        BallController holdingBall = from.GetLastBall();
        List<BallController> moveBalls = new List<BallController>();
        List<BallController> holdBalls = new List<BallController>(from.Balls);
        int max = 0;
        // float t = 0;

        for (int i = holdBalls.Count - 1; i >= 0; i--)
        {
            if (!to.isTubeEmty() && holdingBall.Id != to.GetLastBall().Id)
            {
                moveBalls.Add(to.Balls[i]);

                from.Balls.RemoveAt(i);

                for (i = holdBalls.Count - 1; i >= 0; i--)
                {
                    if (holdBalls[i].Id != holdingBall.Id) break;

                    // Do some thing
                    max++;
                }
                break;
            }
            if (holdingBall.Id == holdBalls[i].Id)
            {
                if (to.Balls.Count + moveBalls.Count >= to.Slot)
                {
                    for (int j = holdBalls.Count - 1 - max; j >= 0; j--)
                    {
                        if (holdBalls[j].Id != holdingBall.Id) break;
                        //Do some thing
                        // t += 0.1f;
                    }
                    //Do some thing
                    break;
                }
                moveBalls.Add(from.Balls[i]);

                from.Balls.RemoveAt(i);

                max++;
            }
            else break;
        }

        int countBall = to.Balls.Count;

        for (int i = 0; i < moveBalls.Count; i++)
        {
            if (i == moveBalls.Count - 1)
            {
                moveBalls[i].Movement(from, to, countBall, i, callBack);   //move -> new branch
            }
            else
            {
                moveBalls[i].Movement(from, to, countBall, i, null);   //move -> new branch
            }

            to.Balls.Add(moveBalls[i]);
            AddPrevTube(from, to);
        }
    }

    private bool ConditionWin()
    {
        for (int i = 0; i < _tubes.Count; i++)
        {
            if (_tubes[i].isTubeEmty()) continue;
            if (!_tubes[i].isDone()) return false;
        }
        return true;
    }

    #region Booster
    private void UseRevokeBall()
    {
        if (_prevTube.Count > 0)
        {
            Debug.Log("Use Revoke");
            TubeController from = _prevTube[_prevTube.Count - 1].Key;
            TubeController to = _prevTube[_prevTube.Count - 1].Value;
            if (_hodingTube != null)
            {
                if (to != _hodingTube)
                    _hodingTube.GetLastBall()?.StartMove(_hodingTube, false, _hodingTube.Balls.Count - 1);
                _hodingTube = null;
            }

            int countBall = from.Balls.Count;
            List<BallController> moveBalls = new List<BallController>();
            int cd = 1;
            for (int i = _prevTube.Count - 1; i >= 0; i--)
            {
                TubeController first = _prevTube[i].Value;
                TubeController second = _prevTube[i].Key;
                moveBalls.Add(first.GetLastBall());
                second.Balls.Add(first.GetLastBall());
                first.Balls.Remove(first.GetLastBall());
                cd++;
                if (_prevTube.Count == 1 || first.Balls.Count <= 0) break;
                if (first != _prevTube[i - 1].Value || second != _prevTube[i - 1].Key)
                {
                    break;
                }
            }

            from.ChangeState(StateTube.Active);
            to.ChangeState(StateTube.Moving);
            for (int i = 0; i < moveBalls.Count; i++)
            {
                if (i == moveBalls.Count - 1)
                {
                    moveBalls[i].Movement(to, from, countBall, i, () =>
                    {
                        from.ChangeState(StateTube.Deactive);
                        to.ChangeState(StateTube.Deactive);
                    });
                }
                else
                {
                    moveBalls[i].Movement(to, from, countBall, i, null);
                }

                if (_prevTube.Count > 0)
                {
                    _prevTube.RemoveAt(_prevTube.Count - 1);
                }
            }

            PlayerData.UserData.UpdateValueBooster(TypeBooster.Revoke, -1);
        }
        else
        {
            Debug.Log("Khong co luot de su dung booster");
        }
    }

    private void UseAddTube()
    {
        int slotTube = _gameManager.Level.tubeSlot;
        // Debug.Log(_cdAddTube);
        if (_cdAddTube < slotTube)
        {
            if (_tubes.Count <= _gameManager.Level.tube)
            {
                SpwanTube(0f, 0f, 1, new List<BallData>());
                SortTube(_tubes.Count);
                Invoke(nameof(InitScreen), 0.02f);
            }
            else
            {
                //  Debug.Log("Update slot to new tube");
                _tubes[_tubes.Count - 1].UpdadeTubeBonus();
            }
            _cdAddTube++;

            PlayerData.UserData.UpdateValueBooster(TypeBooster.AddTube, -1);
        }
        else
        {
            Debug.Log("Ban da them du so luong ong cho phep");
        }
    }
    #endregion
}
