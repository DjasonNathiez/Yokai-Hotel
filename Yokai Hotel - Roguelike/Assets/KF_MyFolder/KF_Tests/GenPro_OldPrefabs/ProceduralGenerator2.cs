using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator2 : MonoBehaviour
{
    [Header("Grid")]
    public int gridSizeX;
    public int gridSizeY;

    public float resolutionX;
    public float resolutionY;

   
    public GameObject cellObj, cellOrign, cellGround;

    [Header("Room")]
    public int maxRoomQ;
    public int minRoomQ;
    public int roomQ;
    int currentRoomQ;

    [Header("Walker")]
    public int maxWalk;
    public int currentWalk;

    [Range(0, 1)] public float changeDirP;
    [Range(0, 1)] public float generateP, destroyP;


    // Start is called before the first frame update
    void Start()
    {
        // initialize cellsGrid
        bool[,] cellsGrid = new bool[(gridSizeX), (gridSizeY)];

        // initialize room
        roomQ = Random.Range(minRoomQ, maxRoomQ);

        // initialize walker
        List<int[,]> walkers = new List<int[,]>();
        List<int[,]> targetWalkers = new List<int[,]>();

        Vector2 walkPos = new Vector2(Mathf.Floor(0.5f * gridSizeX), Mathf.Floor(0.5f * gridSizeY));
        Vector2 walkDir = G4Dir();

        //int[] walkerPos = { (int)walkPos.x, (int)walkPos.y };
        //int[] walkerDir = { (int)walkDir.x, (int)walkDir.y };

        int[,] startWalker = {  { (int)walkPos.x, (int)walkPos.y }, 
                                { (int)walkDir.x, (int)walkDir.y } };

        walkers.Add(startWalker);

        // show Origin cell
        Vector3 posOrigin = new Vector3(walkPos.x * resolutionX, walkPos.y * resolutionY, -1);
        Instantiate(cellOrign,posOrigin , transform.rotation);

        // wallker generation
        while (currentRoomQ < roomQ)
        {
            for (int i = 0; i < walkers.Count; i++)
            {
                int[,] walker = walkers[i];

                // generate walker
                if (Random.value < generateP)
                {
                    int[,] walkerD = walker;

                    Vector2 dir = G4Dir();
                    walkerD[1, 0] = (int)dir.x;
                    walkerD[1, 1] = (int)dir.y;

                    targetWalkers.Add(walkerD);
                }


                // change dir
                if (Random.value < changeDirP)
                {
                    Vector2 dir = G4Dir();
                    walker[1, 0] = (int)dir.x;
                    walker[1, 1] = (int)dir.y;
                }


                // move walker
                bool inRangeX = (walker[0, 0] + walker[1, 0] < gridSizeX && walker[0, 0] + walker[1, 0] > 0);
                bool inRangeY = (walker[0, 1] + walker[1, 1] < gridSizeY && walker[0, 1] + walker[1, 1] > 0);

                if (inRangeX)
                    walker[0, 0] += walker[1, 0];
                if (inRangeY)
                    walker[0, 1] += walker[1, 1];

                // update current walker
                if (cellsGrid[walker[0, 0], walker[0, 1]] != true)
                {
                    cellsGrid[walker[0, 0], walker[0, 1]] = true;
                    currentRoomQ++;

                    Debug.Log("room number" + roomQ);
                }

                // if not destroy add in target list
                if (Random.value > destroyP)
                {
                    targetWalkers.Add(walker);
                }
                   
               
            }

            walkers.Clear();
            walkers.AddRange(targetWalkers);
            targetWalkers.Clear();

            System.GC.Collect();
            currentWalk++;
            Debug.Log("walk cycle" + currentWalk);
        }
        Debug.Log("end of  walk cycle");
        // show cell
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeX; y++)
            {
                if (cellsGrid[x, y] == true)
                {
                    Debug.Log("room generate" + x + ":" + y);
                    Instantiate(cellObj, new Vector2(x * resolutionX, y * resolutionY), transform.rotation);
                }
                //Instantiate(cellGround, new Vector3(x *resolution, y * resolution, 1), transform.rotation);
            }
        }

        walkers.Clear();
        targetWalkers.Clear();

        Debug.Log("generation end");
    }

    private void Update()
    {
        
    }

    public Vector2 G4Dir()
    {
        bool vertical = (Random.value <= 0.5f);

        Vector2 dir = new Vector2(vertical ? 0 : Mathf.Sign(Random.value - 0.5f),
                                  vertical ? Mathf.Sign(Random.value - 0.5f) : 0);
        return dir;
    }
}

