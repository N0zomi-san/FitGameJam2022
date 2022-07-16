using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _mainManage;
 
    public static GameManager mainManage // Get access to GameManager but can't set
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
    [Header("Animation Setting")]
    [SerializeField] GameObject mainCharacter; // character sprite on the left side
    [HideInInspector]
    public Animator mainCharacterAnimator; // animator of the character sprite
    public Transform handTransform; // transform of hand's icon with offset

    public Camera mainCam; // Main Camera in scene

    [Space(5)]
    #endregion

    #region  UI & Texts Management
    [Header("UI elements & Scene Setting")]
    [Space(3)]
    [Header("Texts")]
    public TMP_Text playedPaper_Text; // text to show how many paper player has played
    public TMP_Text playedNakDenied_Text; // text show how many nak's paper player has approved
    public TMP_Text falseDecision_Text; // Text show how many times player has made wrong decision on paper

    [Space(3)]
    [Header("Canvas Objects")]
    public  GameObject gameOverCanvas; // Canvas of Game over's panel
    public GameObject gameEndedCanvas; // Canvas of Game Ended's Panel
    public GameObject iconCanvas; // Canvas contain Icons & scores
    public SpriteRenderer blackFadingSquare; // Sprite for fading black
    
    [Space(3)]
    [Header("Fading & Slide Setting")]
    bool isCutScene = false;
    [SerializeField] int fadingSeconds; // How long do you want to fade
    [SerializeField] int betweenFading_Show; // How long do you want to wait between fading
    [SerializeField] List<GameObject>BorderList; // List of pictures's border
    [SerializeField] List<GameObject> pictureList01; // First list of manga
    [SerializeField] List<Vector3> PosList01; // List contain of spawn pos of pictures01
    [SerializeField] List<GameObject> pictureList02; // Second list of manga
    [SerializeField] List<Vector3> PosList02; // List contain of spawn pos of pictures02

    void SetCanvas(){ // Setting Canvas before the scene start
        gameOverCanvas.SetActive(false);
        gameEndedCanvas.SetActive(false);
    }

    void OpenCloseCanvas(bool set){ // Close every Canvas
        gameOverCanvas.SetActive(set);
        iconCanvas.SetActive(set);
        
    }
    float elapsedTime; // Time elapsed when lerp started

    IEnumerator BlackFadingIn(int duration){
        elapsedTime = 0;   
        var end = Time.time + duration;
        Color targetColor = blackFadingSquare.color;
        targetColor.a = 0;
        blackFadingSquare.color = targetColor;
        float alpha = targetColor.a;
        while(Time.time < end){
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Lerp(0,1, elapsedTime/duration);
            targetColor.a = alpha;
            blackFadingSquare.color = targetColor;
            //Debug.Log("bf : " + alpha);
            yield return null;
        }
    }

    IEnumerator ShowStart(){
        // Load First Set
        yield return new WaitForSeconds(3);
        GameObject border01 = Instantiate(BorderList[0], transform.position, Quaternion.identity);
        for (int i = 0; i < pictureList01.Count; i++){
            Instantiate(pictureList01[i], PosList01[i], Quaternion.identity).transform.SetParent(border01.transform);
            yield return new WaitForSeconds(3);
        }

        // Destroy First Set
        Destroy(border01);

        // Load Second Set
        GameObject border02 = Instantiate(BorderList[1], transform.position, Quaternion.identity);
        for (int i = 0; i < pictureList02.Count; i++){
            Instantiate(pictureList02[i], PosList02[i], Quaternion.identity).transform.SetParent(border02.transform);
            yield return new WaitForSeconds(3);
        }
        

        // Destroy Second Set
        Destroy(border02);
    }

    IEnumerator BlackFadingOut(int duration){
        elapsedTime = 0;
        var end = Time.time + duration;
        Color targetColor = blackFadingSquare.color;
        targetColor.a = 1;
        blackFadingSquare.color = targetColor;
        float alpha = targetColor.a;
            while(Time.time < end){
                elapsedTime += Time.deltaTime;
                alpha = Mathf.Lerp(1,0, elapsedTime/duration);
                targetColor.a = alpha;
                blackFadingSquare.color = targetColor;
                //Debug.Log("af : " + alpha);
                yield return null;
            }
    }
    IEnumerator StartCutscene(){
        isCutScene = true;
        OpenCloseCanvas(false);
        Color setBlack = blackFadingSquare.color;
        setBlack.a = 1;
        blackFadingSquare.color = setBlack;
        yield return new WaitForSeconds(4);
        StartCoroutine(ShowStart());
        yield return ShowStart();
        OpenCloseCanvas(true);
        SetCanvas();
        StartCoroutine(BlackFadingOut(fadingSeconds)); // fading out
        isCutScene = false;
    }

    public void RetryButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitButton(){
        Application.Quit();
    }

    void GameOver(){
        Cursor.visible = true;
        gameOverCanvas.SetActive(true);
        iconCanvas.SetActive(false);
    }
    void GameEnded(){
        Cursor.visible = true;
        gameEndedCanvas.SetActive(true);
        iconCanvas.SetActive(false);
    }

    [Space(5)]
    #endregion

    #region Player's Score Management
    [HideInInspector]
    public int playerScore = 0; // +1 when player make the right choice
    [HideInInspector]
    public int playedPaper = 0; // // How many papers have spawned
    [HideInInspector]
    public int playedNaksPaper = 0; // How many Nak's paper have spawn

    #endregion

    #region PaperSpawn Management
    [Header("Paper Management")]
    [SerializeField] Transform paperSpawnPos; // Paper's spawn transform
    [HideInInspector]
    public bool thereIsPaper = false; // check if paper have approve or deny the paper yet?
    [HideInInspector]
    public int approvedNaksPaper = 0; // How many Nak's papers player has approved
    [HideInInspector]
    public int deniedNaksPaper = 0; // How many Nak's papers player has denied
    [HideInInspector]
    public int falseDecision = 0; // How many times player has make the wrong decision
    public int maxPaper; // amount of paper untill end game
    [HideInInspector]
    public int paperLeft; // How many papers left for player to play
    public int maxNakPaper; // amount of paper to approved in game (effect to player's point)
    [SerializeField] List<GameObject>PaperList; // list of paper's prefabs
    [SerializeField] List<GameObject>Paper_NakList; // list of Naks's paper prefabs

    [Space(5)]
    #endregion

    #region GamePlay Management
    [Header("GamePlay Setting")]
    public GameObject approvedMark; // Approved Mark's prefab
    public GameObject deniedMark; // Deny Mark's prefab
    RaycastHit2D hit; // hit for player interaction with mouse
    int randomRate; // random result of paper's randoming
    bool gameEnded = false;
    bool gameOvered = false;
    
    #endregion

    void Aiming(){ // raycast from screen to sprites
        hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null){return;}
        if (hit.collider.tag == "rightSide" || hit.collider.tag == "Paper"){
            ArmFollow();
            if (Input.GetMouseButtonDown(0) && hit.collider.tag == "Paper"){
                Approved(hit);
                mainCharacterAnimator.SetTrigger("Mark"); // Trigger character marking animation
            }else if(Input.GetMouseButtonDown(1) && hit.collider.tag == "Paper"){
                Denied(hit);
                mainCharacterAnimator.SetTrigger("Mark"); // Trigger character marking animation
            }
        }
    }

    void ArmFollow(){ // make the hand icon follow cursor
        Vector3 target = new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, mainCam.ScreenToWorldPoint(Input.mousePosition).y, -2f);
            handTransform.transform.position = target;
    }

    void Approved(RaycastHit2D interacting){ // Call paper's approved function
        Paper currentPaper = interacting.collider.GetComponent<Paper>();
        currentPaper.markPos = new Vector3(hit.point.x, hit.point.y, -2f);
        currentPaper.IamApproved();
        currentPaper.interacted = true;
    }

    void Denied(RaycastHit2D interacting){ // Call paper's denied function
        Paper currentPaper = interacting.collider.GetComponent<Paper>();
        currentPaper.markPos = new Vector3(hit.point.x, hit.point.y, -2f);
        currentPaper.IamDenied();
        currentPaper.interacted = true;
    }

    void GeneratePaper(){ // Instantiate next paper
        if(!thereIsPaper && playedPaper != maxPaper){
            randomPaper();
            //await Task.Delay(2000);
            if (randomRate == 0 && playedNaksPaper != maxNakPaper){
                Instantiate(Paper_NakList[Random.Range(0,Paper_NakList.Count)], paperSpawnPos.position, Quaternion.identity);
                playedNaksPaper += 1;
                paperLeft -= 1;
            }
            if(randomRate == 1 && paperLeft <= maxNakPaper-playedNaksPaper){
                Instantiate(Paper_NakList[Random.Range(0,Paper_NakList.Count)], paperSpawnPos.position, Quaternion.identity);
                playedNaksPaper += 1;
                paperLeft -= 1;
            }else if (randomRate == 1){
                Instantiate(PaperList[Random.Range(0,PaperList.Count)], paperSpawnPos.position, Quaternion.identity);
                paperLeft -= 1;
            }
        }
    }

    void randomPaper(){ // random if next paper is either Nak's or not
            if (80 <= Random.Range(0f, 100.0f)){ // 10% to spawn Nak's paper
                randomRate = 0;
            }else{
                randomRate = 1;
            }
    }

    void CheckBFstart(){ // check the script's component before start
        mainCharacterAnimator = mainCharacter.GetComponent<Animator>();
        Cursor.visible = false;
        paperLeft = maxPaper;
    }

    void CheckGamePlay(){
        Debug.Log("Score : " + playerScore);
        Debug.Log("Amount Paper : " + playedPaper);
        Debug.Log("There is paper ? : " + thereIsPaper);
        Debug.Log("Nak paper : " + playedNaksPaper);
        Debug.Log("Approved Nak's paper : " + approvedNaksPaper);
        Debug.Log("Denied Nak's papers : " + deniedNaksPaper);
        Debug.Log("Paper left : " + paperLeft);
        Debug.Log("Nak's paper left : " + (maxNakPaper - playedNaksPaper).ToString());
    }

    IEnumerator CheckEndGame(){
        if (playedPaper == maxPaper){
            yield return new WaitForSeconds(3);
            gameEnded = true;
            Debug.Log("The game has end");
        }
        if (falseDecision == 3){
            yield return new WaitForSeconds(3);
            gameOvered = true;
            Debug.Log("The game has over");
        }
    }

    public void UpdateText(){
        playedPaper_Text.text = playedPaper.ToString();
        playedNakDenied_Text.text = deniedNaksPaper.ToString();
        falseDecision_Text.text = falseDecision.ToString();
    }

    void GamePlay(){
        if(gameOvered){
                GameOver();
                return;
            }
            if(gameEnded){
                GameEnded();
                return;
            }
            Aiming();
            GeneratePaper();
            UpdateText();
    }

    private void Awake() {
        _mainManage = this;
    }

    void Start()
    {
        CheckBFstart();
        SetCanvas();
        StartCoroutine(StartCutscene());
    }

    
    void Update()
    {
        if(!isCutScene){
            StartCoroutine(CheckEndGame());
            //CheckGamePlay();
            if(!isCutScene){
                GamePlay();
            }
        
        }
    }
}
