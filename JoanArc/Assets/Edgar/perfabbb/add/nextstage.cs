using Edgar.Unity.Examples;
using Edgar.Unity.Examples.Metroidvania;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class nextstage : MonoBehaviour
{
    public static nextstage Instance;
    public int index;
    public bool cango;


    public MetroidvaniaGameManager aim;


    
    [Header("³¡¾°¿ØÖÆ")]
    public GameObject NormalScene;
    public GameObject FubenScene;
    public GameObject Bossroom;

    public GameObject leaveButton;

    public Transform Player;
    public GameObject birrhpoint;

    public int Aimlayer;
    private void Awake()
    {
        Instance = this;
    }



    void Start()
    {
        index = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputHelper.GetKeyDown(KeyCode.Return) && cango == true )
        {
            aim.loadtime = Aimlayer;

            aim.LoadNextLevel();
            cango = false;

            leaveButton.SetActive(true);

            NormalScene.SetActive(false);
            Invoke("SeekFuben", 1f);

            switch(aim.loadtime)
            {
                case 1:
                    AudioManager.instance.fubenmusic();
                    break;
                case 2:
                    AudioManager.instance.fubenmusic();
                    break;

                case 3:
                    AudioManager.instance.BossbattleMusic();
                    break;
            }
        }


    }

    private void SeekFuben()
    {
        FubenScene = GameObject.Find("Generated Level");
        Bossroom = GameObject.Find("Boss(Clone)");
    }

    public void ReturnToNormal()
    {
        leaveButton.SetActive(false);
        NormalScene.SetActive (true);

        if(FubenScene!=null)
         Destroy(FubenScene);
        if (Bossroom != null)
            Destroy(Bossroom);

        Player.position = birrhpoint.transform.position;
        aim.loadtime = 0;

        AudioManager.instance.NOramlCitymusic();
    }
}
