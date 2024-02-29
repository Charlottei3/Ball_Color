using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("~~~DATA~~~")]
    public List<BallDataSO> BallDatasSO;
    public TubeDataSO TubeDataSO;
    public BgDataSO BgDataSo;
    [field: SerializeField] public Level Level { get; private set; }
    public DataManager Datamanager { get; private set; }
    private UserData _userData;
    [Header("REFFERENCE")]
    [SerializeField] GamePlayManager _gamePlayManager;
    public GamePlayManager GamePlayManager => _gamePlayManager;
    public StateGameController StateGameController;
    [Header("Tool")]
    [SerializeField] int _level;
    [SerializeField] int _testIdTube, _testIdBall, _idBg;
    #region Unity Method
    public override void Awake()
    {
        Datamanager = DataManager.Instance;
        _userData = PlayerData.UserData;

        ActionEvent.OnResetGamePlay += InitLevel;

        InitLevel();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerData.UserData.HighestLevel = _level;
            ActionEvent.OnResetGamePlay?.Invoke();
        }
#endif
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= InitLevel;
    }
    #endregion

    #region Public Method
    public void InitLevel()
    {
        //  Debug.Log($"So luong level:{Datamanager.LevelDataSO.getListLevel()}");
        //Debug.LogError($"Current Highest Level: {_userData.HighestLevel}");
        StateGameController.Playing();
        this.Level = Datamanager.LevelDataSO.getLevel(_userData.HighestLevel);
    }

    public BallDataSO getBallDataSO()
    {
        return BallDatasSO[_testIdBall];
    }

    public GameObject getTube()
    {
        return TubeDataSO.getTube(_testIdTube);
    }

    public Sprite getBg()
    {
        return BgDataSo.getBG(_idBg);
    }

    public void Win()
    {
        StateGameController.Win();
        _userData.UpdateHighestLevel();
        SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("victory1"));
        FunctionCommon.DelayTime(0.5f, () => { PopupWin.Instance.Show(); });
    }

    public void OnClickSetting()
    {
        PopupSetting.Instance.Show();
    }

    public void OnClickReplay()
    {
        ActionEvent.OnResetGamePlay?.Invoke();
    }
    #endregion
}
