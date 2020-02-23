using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class role_data {
    public int role_id;
    public string role_name;
    public int role_tot_health_point;
    public int role_now_health_point;
    public int role_add_health_point;
    public int role_tot_magic_point;
    public int role_now_magic_point;
    public int role_add_magic_point;
    public int role_attack_point;
    public int role_defense_point;
}

[CreateAssetMenu(menuName = "Editor/RoleConfig")]
[Serializable]
public class RoleConfig : ScriptableObject {
    // Start is called before the first frame update
    public List<role_data> role_config_list = new List<role_data>();
}
