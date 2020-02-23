using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodFollowController : MonoBehaviour{
    
    private Camera mainCamera;
    float height;
    public int roleType;
    public int roleId;
    public Texture2D bloodBackground;
    public Texture2D bloodFill;
    PlayerController playerController;
    private int totHealthPoint;
    private int nowHealthPoint;

    // Start is called before the first frame update
    void Start() {
        RoleConfig roles = null;
        if (roleType == 0) roles = Resources.Load("Role/Players") as RoleConfig;
        else roles = Resources.Load("Role/Monsters") as RoleConfig;
        role_data roleData = roles.role_config_list[roleId];
        totHealthPoint = roleData.role_tot_health_point;
        nowHealthPoint = roleData.role_now_health_point;
        mainCamera = Camera.main;
        float size_y = GetComponent<Collider>().bounds.size.y;
        float scal_y = transform.localScale.y;
        height = size_y * scal_y;
    }

    void Update() {
        RoleConfig roles = null;
        if (roleType == 0) roles = Resources.Load("Role/Players") as RoleConfig;
        else roles = Resources.Load("Role/Monsters") as RoleConfig;
        role_data roleData = roles.role_config_list[roleId];
        totHealthPoint = roleData.role_tot_health_point;
        nowHealthPoint = roleData.role_now_health_point;
        mainCamera = Camera.main;
        float size_y = GetComponent<Collider>().bounds.size.y;
        float scal_y = transform.localScale.y;
        height = size_y * scal_y;
    }

    void OnGUI() {
        RoleConfig roles = null;
        if (roleType == 0) roles = Resources.Load("Role/Players") as RoleConfig;
        else roles = Resources.Load("Role/Monsters") as RoleConfig;
        role_data roleData = roles.role_config_list[roleId];
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        Vector2 position = mainCamera.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, Screen.height - position.y);
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(bloodFill));
        float bloodWidth = bloodFill.width * nowHealthPoint / totHealthPoint;
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodSize.x, bloodSize.y), bloodBackground);
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodWidth, bloodSize.y), bloodFill);

        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(roleData.role_name));
        GUI.color = Color.black;
        GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), roleData.role_name);
    }
}
