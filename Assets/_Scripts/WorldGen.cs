using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorldGen : MonoBehaviour
{
    [Header("Config")]
    public Vector3 minDisplacement, maxDisplacement;
    public Transform worldParent;

    [Header("Tile prefabs")]
    public TilePath pathTilePrefab, pathCornerTilePrefab;
    public TileEnd endTilePrefab;

    [Header("End positions")]
    public TileEnd endA, endB;

    public void Generate()
    {
        Debug.Log("<color=cyan>[INFO] Generating World </color>", this);
        endA = this.GenerateEndTile(new Vector3(0,0,0));
        endB = this.GenerateEndTile(this.GenerateRandomPosition());
        this.PlaceTiles();
        Debug.Log("<color=green>[SUCCESS] WorldGen Initialized. </color>", this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// Generates tiles from End A to End B by using linked lists. 
    /// Simple PCG to showcase some basic grasp of datastructures/nested shorthands/procedural generation.
    /// </summary>
    private void PlaceTiles()
    {
        Debug.Log("<color=cyan>[INFO] Placing Tiles. </color>", this);
        TilePath currTile, prevTile;
        int prevX =0, prevZ = 0;

        try
        {
            currTile = endA;

            while(currTile.NextTile != endB)
            {
                prevTile = currTile;

                Vector3 diff = endB.transform.position - prevTile.transform.position; //Get the difference between the current position and the end.
                int fuzziness = Random.Range(0, 100); //Fuzziness factor to have some variance. May cause some wasted loops.
                int nextX = 0, nextZ = 0;

                nextX = diff.x == 0 ? 0 : (int)diff.x > 0 ? 1 : -1; //If there's still a difference in X axis, move +-1. Else, move 0.
                nextZ = diff.z == 0 ? 0 : (int)diff.z > 0 ? 1 : -1 ; //If there's a difference in Z, move +-1. Else, move 0.

                if(nextX != 0 && nextZ != 0)
                {
                    if(fuzziness < 50)
                        nextX = 0;
                    else
                        nextZ = 0;
                }


                Vector3 newPos = new Vector3(prevTile.transform.position.x + nextX, 0, prevTile.transform.position.z + nextZ);
                



                if((newPos != endB.transform.position))
                {
                    currTile = Instantiate(pathTilePrefab, newPos, Quaternion.identity);
                    currTile.PrevTile = prevTile;
                    prevTile.NextTile = currTile;

                    if(nextZ != 0)
                        currTile.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);



                    currTile.transform.parent = this.worldParent;
                    Debug.Log("<color=cyan>Tile Placed</color>", currTile);
                }
                else
                {
                    currTile.NextTile = endB;
                    endB.PrevTile = currTile;
                    Debug.Log("<color=green>[SUCCESS] Reached the end of world generation. </color>", this);
                }

                prevX = nextX;
                prevZ = nextZ;

            }
            Debug.Log("<color=green>[SUCCESS] Tiles Placed. </color>", this);
        }catch(System.Exception e)
        {
            Debug.LogError(e);
        }

    }

    private TileEnd GenerateEndTile(Vector3 pos)
    {
        Debug.Log($"<color=cyan>[INFO] Generating end tile on {pos.x}, {pos.y}, {pos.z} </color>", this);
        TileEnd end = Instantiate(endTilePrefab, pos, Quaternion.identity);
        end.transform.parent = this.worldParent;
        Debug.Log("<color=green>[SUCCESS] End Tile Placed.</color>", this);
        return end;
    }

    private Vector3 GenerateRandomPosition()
    {
        int x = Random.Range((int)minDisplacement.x, (int)maxDisplacement.x);
        int z =  Random.Range((int)minDisplacement.z, (int)maxDisplacement.z);
        return new Vector3(x, 0,z);
    }
}
