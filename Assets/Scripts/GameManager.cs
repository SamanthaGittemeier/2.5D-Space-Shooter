using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    [SerializeField]
    private Player _playerGM;

    [SerializeField]
    private Animator _fuelBarAnim;

    [SerializeField]
    private AnimatorClipInfo[] _currentClipInfo;

    [SerializeField]
    private AnimatorStateInfo _currentAnimStateInfo;

    [SerializeField]
    private string _currentClipName;

    [SerializeField]
    private float _currentClipTime;
    [SerializeField]
    private float _currentClipLength;
    [SerializeField]
    private float _canSprint = -1f; //can fire
    [SerializeField]
    private float _sprintTime; //fire rate or delay

    // Start is called before the first frame update
    void Start()
    {
        _playerGM = GameObject.Find("Player").GetComponent<Player>();
        _fuelBarAnim = GameObject.Find("Fuel Bar").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            Debug.Log("R Key Pressed");
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        _currentAnimStateInfo = _fuelBarAnim.GetCurrentAnimatorStateInfo(0);
        _currentClipInfo = _fuelBarAnim.GetCurrentAnimatorClipInfo(0);
        _currentClipName = _currentClipInfo[0].clip.name;
        _currentClipLength = _currentClipInfo[0].clip.length;
        //_currentClipTime = _currentClipInfo[0].clip.length * _currentAnimStateInfo.normalizedTime;

        if (_currentClipTime == 10)
        {
            _playerGM.ResetSpeed();
            _fuelBarAnim.SetBool("ShiftDown", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _currentClipName == "FuelBarFull_A" || Input.GetKey(KeyCode.RightShift))
        {
            //_currentClipTime = _currentClipInfo[0].clip.length * _currentAnimStateInfo.normalizedTime;
            _playerGM.Sprint();
            _fuelBarAnim.SetBool("ShiftDown", true);
            //if (_fuelBarAnim.GetBool("ShiftDown") == true)
            //{
            //    _currentClipTime = _currentClipInfo[0].clip.length * _currentAnimStateInfo.normalizedTime;
            //    if (_currentClipTime == 10)
            //    {
            //        _playerGM.ResetSpeed();
            //        _fuelBarAnim.SetBool("ShiftDown", false);
            //    }
            //}
            //while (_fuelBarAnim.GetBool("ShiftDown") == true)
            //{
            //    _currentClipTime = _currentClipInfo[0].clip.length * _currentAnimStateInfo.normalizedTime;
            //    if (_currentClipTime >= 10)
            //    {
            //        _playerGM.ResetSpeed();
            //        _fuelBarAnim.SetBool("ShiftDown", false);
            //    }
            //}
            //StartCoroutine(OutOfFuel());
            //_sprintTime = _currentClipLength;
            //_canSprint = Time.time + _sprintTime;
            //if (Time.time < _canSprint)
            //{
            //    _playerGM.ResetSpeed();
            //    _fuelBarAnim.SetBool("ShiftDown", false);
            //    //_fuelBarAnim.Play("FuelBarRefilling");
            //}
            //if (_currentClipTime >= 10f)
            //{
            //    _playerGM.ResetSpeed();
            //    _fuelBarAnim.SetBool("ShiftDown", false);
            //}
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _currentClipName == "FuelBarDepleting_A" || Input.GetKeyUp(KeyCode.RightShift))
        {
            _playerGM.ResetSpeed();
            _fuelBarAnim.SetBool("ShiftDown", false);
            //_fuelBarAnim.Play("FuelBarRefilling");
        }
    }

    //private IEnumerator OutOfFuel()
    //{
    //    while (_fuelBarAnim.GetBool("ShiftDown") == true)
    //    {
    //        _currentClipTime = _currentClipInfo[0].clip.length * _currentAnimStateInfo.normalizedTime;
    //        if (_currentClipTime >= 10)
    //        {
    //            _playerGM.ResetSpeed();
    //            _fuelBarAnim.SetBool("ShiftDown", false);
    //        }
    //    }
    //}

    public void GameOver()
    {
        _isGameOver = true;
    }
}
