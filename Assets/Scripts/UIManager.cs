using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject MainOptions;
    public GameObject UnitList;
    public GameObject TurretList;
    public GameObject TacticList;
    public EventSystem eventSystem;
    GameObject LastClickedButton;
    public GameObject Scene;
    public GameObject[] Buttons; //0 = Unit 1 = Turret
    public GameObject StartMenuButtons;
    public GameObject SelectLength;
    bool gameStart;
    public GameObject StartMenu;
    public GameObject GameHolder;
    public GameObject InGameMenu;
      public GameObject ResumeB;
    public Image BlackPanelFade;
    [SerializeField] Texture2D cursor;
    
    
    void Awake()
    {
        gameStart = false;
        Cursor.SetCursor(cursor, Vector2.zero,CursorMode.Auto);
    }
    
    public void Update()
    {
        if(!gameStart)
        {
            if(Input.GetMouseButtonDown(1))
            {GoBackToStartGameMenu();}
        }
        else if(!MainOptions.activeInHierarchy & gameStart)
        {
            if(Input.GetMouseButtonDown(1))
            {Leave();}
        }
    }
    
    //StartMenu 

    public void StartGame()
    {
        StartMenuButtons.SetActive(false);
        SelectLength.SetActive(true);
    }

    public void GoBackToStartGameMenu()
    {
        StartMenuButtons.SetActive(true);
        SelectLength.SetActive(false);
    }

    public void Medium()
    {
       // LengthManage
       StartMenu.SetActive(false);
       GameHolder.SetActive(true);
       BlackPanelFade.DOFade(0,1f);
    }

    //InGame

    public void OpenUnitList()
    {
        MainOptions.SetActive(false);
        TurretList.SetActive(false);
        UnitList.SetActive(true);
        TacticList.SetActive(false);
        InGameMenu.SetActive(false);
        LastClickedButton = Buttons[0];
    }

    public void OpenTurretList()
    {
        MainOptions.SetActive(false);
        TurretList.SetActive(true);
        UnitList.SetActive(false);
        TacticList.SetActive(false);
        InGameMenu.SetActive(false);
        LastClickedButton = Buttons[1];
    }

    public void OpenTactics()
    {
        MainOptions.SetActive(false);
        TurretList.SetActive(false);
        UnitList.SetActive(false);
        TacticList.SetActive(true);
        InGameMenu.SetActive(false);
        LastClickedButton = Buttons[2];
    }

    public void OpenInGameMenu()
    {
        MainOptions.SetActive(true);
        TurretList.SetActive(false);
        UnitList.SetActive(false);
        TacticList.SetActive(false);
        InGameMenu.SetActive(true);
        Time.timeScale = 0;
        eventSystem.SetSelectedGameObject(ResumeB);
        LastClickedButton = Buttons[3];
    }

    public void Resume()
    {
        Time.timeScale = 1;
        BlackPanelFade.DOFade(0f,.2f);
        InGameMenu.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
    
    public void Leave()
    {  
        MainOptions.SetActive(true);
        TurretList.SetActive(false);
        UnitList.SetActive(false);
        TacticList.SetActive(false);
        eventSystem.SetSelectedGameObject(LastClickedButton);
    }

    public void SceneReload()
    {   Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

}
