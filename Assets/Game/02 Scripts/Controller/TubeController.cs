using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TubeData
{
    public int Slot;
    public List<BallData> dataBall = new List<BallData>();

    public TubeData(int slot, List<BallData> dataBall)
    {
        Slot = slot;
        this.dataBall = dataBall;
    }
}

public class TubeController : MonoBehaviour
{
    public TubeData data;
    [Header("REFFERENCE")]
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] SpriteRenderer[] _sprList;
    [SerializeField] SpriteRenderer _slot1Spr;
    [SerializeField] BoxCollider2D _boxCol2D;
    [SerializeField] Transform _spwanTrans;
    [SerializeField] ParticleSystem _vfxComplete;
    [Space(10)]
    [Header("VALUE")]
    [SerializeField] float _width;
    [SerializeField] float _height, _heightMid;
    private int _slot;
    public int Slot => _slot;
    public float Width => _width;
    public float Height => _height;
    private Vector2 _spwanPos, _startPosMove;
    public Vector2 SpawnPos => _spwanPos;
    public Vector2 StartPosMove => _startPosMove;
    public List<BallController> Balls = new List<BallController>();
    [field: SerializeField] public StateTube State { get; private set; }


    private void Awake()
    {
        _width = _sprList[0].bounds.size.x;
        _heightMid = _sprList[1].bounds.size.y;

        _boxCol2D = this.GetComponent<BoxCollider2D>();

        //  Debug.LogError($"Width Tube: {_width}--- Height Tube: {_heightMid}");
    }

    private void OnEnable()
    {

    }

    private void Reset()
    {
        this.data = null;
        _boxCol2D.size = Vector2.zero;
        ChangeState(StateTube.Deactive);
        if (Balls.Count < 1) return;
        foreach (var ball in Balls)
        {
            SimplePool.Despawn(ball.gameObject);
        }
        Balls.Clear();
        // vfx false
        _vfxComplete.Stop();
    }

    private void OnDisable()
    {
        Reset();
    }

    private void OnMouseDown()
    {
        if (PopupManager.Instance.hasPopupShowing) return;
        if (State.Equals(StateTube.Active) || State.Equals(StateTube.Moving)) return;
        GameManager.Instance.GamePlayManager.OnClick(this);
    }

    public void Init(Vector2 target, TubeData data, int slot)
    {
        this.data = data;
        _slot = slot;

        SetPosition(target);

        DisplayTube();

        for (int i = 0; i < data.dataBall.Count; i++)
        {
            SpwanBall(data.dataBall[i], i);
        }
    }

    private void DisplayTube()
    {
        _height = 0;
        if (_slot > 1)
        {
            int tmp = _slot - 2;
            _sprList[1].size = new Vector2(_sprList[1].size.x, _heightMid * tmp);
            //  Debug.Log("Size tube mid= " + _sprList[1].size);
            //  Debug.Log($"spwan pos: {_spwanPos}");
            float test = _sprList[1].transform.position.y + _sprList[1].size.y + _sprList[2].size.y / 2;
            _sprList[2].transform.position = new Vector2(_sprList[2].transform.position.x, test);
            foreach (var spr in _sprList)
            {
                _height += spr.bounds.size.y;
                spr.gameObject.SetActive(true);
            }
            //  Debug.LogError("Height tube = " + _height);
            _slot1Spr.gameObject.SetActive(false);
            _boxCol2D.size = new Vector2(_width, _height);
            _boxCol2D.offset = new Vector2(0, _boxCol2D.size.y / 2 - 0.67f);
            //  _boxCol2D.offset = new Vector2(0, _height / 2);
        }
        else
        {
            foreach (var spr in _sprList)
            {
                spr.gameObject.SetActive(false);
            }
            _slot1Spr.gameObject.SetActive(true);
            _height = _sprList[0].size.y + 0.67f;
            _boxCol2D.size = new Vector2(_width, _height);
            _boxCol2D.offset = new Vector2(0, _height / 2 - 0.67f);
        }
        _startPosMove = new Vector2(this.transform.position.x, this.transform.position.y + _height);
    }

    public void UpdadeTubeBonus()
    {
        _slot++;
        DisplayTube();
    }

    public void SetPosition(Vector2 target)
    {
        this.transform.position = target;

        _spwanPos = _spwanTrans.position;

        _startPosMove = new Vector2(this.transform.position.x, this.transform.position.y + _height);
    }

    private void SpwanBall(BallData data, int index)
    {
        GameObject ballObj = SimplePool.Spawn(_ballPrefab, Vector2.zero, Quaternion.identity);
        ballObj.transform.SetParent(this.transform);
        BallController ball = ballObj.GetComponent<BallController>();
        ball.Init(data, _spwanPos, _startPosMove, index);
        Balls.Add(ball);
    }

    public void ChangeState(StateTube state)
    {
        this.State = state;
    }

    public void PlayVfx()
    {
        _vfxComplete.Play();
    }

    public bool isTubeEmty()
    {
        if (Balls.Count > 0)
        {
            return false;
        }
        return true;
    }

    public bool CanSortBall(TubeController tube)
    {
        if (isTubeEmty())
        {
            return true;
        }
        if (_slot == Balls.Count) return false;

        if (data.Slot == Balls.Count)
        {
            return false;
        }

        bool isSameColor = GetLastBall().Id == tube.GetLastBall().Id;
        if (!isSameColor)
        {
            return false;
        }
        return true;
    }

    public List<BallController> getListSameBall()
    {
        BallController ball = Balls[Balls.Count - 1];

        List<BallController> sameBalls = new List<BallController>() { ball };

        for (int i = Balls.Count - 2; i >= 0; i--)
        {
            if (Balls[i].Id == ball.Id)
            {
                sameBalls.Add(Balls[i]);
            }
            else
            {
                break;
            }
        }
        return sameBalls;
    }

    public BallController GetLastBall()
    {
        if (!isTubeEmty())
        {
            BallController ball = Balls[Balls.Count - 1];
            return ball;
        }
        return null;
    }

    public bool isDone()
    {
        if (_slot < data.Slot) return false;
        if (Balls.Count != data.Slot) return false;
        for (int i = Balls.Count - 1; i >= 0; i--)
        {
            if (i > 0)
            {
                if (Balls[i].Id != Balls[i - 1].Id)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

public enum StateTube
{
    Active,
    Deactive,
    Moving,
}
