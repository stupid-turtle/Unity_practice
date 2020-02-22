using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodFollowController : MonoBehaviour{
    
    private Camera mainCamera;
    float height;
    public string PlayerName;
    public Texture2D bloodBackground;
    public Texture2D bloodFill;
    PlayerController playerController;
    private int totHealthPoint;
    private int nowHealthPoint;

    // Start is called before the first frame update
    void Start() {
        playerController = new PlayerController();
        totHealthPoint = playerController.totHealthPoint;
        nowHealthPoint = playerController.nowHealthPoint;
        mainCamera = Camera.main;
        float size_y = GetComponent<Collider>().bounds.size.y;
        float scal_y = transform.localScale.y;
        height = size_y * scal_y;
    }
    
    void OnGUI() {
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        Vector2 position = mainCamera.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, Screen.height - position.y);
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(bloodFill));
        float bloodWidth = bloodFill.width * nowHealthPoint / totHealthPoint;
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodSize.x, bloodSize.y), bloodBackground);
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodWidth, bloodSize.y), bloodFill);

        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(PlayerName));
        GUI.color = Color.black;
        GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), PlayerName);
    }
}
