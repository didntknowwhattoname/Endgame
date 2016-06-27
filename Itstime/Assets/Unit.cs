using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Unit : MonoBehaviour
{

    // tileX and tileY represent the correct map-tile position
    // for this piece.  Note that this doesn't necessarily mean
    // the world-space coordinates, because our map might be scaled
    // or offset or something of that nature.  Also, during movement
    // animations, we are going to be somewhere in between tiles.
    public int tileX;
    public int tileZ;
    public GameObject character;
    public GameObject unit;
    public CharGrid map;
    public string backward;
    public string forward;
    public string right;
    public string left;
    public float distance = 50f;
    public float speed = 5f;
    public bool pointSelected = true;
    bool once = true;
    public GameObject pathColor;
    public List<GameObject> highlightPath = new List<GameObject>();

    // Our pathfinding info.  Null if we have no destination ordered.
    public List<Node> currentPath = null;

    // How far this unit can move in one turn. Note that some tiles cost extra.
    //int moveSpeed = 2;
    void Start()
    {
        map.units.Add(this);
        
    }

    void Update()
    {
        
        if (once)
        {
            map.setTile(tileX, tileZ, false);
            once = false;
        }

        if (currentPath != null)
           {
            map.setTile(tileX, tileZ, true);
            map.setTile(currentPath[currentPath.Count - 1].x, currentPath[currentPath.Count - 1].z, false);

            }

                // Have we moved our visible piece close enough to the target tile that we can
                // advance to the next step in our pathfinding?
                if (Vector3.Distance(transform.position, map.TileCoordToWorldCoord(tileX, tileZ)) < 0.1f)
                    AdvancePathing();

                // Smoothly animate towards the correct map tile.
                if ((float)decimal.Round((decimal)transform.position.z, 2) > map.TileCoordToWorldCoord(tileX, tileZ).z)
                    moveDown();
                if ((float)decimal.Round((decimal)transform.position.z, 2) < map.TileCoordToWorldCoord(tileX, tileZ).z)
                    moveUp();
                if ((float)decimal.Round((decimal)transform.position.x, 2) > map.TileCoordToWorldCoord(tileX, tileZ).x)
                    moveLeft();
                if ((float)decimal.Round((decimal)transform.position.x, 2) < map.TileCoordToWorldCoord(tileX, tileZ).x)
                    moveRight();

            

        }

    // Advances our pathfinding progress by one tile.
    void AdvancePathing()
    {
        if (currentPath == null)
        {
            foreach (GameObject go in highlightPath)
            {
                DestroyObject(go);
            }
            return;
        }




        // Teleport us to our correct "current" position, in case we
        // haven't finished the animation yet.
        transform.position = map.TileCoordToWorldCoord(tileX, tileZ);

        // Get cost from current tile to next tile

        // Move us to the next tile in the sequence
        tileX = currentPath[1].x;
        tileZ = currentPath[1].z;

        //map.setTile(currentPath[1].x, currentPath[1].z, false);

        // Remove the old "current" tile from the pathfinding list
        //map.setTile(currentPath[0].x, currentPath[0].z, true);
        currentPath.RemoveAt(0);

        if (currentPath.Count == 1)
        {
            // We only have one tile left in the path, and that tile MUST be our ultimate
            // destination -- and we are standing on it!
            // So let's just clear our pathfinding info.
            currentPath = null;
           // map.selectedUnit = null;
        }
    }


    void moveDown()
    {
        character.GetComponent<Animator>().Play(backward);
        transform.Translate(new Vector3(0f, 0, -1f) * distance/speed * Time.deltaTime);
        if (decimal.Round((decimal)transform.position.z, 2) < (decimal)map.TileCoordToWorldCoord(tileX, tileZ).z)
        {
            transform.position = new Vector3(map.TileCoordToWorldCoord(tileX, tileZ).x, 0, map.TileCoordToWorldCoord(tileX, tileZ).z);
        }
    }
    void moveUp()
    {
        character.GetComponent<Animator>().Play(forward);
       transform.Translate(new Vector3(0, 0, 1f) * distance / speed * Time.deltaTime);
        if (decimal.Round((decimal)transform.position.z, 2) > (decimal)map.TileCoordToWorldCoord(tileX, tileZ).z)
        {
            transform.position = new Vector3(map.TileCoordToWorldCoord(tileX, tileZ).x, 0, map.TileCoordToWorldCoord(tileX, tileZ).z);
        }
    }

    void moveRight()
    {
        character.GetComponent<Animator>().Play(right);
        transform.Translate(new Vector3(1f, 0, 0) * distance / speed * Time.deltaTime);
        if (decimal.Round((decimal)transform.position.x, 2) > (decimal)map.TileCoordToWorldCoord(tileX, tileZ).x)
        {
            transform.position = new Vector3(map.TileCoordToWorldCoord(tileX, tileZ).x, 0, map.TileCoordToWorldCoord(tileX, tileZ).z);
        }

    }

    void moveLeft()
    {
        character.GetComponent<Animator>().Play(left);
        transform.Translate(new Vector3(-1f, 0, 0) * distance / speed * Time.deltaTime);
        if (decimal.Round((decimal)transform.position.x, 2) < (decimal)map.TileCoordToWorldCoord(tileX, tileZ).x)
        {
            transform.position = new Vector3(map.TileCoordToWorldCoord(tileX, tileZ).x, 0, map.TileCoordToWorldCoord(tileX, tileZ).z);
        }

    }

}
