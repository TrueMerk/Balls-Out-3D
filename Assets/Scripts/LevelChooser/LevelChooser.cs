using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChooser : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons = new List<Button>();
    public int level;
    public int _b;
    public PlayerState playerState;
    [SerializeField] private GameObject _menu;
    [SerializeField] private Level _level;

    private void Start()
    {
        _menu.SetActive(false);
    }

    public void LevelChoose1()
    {
        level = 0;
        PlayerPrefs.SetInt("level", level);
        _b = 0;
        PlayerPrefs.SetInt("orange",_b);
        playerState.Load();
        SceneManager.LoadScene(1);
        _menu.SetActive(false);
    }
    public void LevelChoose2()
    {
        level = 1;
        PlayerPrefs.SetInt("level", level);
        _b = 0;
        PlayerPrefs.SetInt("orange",_b);
        playerState.Load();
        SceneManager.LoadScene(1);
        _menu.SetActive(false);
    }
    
    public void LevelChoose3()
    {
        level = 2;
        PlayerPrefs.SetInt("level", level);
        _b = 1;
        PlayerPrefs.SetInt("orange",_b);
        playerState.Load();
        SceneManager.LoadScene(1);
        _menu.SetActive(false);
       
    }
    public void LevelChoose4()
    {
        level = 40;
        PlayerPrefs.SetInt("level", level);
        _b = 0;
        PlayerPrefs.SetInt("orange",_b);
        playerState.Load();
        SceneManager.LoadScene(1);
        _menu.SetActive(false);
       
    }
}
