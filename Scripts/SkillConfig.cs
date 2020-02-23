using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class skill_data {
    public int skill_id;
    public string skill_name;
    public GameObject skill_effect;
    public float skill_delay;
    public float skill_cd;
    public float skill_length;
    public float skill_stay_length;
    public int skill_damage;
    public GameObject skill_box;
}

[CreateAssetMenu(menuName = "Editor/SkillConfig")]
[Serializable]
public class SkillConfig : ScriptableObject{
    // Start is called before the first frame update
    public List<skill_data> skill_config_list = new List<skill_data>();
}
