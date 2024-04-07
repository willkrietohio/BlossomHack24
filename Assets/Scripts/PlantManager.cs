using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlantManager : Singleton<PlantManager>
{
    public class CropTile
    {
        private Vector3Int pos;
        private TileBase[] tiles;
        private int index;
        private int ticks = 0;
        private int tickGoal = 300;

        public CropTile(Vector3Int _pos, TileBase[] _tiles, int _tickGoal)
        {
            pos = _pos;
            tiles = _tiles;
            index = 0;
            UpdateGraphics();
            tickGoal = _tickGoal;
        }

        public void AddTick()
        {
            ticks += (Instance.soilMap.GetTile(pos).name == Instance.wateredTile.name) ? 5 : 1;
            if (ticks >= tickGoal && index < 5)
            {
                ticks -= tickGoal;
                UpdateGraphics();
                if (index == 5) Instance.soilMap.SetTile(pos, Instance.tilledTile);
            }
        }

        public bool atLocation(Vector3Int posIn)
        {
            return pos == posIn;
        }

        public bool finished() 
        {
            return index == 5;
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

    public CropTile FindCropTile(Vector3Int posIn)
    {
        for (int i = 0; i < Crops.Count; i++)
        {
            if (Crops[i].atLocation(posIn)) return Crops[i];
        }
        return null;
    }

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
            if (soilMap.GetTile(position))
            {
                if (soilMap.GetTile(position).name == tilledTile.name || soilMap.GetTile(position).name == wateredTile.name)
                {
                    if (!plantMap.GetTile(position))
                    {
                        switch (GameManager.instance.selected)
                        {
                            case GameManager.Selectables.Wheat:
                                if (GameManager.instance.gold >= 5)
                                {
                                    Crops.Add(new CropTile(position, wheatTile, 100));
                                    GameManager.instance.gold -= 5;
                                    GameManager.instance.updateGold();
                                }
                                break;
                            case GameManager.Selectables.Carrots:
                                if (GameManager.instance.gold >= 8)
                                {
                                    Crops.Add(new CropTile(position, carrotTile, 150));
                                    GameManager.instance.gold -= 8;
                                    GameManager.instance.updateGold();
                                }
                                break;
                            case GameManager.Selectables.Corn:
                                if (GameManager.instance.gold >= 15)
                                {
                                    Crops.Add(new CropTile(position, cornTile, 200));
                                    GameManager.instance.gold -= 15;
                                    GameManager.instance.updateGold();
                                }

                                break;
                            case GameManager.Selectables.Tomatos:
                                if (GameManager.instance.gold >= 25)
                                {
                                    Crops.Add(new CropTile(position, tomatoTile, 250));
                                    GameManager.instance.gold -= 25;
                                    GameManager.instance.updateGold();
                                }

                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        CropTile tile = FindCropTile(position);
                        if (tile != null && tile.finished())
                        {
                            if (plantMap.GetTile(position).name == wheatTile[4].name)
                            {
                                GameManager.instance.gold += 8;
                                GameManager.instance.updateGold();
                            }
                            else if (plantMap.GetTile(position).name == carrotTile[4].name)
                            {
                                GameManager.instance.gold += 15;
                                GameManager.instance.updateGold();
                            }
                            else if (plantMap.GetTile(position).name == cornTile[4].name)
                            {
                                GameManager.instance.gold += 35;
                                GameManager.instance.updateGold();
                            }
                            else if (plantMap.GetTile(position).name == tomatoTile[4].name)
                            {
                                GameManager.instance.gold += 58;
                                GameManager.instance.updateGold();
                            }
                            plantMap.SetTile(position, null);
                            Crops.Remove(tile);
                        }
                    }
                    if (GameManager.instance.selected == GameManager.Selectables.Water)
                    {
                        soilMap.SetTile(position, wateredTile);
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
