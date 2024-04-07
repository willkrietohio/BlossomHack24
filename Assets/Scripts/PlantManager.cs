using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantManager : Singleton<PlantManager>
{
    private class CropTile
    {
        private Vector3Int pos;
        private TileBase[] tiles;
        private int index;
        private int ticks = 0;

        public CropTile(Vector3Int _pos, TileBase[] _tiles)
        {
            pos = _pos;
            tiles = _tiles;
            index = 0;
            UpdateGraphics();
        }

        public void AddTick()
        {
            ticks += (Instance.soilMap.GetTile(pos).name == Instance.wateredTile.name) ? 5 : 1;
            if (ticks % 100 == 0 && index < 5)
                UpdateGraphics();
        }

        private void UpdateGraphics() => Instance.plantMap.SetTile(pos, tiles[index++]);
    }

    [SerializeField] private Tilemap plantMap;
    [SerializeField] private Tilemap soilMap;
    [SerializeField] private TileBase[] wheatTile;
    [SerializeField] private TileBase[] carrotTile;
    [SerializeField] private TileBase[] cornTile;
    [SerializeField] private TileBase[] tomatoTile;
    [SerializeField] private TileBase soilTile;
    [SerializeField] private TileBase tilledTile;
    [SerializeField] private TileBase wateredTile;
    [SerializeField] private Grid grid;
    private List<CropTile> Crops = new List<CropTile>();

    private void Start()
    {
#pragma warning disable 4014
        Tick();
#pragma warning restore 4014
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            // save the camera as public field if you using not the main camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = grid.WorldToCell(worldPoint);
            if (soilMap.GetTile(position) && !plantMap.GetTile(position))
            {
                if (soilMap.GetTile(position).name == tilledTile.name)
                {
                    switch (GameManager.instance.selected)
                    {
                        case GameManager.Selectables.Wheat:
                            Crops.Add(new CropTile(position, wheatTile)); break;
                        case GameManager.Selectables.Carrots:
                            Crops.Add(new CropTile(position, carrotTile)); break;
                        case GameManager.Selectables.Corn:
                            Crops.Add(new CropTile(position, cornTile)); break;
                        case GameManager.Selectables.Tomatos:
                            Crops.Add(new CropTile(position, tomatoTile)); break;
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
    private async Task Tick()
    {
        Crops.ForEach(c => c.AddTick());
        await Task.Delay(200);
        if(Application.isPlaying)
#pragma warning disable 4014
            Tick();
#pragma warning restore 4014
    }
}
