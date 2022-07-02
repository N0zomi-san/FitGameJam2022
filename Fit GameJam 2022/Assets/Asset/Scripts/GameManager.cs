using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _mainManage;
 
    public static GameManager mainManage
    {
        get
        {
            if(_mainManage == null)
            {
                GameObject newOne = new GameObject("GameManager");
                newOne.AddComponent<GameManager>();
            }
 
            return _mainManage;
        }
    }
    
    #region Character & Animation Management
    SpriteRenderer mainCharacter; // character sprite on the left side
    Animator mainCharacterAnimator; // animator of the character sprite
    public Transform handTransform; // transform of hand's icon with offset

    public Camera mainCam; // Main Camera in scene

    #endregion

    #region  UI & Texts Management

    //public Text playedPaper_Text; // text to show how many paper player has played
    //public Text playedNakApproved_Text; // text show how many nak's paper player has approved
    //public Text falseDecision_Text; // Text show how many times player has made wrong decision on paper

    #endregion

    #region Player's Score Management
    [HideInInspector]
    public int playerScore = 0; // +1 when player make the right choice
    [HideInInspector]
    public int playedPaper = 0; // +1 when player make a choice with a paper
    [HideInInspector]
    public int playedNakApproved = 0; // +1 when player approve a Nak's paper

    #endregion

    #region PaperSpawn Management
    [SerializeField] Transform paperSpawnPos; // Paper's spawn transform
    [HideInInspector]
    public bool thereIsPaper = false; // check if paper have approve or deny the paper yet?
    [HideInInspector]
    public int currentAmountPaper = 0; // How many normal papers have spawned
    [HideInInspector]
    public int currentAmountNaksPaper = 0; // How many Nak's papers have spawned
    public int maxPaper; // amount of paper untill end game
    public int maxNakPaper; // amount of paper to approved in game (effect to player's point)
    [SerializeField] List<GameObject>PaperList; // list of paper's prefabs
    [SerializeField] List<GameObject>Paper_NakList; // list of Naks's paper prefabs

    #endregion

    #region GamePlay Management
    public GameObject approvedMark; // Approved Mark's prefab
    public GameObject deniedMark; // Deny Mark's prefab
    RaycastHit2D hit; // hit for player interaction with mouse
    int randomRate; // random result of paper's randoming

    bool gameEnded = false;
    
    #endregion

    void Aiming(){ // raycast from screen to sprites
        hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null){return;}
        if (hit.collider.tag == "rightSide" || hit.collider.tag == "Paper"){
            ArmFollow();
            if (Input.GetMouseButtonDown(0) && hit.collider.tag == "Paper"){
                Approved(hit);
                Debug.Log("Paper Interacted!");
            }else if(Input.GetMouseButtonDown(1) && hit.collider.tag == "Paper"){
                Denied(hit);
                Debug.Log("Paper Interacted!");
            }
        }
    }

    void ArmFollow(){ // make the hand icon follow cursor
        Vector3 target = new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, mainCam.ScreenToWorldPoint(Input.mousePosition).y, -2f);
            handTransform.transform.position = target;
    }

    void Approved(RaycastHit2D interacting){ // Call paper's approved function
        Paper currentPaper = interacting.collider.GetComponent<Paper>();
        currentPaper.IamApproved();
        currentPaper.interacted = true;
    }

    void Denied(RaycastHit2D interacting){ // Call paper's denied function
        Paper currentPaper = interacting.collider.GetComponent<Paper>();
        currentPaper.IamDenied();
        currentPaper.interacted = true;
    }

    void GeneratePaper(){ // Instantiate next paper
        if(!thereIsPaper && currentAmountPaper != maxPaper){
            randomPaper();
            if (randomRate == 0 && currentAmountNaksPaper != maxNakPaper){
                Instantiate(Paper_NakList[Random.Range(0,Paper_NakList.Count)], paperSpawnPos.position, Quaternion.identity);
                currentAmountNaksPaper += 1;
                Debug.Log("Paper Generated!");
            }else{
                Instantiate(PaperList[Random.Range(0,PaperList.Count)], paperSpawnPos.position, Quaternion.identity);
                
                Debug.Log("Paper Generated!");
            }
        }
    }

    void randomPaper(){ // random if next paper is either Nak's or not
            if (20 <= Random.Range(0f, 100.0f)){
                randomRate = 0;
            }else{
                randomRate = 1;
            }
    }

    void CheckBFstart(){ // check the script's component before start

    }

    void CheckGamePlay(){
        Debug.Log("Score : " + playerScore);
        Debug.Log("Played Paper : " + playedPaper);
        Debug.Log("There is paper ? : " + thereIsPaper);
        Debug.Log("Nak paper : " + currentAmountNaksPaper);
        Debug.Log("paper amount : " + currentAmountPaper);
    }

    void CheckEndGame(){
        if (currentAmountPaper == maxPaper){
            gameEnded = true;
            Debug.Log("The game has end");
        }
    }

    private void Awake() {
        _mainManage = this;
    }

    void Start()
    {
         CheckBFstart();
    }

    
    void Update()
    {
        CheckEndGame();
        CheckGamePlay(); // Debug to check state of numbers and gameplay
        Aiming();
        if(!gameEnded){GeneratePaper();}else{
        }

    }
}
