using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap plantMap;
    public Tilemap soilMap;
    public TileBase tile;
    public TileBase soilTile;
    public Grid grid;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            // save the camera as public field if you using not the main camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = grid.WorldToCell(worldPoint);
            //if (soilMap.GetTile(position) == soilTile) 
                plantMap.SetTile(position, tile);
        }
    }
    public void OnClick(GameObject go)
    {
        //if (go == null) return;
    }
}
