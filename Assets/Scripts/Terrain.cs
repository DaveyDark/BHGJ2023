using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TerrainType{EARTH,WATER,FIRE,AIR,NONE};

public class Terrain : MonoBehaviour
{
    // Start is called before the first frame update
    

    [Header("Terrain properties")]
    public TerrainType terrainType;
    public Color overlayColor;
    
    public Image overlay; //This should be moved to GameManager

    class TerrainProperty{
        public float max_time,drag;
        //public TerrainType[] strong_against;
    }

    private static Dictionary<TerrainType,TerrainProperty> terrainProperties = new Dictionary<TerrainType,TerrainProperty> { 
        { TerrainType.EARTH,new TerrainProperty{ max_time = 5,drag = 8} },
        { TerrainType.WATER,new TerrainProperty{ max_time = 7,drag = 3} },
        { TerrainType.AIR,new TerrainProperty{ max_time = 4,drag = 1}},
        { TerrainType.FIRE,new TerrainProperty{ max_time = 8,drag = 6} },
    };

    private float current_time=0f;
    private bool activated = false;
    private TerrainProperty properties;

    void Start()
    {
        properties = terrainProperties[terrainType];
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            
            current_time += Time.deltaTime;
            if (current_time >= properties.max_time) {
                current_time = 0f;
                

                Debug.Log("Time exceeded");
                //Game Over
            }

            //Change Overlay color
            Color color = overlay.color;
            color.a=(current_time/properties.max_time);
            overlay.color = color;
        }
    }
    

    public float getDrag(){
        return properties.drag;
    }


    public void Activate(){
        current_time = -1f;
        activated = true;
        overlay.color = overlayColor;
        
    }

    public void Deactivate(){
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0f);
        activated = false;
    }
}
