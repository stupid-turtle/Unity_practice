using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour{
    
    public Text hpText;
    public Text mpText;
    public Text attackText;
    public Text defenseText;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start(){
        RoleConfig roles = Resources.Load("Role/Players") as RoleConfig;
        role_data roleData = roles.role_config_list[0];

        hpText.text = "HP: " + roleData.role_now_health_point + "/" + roleData.role_tot_health_point;
        mpText.text = "MP: " + roleData.role_now_magic_point + "/" + roleData.role_tot_magic_point;
        attackText.text = "ATK: " + roleData.role_attack_point;
        defenseText.text = "DIT: " + roleData.role_defense_point;
    }

    // Update is called once per frame
    void Update() {
        RoleConfig roles = Resources.Load("Role/Players") as RoleConfig;
        role_data roleData = roles.role_config_list[0];
        hpText.text = "HP: " + roleData.role_now_health_point + "/" + roleData.role_tot_health_point;
        mpText.text = "MP: " + roleData.role_now_magic_point + "/" + roleData.role_tot_magic_point;
    }
}
