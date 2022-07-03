using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Paper : MonoBehaviour
{
    SpriteRenderer thisSprite;
    Animator paperAnimator;
    [Tooltip("Check if this is a normal paper")]
    bool isNormalPaper;
    [Tooltip("Check if this is a Nak's paper")]
    [SerializeField] bool isNaksPaper; // If this paper is belong to Nak
    [HideInInspector]
    public bool interacted = false; // Check if player has make decision on this paper
    GameObject thisMark; // Contain mark from the player when instantiate
    [HideInInspector]
    public Vector3 markPos; // Contain MousePosition from raycast to instantiate the mark on the paper
    [HideInInspector]
    public bool animatingState; // True if the paper is still in animationing

    [SerializeField] Transform naksMarkPos; // Position of Nak's mark on the paper

    public async void IamApproved(){ // Call from GameManager when player Approved this paper
        if(interacted){return;}
        if (isNaksPaper){
            GameManager.mainManage.playerScore -= 1; // PlayerScore change
            GameManager.mainManage.falseDecision += 1; // Player made the wrong choice
            GameManager.mainManage.approvedNaksPaper += 1; // Player has approved a Nak's paper
            Debug.Log("Is Nak's paper");
        }else{
            GameManager.mainManage.playerScore += 1; // PlayerScore change
            Debug.Log("Is normal paper");
            
        }
        thisMark = Instantiate(GameManager.mainManage.approvedMark, markPos, Quaternion.identity); // Geneate Approved mark
        thisMark.transform.SetParent(transform,true);

        GameManager.mainManage.playedPaper += 1;
        paperAnimator.SetTrigger("Ended");
        await Task.Delay(2000); // delay 3 seconds before destroy this paper
        destroyMe();
    }

    public async void IamDenied(){ // Call from GameManager when player Deny this paper
        if(interacted){return;}
        if (isNaksPaper){
            GameManager.mainManage.playerScore += 1; // PlayerScore change
            GameManager.mainManage.deniedNaksPaper += 1; // Player has denied a Nak's paper
            Debug.Log("Is Nak's paper");
        }else{
            GameManager.mainManage.playerScore -= 1; // PlayerScore change
        GameManager.mainManage.falseDecision += 1; // Player made the wrong choice
        Debug.Log("Is normal paper");
        }
        
        thisMark = Instantiate(GameManager.mainManage.deniedMark, markPos, Quaternion.identity); // Generate Denied mark
        thisMark.transform.SetParent(transform,true);

        GameManager.mainManage.playedPaper += 1;
        paperAnimator.SetTrigger("Ended");
        await Task.Delay(2000); // delay 3 seconds before destroy this paper
        destroyMe();
    }

    void destroyMe(){
        GameManager.mainManage.thereIsPaper = false;
        Destroy(gameObject);
        Destroy(thisMark);
    }

    async void Start()
    {
        animatingState = true;
        thisSprite = GetComponent<SpriteRenderer>();
        paperAnimator = GetComponent<Animator>();
        GameManager.mainManage.thereIsPaper = true;
        paperAnimator = GetComponent<Animator>();
        await Task.Delay(3000);
        animatingState = false;
    }

    
    void Update()
    {
        
    }
}
