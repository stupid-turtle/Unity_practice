using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour{
    
    private int totHealthPoint;
    private int nowHealthPoint;
    private int totMagicPoint;
    private int nowMagicPoint;
    private int attackPoint;
    private int defensePoint;
    public Text hpText;
    public Text mpText;
    public Text attackText;
    public Text defenseText;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start(){
        playerController = new PlayerController();
        totHealthPoint = playerController.totHealthPoint;
        nowHealthPoint = playerController.nowHealthPoint;
        totMagicPoint = playerController.totMagicPoint;
        nowMagicPoint = playerController.nowMagicPoint;
        attackPoint = playerController.attackPoint;
        defensePoint = playerController.defensePoint;
        hpText.text = "HP: " + nowHealthPoint + "/" + totHealthPoint;
        mpText.text = "MP: " + nowMagicPoint + "/" + totMagicPoint;
        attackText.text = "ATK: " + attackPoint;
        defenseText.text = "DIT: " + defensePoint;
    }

    // Update is called once per frame
    void Update() {
        totHealthPoint = playerController.totHealthPoint;
        nowHealthPoint = playerController.nowHealthPoint;
        totMagicPoint = playerController.totMagicPoint;
        nowMagicPoint = playerController.nowMagicPoint;
        hpText.text = "HP: " + nowHealthPoint + "/" + totHealthPoint;
        mpText.text = "MP: " + nowMagicPoint + "/" + totMagicPoint;
    }

}
