using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    #region Character Management
    SpriteRenderer mainCharacter;
    Animator mainCharacterAnimator;
    [SerializeField] Transform handTransform;

    #endregion

    #region GamePlay Management
    int currentScore; // +1 when player make the right choice
    int currentPaper; // +1 when player make a choice with a paper
    int currentNakApproved; // +1 when player approve a Nak's paper

    
    public Camera mainCam;
    RaycastHit2D hit; // hit for player interaction with mouse
    


    #endregion

    void AimingRIghtSide(){
        hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null){return;}
        if (hit.collider.tag == "rightSide"){
            ArmFollow();
        }
    }

    void ArmFollow(){
        Vector3 target = new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, mainCam.ScreenToWorldPoint(Input.mousePosition).y, -2f);
            handTransform.transform.position = target;
    }

    void Approved(){

    }

    void Denied(){

    }

    void SpawnPaper(){

    }

    void Start()
    {
         
    }

    
    void Update()
    {
        AimingRIghtSide();
    }
}
