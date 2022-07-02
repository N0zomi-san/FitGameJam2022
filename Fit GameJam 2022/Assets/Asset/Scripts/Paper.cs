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
    public bool interacted = false;
    bool playerMarked;
    GameObject thisMark;

    [SerializeField] Transform naksMarkPos; // Position of Nak's mark on the paper

    public async void IamApproved(){ // Call from GameManager when player Approved this paper
        if(interacted){return;}
        if (isNaksPaper){
            // -1 player's point
            // Update UI's icons and numbers
        }if (isNormalPaper){
            // +1 player's point
            // Update UI's icons and numbers
        }
        thisMark = Instantiate(GameManager.mainManage.approvedMark, transform.position, Quaternion.identity); // Geneate Approved mark
        GameManager.mainManage.playedPaper += 1;
        // animate paper before destroy it
        await Task.Delay(3000); // delay 3 seconds before destroy this paper
        destroyMe();
    }

    public async void IamDenied(){ // Call from GameManager when player Deny this paper
        if(interacted){return;}
        if (isNormalPaper){
            // -1 player's point
            // Update UI's icons and numbers
        }if (isNaksPaper){
            // +1 player's point
            // Update UI's icons and numbers
        }
        thisMark = Instantiate(GameManager.mainManage.deniedMark, transform.position, Quaternion.identity); // Generate Denied mark
        GameManager.mainManage.playedPaper += 1;
        // animate paper before destroy it
        await Task.Delay(3000); // delay 3 seconds before destroy this paper
        destroyMe();
    }

    void destroyMe(){
        GameManager.mainManage.thereIsPaper = false;
        Debug.Log("Paper Destroyed!");
        Destroy(gameObject);
        Destroy(thisMark);
    }

    async void waitForAnimation(string animationsCode){
        // make function to compleate paper's animation before destroy it
    }

    void starterAnimate(){ // Animate when paper is spawned

    }

    void enderAnimate(){ // Animate when paper is about to destroyed

    }

    void Start()
    {
        thisSprite = GetComponent<SpriteRenderer>();
        GameManager.mainManage.currentAmountPaper += 1;
        GameManager.mainManage.thereIsPaper = true;
        paperAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }
}
