using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaster : MonoBehaviour
{
    //Governs random map generation

    //Objects and Components:
    public GameObject grassTile;
    public Vector2 tileOffset;
    public float tileRowOffset; //How far away (along the x axis) tiles are from each other
    public float tileVibrance; //(Default is 60) Affects how brightly-colored tiles are
    [Space()]
    public int tileGridX;
    public int tileGridY;
    private Vector3 centerMap;

    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        //Generates a map based on the input factors
        for (int x = 0; x < tileGridX; x++) //Create X columns
        {
            Vector3 placePos = transform.position; //Get current position tracker to keep track of where tiles are being placed
            for (int y = 0; y < tileGridY; y++) //Create tiles of the appropriate number of rows in each column
            {
                placePos.x = (tileRowOffset * x) + (tileOffset.x * y); placePos.y = (tileOffset.y * y); //Prep position for instantiated tile to go
                GameObject newTile = Instantiate(grassTile, transform); //Instantiate new map tile
                newTile.transform.position = placePos; //Set newTile position

                //Color Tiles:
                int color = Random.Range(0, 3);

                switch (color)
                {
                    case 0: //RED GRASS
                        ColorTile(newTile, 0); //Color all grass children in this tile RED
                        newTile.tag = "Red_Grass";
                        break;
                    case 1: //BLUE GRASS
                        ColorTile(newTile, 196); //Color all grass children in this tile BLUE
                        newTile.tag = "Blue_Grass";
                        break;
                    case 2: //GREEN GRASS
                        ColorTile(newTile, 123); //Color all grass children in this tile GREEN
                        newTile.tag = "Green_Grass";
                        break;
                }
                        //Get Center of Map:
                        if (x == (tileGridX/2) - (tileGridX%2) && y == (tileGridY/2) + (tileGridY%2)) { centerMap = newTile.transform.position; }
            }
        }
        transform.position = new Vector3(-centerMap.x, -centerMap.y, 0); //Set whole map position to center of camera
    }

    private void ColorTile(GameObject tile, float hue) //Colors entirety of given tile with given hue
    {
        SpriteRenderer[] grassRen = tile.GetComponentsInChildren<SpriteRenderer>(); //Get spriterenderers of all grass in tile
        for (int z = 0; z < grassRen.Length; z++) //Parse through all grass renderers in tile
            {
                grassRen[z].color = Color.HSVToRGB(hue/360, tileVibrance/100, grassRen[z].color.grayscale); //Give each tile appropriate hue
            Debug.Log("MapMaster: Setting Tile Color. R = " + grassRen[z].color.r + ", G = " + grassRen[z].color.g + ", B = " + grassRen[z].color.b);
            }
        
    }
}
