using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap plantMap;
    public Tilemap soilMap;
    public TileBase[] wheatTile;
    public TileBase[] carrotTile;
    public TileBase[] cornTile;
    public TileBase[] tomatoTile;
    public TileBase soilTile;
    public TileBase tilledTile;
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
            if (soilMap.GetTile(position).name == tilledTile.name) {
                switch (GameManager.instance.selected)
                {
                    case GameManager.Selectables.Wheat:
                        plantMap.SetTile(position, wheatTile[0]);
                        break;
                    case GameManager.Selectables.Carrots:
                        plantMap.SetTile(position, carrotTile[0]);
                        break;
                    case GameManager.Selectables.Corn:
                        plantMap.SetTile(position, cornTile[0]);
                        break;
                    case GameManager.Selectables.Tomatos:
                        plantMap.SetTile(position, tomatoTile[0]);
                        break;
                    default: break;
                }
            }
            else if (soilMap.GetTile(position).name == soilTile.name && GameManager.instance.selected == GameManager.Selectables.Hoe)
            {
                soilMap.SetTile(position, tilledTile);
            }
        }
    }
}
