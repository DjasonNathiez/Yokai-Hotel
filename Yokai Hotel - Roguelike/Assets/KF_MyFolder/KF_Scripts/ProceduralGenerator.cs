using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralGenerator : MonoBehaviour
{
    [Header("Grid")]
    public int gridSizeX;
    public int gridSizeY;

    public float resolutionX;
    public float resolutionY;
    public RoomData[,] roomsGrid;
    public List<RoomData> validRooms = new List<RoomData>();

    public GameObject roomPrefab, keyPrefab, shopPrefab, startPrefab, preparePrefab, endPrefab;
    public List<GameObject> rooms;

    [Header("Room")]
    public int maxRoomQ;
    public int minRoomQ;

    public int maxRareRoomQ;
    int currentRareRoomQ = 0;

    [Range(0, 1)]
    public float rareRoomP;

    int roomQ;
    int currentRoomQ;

    bool UpdateRoom = true;

    [Header("Walker")]
    public int maxWalkCycle;
    int currentWalkCycle;

    public int maxWalkers;
    public int currentWalkers;

    public bool backtracking;
    List<Walker> walkers = new List<Walker>();
    List<Walker> targetWalkers = new List<Walker>();

    bool walkEnd = false;
    

    [Range(0, 1)] public float changeDirP;
    [Range(0, 1)] public float generateP, destroyP;


    // Start is called before the first frame update
    void Start()
    {
        // initialize cellsGrid
        roomsGrid = new RoomData[(gridSizeX), (gridSizeY)];

        // initialize room
        roomQ = Random.Range(minRoomQ, maxRoomQ);

        // initialize walker
        Vector2 walkPos = new Vector2(Mathf.Floor(0.5f * gridSizeX), Mathf.Floor(0.5f * gridSizeY));
        Vector2 walkDir = G4Dir();

        Walker walkOrigin = new Walker((int)walkPos.x, (int)walkPos.y, (int)walkDir.x, (int)walkDir.y);
        if (maxWalkers > 0)
        {
            // update current walker pos
            walkers.Add(walkOrigin);
            currentWalkers++;

            // archive walker in grid
            roomsGrid[walkOrigin.posX, walkOrigin.posY].type = Room.RoomType.START;
        }

        /* show Origin cell
        if (walkers.Count > 0)
        {
            Vector3 posOrigin = new Vector3(walkers[0].posX * resolutionX, walkers[0].posY * resolutionY, -1);
            Instantiate(cellOrign, posOrigin, transform.rotation);
        }
        */

        // Generate
        GenerateDungeon(ref roomsGrid, walkers, targetWalkers);

        // show graphic

         
        // show cell
        if (UpdateRoom && currentWalkCycle >= maxWalkCycle)
        {
            int farthestOX = walkOrigin.posX;
            int farthestOY = walkOrigin.posY;
            float powDistF = 0;

            int preparendRX = farthestOX;
            int preparendRY = farthestOY;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeX; y++)
                {
                    if (roomsGrid[x, y].type != Room.RoomType.NULL)
                    {

                        // instantiate room prefab
                        GameObject newRoom;
                        if (walkOrigin.posX == x && walkOrigin.posY == y)
                            newRoom = startPrefab;
                        else
                            newRoom = rooms[Random.Range(0, rooms.Count)];
                        GameObject roomObj = Instantiate(newRoom, new Vector2(x * resolutionX, y * resolutionY) , transform.rotation);
                        Room room = roomObj.GetComponent<Room>();

                        // apply room type and active door
                        if (room)
                        {
                            // apply basic room type
                            roomsGrid[x, y].room = room;
                            roomsGrid[x, y].room.UpdateRoom((int)roomsGrid[x, y].type);

                            // update room position
                            roomsGrid[x, y].posX = x;
                            roomsGrid[x, y].posY = y;
                            Vector2 placement = new Vector2(x, y);
                            room.roomPos = placement;


                            // definie active door
                            for (int a = 0; a < 360; a += 90)
                            {
                                float rad = Mathf.Deg2Rad * a;
                                Vector2 checkDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

                                bool checkRangeX = (x + (int)checkDir.x < gridSizeX && x + (int)checkDir.x > 0);
                                bool checkRangeY = (y + (int)checkDir.y < gridSizeY && y + (int)checkDir.y > 0);

                                if (checkRangeX && checkRangeY)
                                {
                                    Room.RoomType nearRoomType = roomsGrid[x + (int)checkDir.x, y + (int)checkDir.y].type;
                                    if ( nearRoomType != Room.RoomType.NULL)
                                    {
                                        roomsGrid[x, y].room.UpdateDoors((int)a / 90, 0, true);

                                        if(nearRoomType == Room.RoomType.RARE)
                                               roomsGrid[x, y].room.UpdateDoors((int)a / 90, 1, true);
                                    }
                                        
                                    else
                                        roomsGrid[x, y].room.UpdateDoors((int)a / 90, 0, false);
                                }
                                else
                                    roomsGrid[x, y].room.UpdateDoors((int)a / 90, 0, false);

                                

                            }

                            // archive room in valid list
                            validRooms.Add(roomsGrid[x, y]);

                            // update special room condition
                            float powDistOrgn = Mathf.Pow((x - walkOrigin.posX), 2) + Mathf.Pow((y - walkOrigin.posY), 2);

                            // search valid position for preparend room
                            if (roomsGrid[x, y].room.doors[1].open != true && powDistF <= powDistOrgn)
                            {
                                preparendRX = x;
                                preparendRY = y;

                                powDistF = powDistOrgn;
                                Debug.Log("Room up is" + roomsGrid[x, y].room.doors[1].open);
                            }
                        }

                    }
                }
            }
            // apply special room change

            // destroyRoom
            Destroy(roomsGrid[preparendRX, preparendRY].room.gameObject);

            // replace room 
            GameObject prepareRoom = Instantiate(preparePrefab, new Vector2(preparendRX * resolutionX, preparendRY * resolutionY), transform.rotation);
            roomsGrid[preparendRX, preparendRY].room = prepareRoom.GetComponent<Room>();

            roomsGrid[preparendRX, preparendRY].type = Room.RoomType.PREPAREND;
            roomsGrid[preparendRX, preparendRY].room.UpdateRoom((int)roomsGrid[preparendRX, preparendRY].type);
            roomsGrid[preparendRX, preparendRY].room.UpdateDoors(1, (int)Door.DoorType.FINAL, true);
            

            // generate end room
            Vector2 endRoomPos = new Vector2(preparendRX, preparendRY);
            GameObject endRoomObj = Instantiate(endPrefab, new Vector2(preparendRX * resolutionX, (preparendRY + 1) * resolutionY), transform.rotation);
            Room roomEnd = endRoomObj.GetComponent<Room>();

            roomsGrid[preparendRX, preparendRY + 1].type = Room.RoomType.END;

            roomsGrid[preparendRX, preparendRY + 1].room = roomEnd;
            roomsGrid[preparendRX, preparendRY + 1].room.UpdateRoom((int)roomsGrid[preparendRX, preparendRY+1].type);

            // generate key room 
            float powDistEnd = 0;
            RoomData keyRoomData = new RoomData(Room.RoomType.NULL, null, null, 0, 0);
            
            foreach( RoomData rd in validRooms)
            {
                float powDist = Mathf.Pow((endRoomPos.x - rd.posX), 2) + Mathf.Pow((endRoomPos.y - rd.posY), 2);
                bool roomCond = (rd.type == Room.RoomType.PREPAREND) || (rd.type == Room.RoomType.START) && (rd.type == Room.RoomType.KEY);
                if (powDistEnd <= powDist  && !roomCond && (rd.type == Room.RoomType.COMMUN || rd.type == Room.RoomType.RARE))
                {
                    powDistEnd = powDist;
                    keyRoomData = rd;
                }
            }

            // destroyRoom
            Destroy(keyRoomData.room.gameObject);

            // replace Room
            GameObject  keyRoom = Instantiate(keyPrefab, new Vector2(keyRoomData.posX * resolutionX, keyRoomData.posY * resolutionY), transform.rotation);
            roomsGrid[keyRoomData.posX, keyRoomData.posY].room = keyRoomData.room = keyRoom.GetComponent<Room>();

            keyRoomData.type = Room.RoomType.KEY;
            keyRoomData.room.UpdateRoom((int)keyRoomData.type);

            // generate shop room
            float maximisDist = 0;
            RoomData shopRoomData = new RoomData(Room.RoomType.NULL, null, null, 0, 0);
            
            foreach (RoomData rd in validRooms)
            {
                Vector2 currentPos = new Vector2(rd.posX, rd.posY);
                Vector2 keyPos = new Vector2(keyRoomData.posX, keyRoomData.posY);

                float sEndDist = Vector2.Distance(endRoomPos, currentPos);
                float sKeyDist = Vector2.Distance(keyPos, currentPos);
                float sStartDist = Vector2.Distance(walkPos, currentPos);

                float cMaximisDist = sEndDist + sKeyDist + sStartDist;
                bool roomCond = (sKeyDist != 0 && sEndDist != 0 && sStartDist != 0 && rd.type != Room.RoomType.PREPAREND);
                if (maximisDist <= cMaximisDist && roomCond)
                {
                    maximisDist = cMaximisDist;
                    shopRoomData = rd;
                    Debug.Log("assign shop room"+rd.posX * resolutionX+" "+ rd.posY * resolutionY);
                    Debug.Log("room cond is" + roomCond+"");
                }
            }

            // destroy Room
            //Destroy(shopRoomData.room.gameObject);

            // Replace
            //GameObject shopRoom = Instantiate(shopPrefab, new Vector2(shopRoomData.posX * resolutionX, shopRoomData.posY * resolutionY), transform.rotation);
            //roomsGrid[shopRoomData.posX, shopRoomData.posY].room = shopRoomData.room = shopRoom.GetComponent<Room>();

            shopRoomData.type = Room.RoomType.SHOP;
            shopRoomData.room.UpdateRoom((int)shopRoomData.type);

            // link door
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeX; y++) // search in grid
                {
                    Room room = roomsGrid[x, y].room;
                    if (room != null)
                    {
                        if (room.type != Room.RoomType.NULL || room.type != Room.RoomType.END)
                        {
                            if (room.type == Room.RoomType.START)
                            Debug.Log("StartUp");

                            for (int i = 0; i < room.doors.Length; i++)
                            {
                                Door door = room.doors[i];
                                if (door)
                                {
                                    int dirX = (int)door.dir.x;
                                    int dirY = (int)door.dir.y;

                                    Room checkRoom = roomsGrid[x + dirX, y + dirY].room; // get target room

                                    if (checkRoom)
                                    {
                                        if (checkRoom.type != Room.RoomType.NULL || checkRoom.type != Room.RoomType.END)
                                        {
                                            door.doorLink = checkRoom.doors[(i + 2) % 4];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // end update room 
            UpdateRoom = false;
        }
    }

    public struct Walker
    {
        public int posX, posY;
        public int dirX, dirY;

        // Constructor
        public Walker(int wPosX, int wPosY, int wDirX, int wDirY)
        {
            posX = wPosX;
            posY = wPosY;

            dirX = wDirX;
            dirY = wDirY;
        } 
    }

    public struct RoomData
    {
        public Room.RoomType type;
        public Room room;
        
        public Door[] doors;

        public int posX, posY;
        public RoomData (Room.RoomType roomType, Room roomScrpt, Door[] doorArray, int roomPosX, int roomPosY)
        {
            type = roomType;
            room = roomScrpt;
            doors = doorArray;

            posX = roomPosX;
            posY = roomPosY;
        }
    }

    public Vector2 G4Dir()
    {
        bool vertical = (Random.value <= 0.5f);

        Vector2 dir = new Vector2(vertical ? 0 : Mathf.Sign(Random.value - 0.5f),
                                        vertical ? Mathf.Sign(Random.value - 0.5f) : 0);
        return dir;
    }

    public void GenerateDungeon(ref RoomData[,] cellsGrid , List<Walker> walkers, List<Walker> targetWalkers)
    {
        // wallker generation
        if (currentRoomQ < roomQ && !walkEnd)
        {
            while (currentWalkCycle < maxWalkCycle)
            {
                for (int i = 0; i < walkers.Count; i++)
                {
                    Walker walker = walkers[i]; // struct require local variable to be modifie

                    // generate walker
                    if ((Random.value < generateP) && (currentWalkers < maxWalkers))
                    {
                        Walker walkerD = walker;

                        Vector2 dir = G4Dir();

                        walkerD.dirX = (int)dir.x;
                        walkerD.dirY = (int)dir.y;

                        currentWalkers++;
                        targetWalkers.Add(walkerD);
                    }


                    // change dir
                    bool stuck = false;
                    if (Random.value < changeDirP)
                    {
                        List<Vector2> freeDirs = new List<Vector2>();
                        Vector2 freeDir = Vector2.zero;

                        for (int a = 0; a < 360; a += 90)
                        {
                            float rad = Mathf.Deg2Rad * a;
                            Vector2 checkDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

                            bool checkRangeX = (walker.posX + (int)checkDir.x < gridSizeX && walker.posX + (int)checkDir.x > 0);
                            bool checkRangeY = (walker.posY + (int)checkDir.y < gridSizeY && walker.posY + (int)checkDir.y > 0);

                            if (checkRangeX && checkRangeY)
                            {
                                if (cellsGrid[walker.posX + (int)checkDir.x, walker.posY + (int)checkDir.y].type == Room.RoomType.NULL)
                                    freeDirs.Add(checkDir);
                            }
                        }

                        if (freeDirs.Count > 0)
                            freeDir = freeDirs[Random.Range(0, freeDirs.Count - 1)];
                        else
                            stuck = true;

                        Vector2 dir = (backtracking) ? G4Dir() : freeDir;

                        walker.dirX = (int)dir.x;
                        walker.dirY = (int)dir.y;
                    }


                    // move walker
                    bool inRangeX = (walker.posX + walker.dirX < gridSizeX && walker.posX + walker.dirX > 0);
                    bool inRangeY = (walker.posY + walker.dirY < gridSizeY && walker.posY + walker.dirY > 0);

                    if (inRangeX)
                        walker.posX += walker.dirX;

                    if (inRangeY)
                        walker.posY += walker.dirY;

                    // update current walker
                    if (cellsGrid[walker.posX, walker.posY].type == Room.RoomType.NULL && currentRoomQ < roomQ)
                    {
                        Room.RoomType roomType = (Random.value < rareRoomP && currentRareRoomQ < maxRareRoomQ) ? Room.RoomType.RARE : Room.RoomType.COMMUN;

                        cellsGrid[walker.posX, walker.posY].type = roomType;
                        if (roomType == Room.RoomType.RARE)
                            currentRareRoomQ++;

                        currentRoomQ++;
                    }

                    walkers[i] = walker;

                    // if not destroy or stuck add in target list
                    if ((Random.value >= destroyP && !stuck) || (targetWalkers.Count == 0 && currentRoomQ < roomQ))
                    {
                        Debug.Log("Walker in next list");
                        targetWalkers.Add(walkers[i]);
                    }
                    else
                        currentWalkers--;
                }

                walkers.Clear();
                walkers.AddRange(targetWalkers);
                targetWalkers.Clear();

                currentWalkCycle++;
            }
        }

        // walk end
        if (currentRoomQ >= roomQ && !walkEnd)
        {
            walkers.Clear();
            targetWalkers.Clear();

            walkEnd = true;
            Debug.Log("generation end");
        }
    }

}
