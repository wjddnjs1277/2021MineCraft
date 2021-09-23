using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public int type;
    public bool isVisual;
    public GameObject blockObj;

    public Block(int type, bool isVisual, GameObject blockObj)
    {
        this.type = type;
        this.isVisual = isVisual;
        this.blockObj = blockObj;
    }
}

public class GroundGenerator : Singleton<GroundGenerator>
{
    [Header("Ground info")]
    [SerializeField] float waveLenght;      // 파장
    [SerializeField] float amplitude;       // 진폭
    [SerializeField] GameObject[] blocks;
    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject cloudPrefab;

    static int sizeX = 125;
    static int sizeY = 125;
    static int sizeZ = 125;

    float groundHeight = 10;

    public Block[,,] worldBlock = new Block[sizeX, sizeY, sizeZ];

    void Start()
    {
        StartCoroutine(GameInit());
    }

    public void VisualBlock(Vector3 pos)
    {
        Vector3 blockPos = pos;

        worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;

        for(int x= -1; x <=1; x++)
        {

            for (int y = -1; y <= 1; y++)
            {

                for (int z = -1; z <= 1; z++)
                {
                    if(!(x ==0 && y == 0 && z == 0))
                    {
                        // 배열의 범위에 벗어나지 않도록 계산
                        if (blockPos.x + x < 0 || blockPos.x + x > sizeX)
                            continue;
                        if (blockPos.y + y < 0 || blockPos.y + y > sizeY)
                            continue;
                        if (blockPos.z + z < 0 || blockPos.z + z > sizeZ)
                            continue;

                        Vector3 neighbour = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);
                        DrawBlock(neighbour);
                    }
                }
            }
        }
    }
    void DrawBlock(Vector3 blockPos)
    {
        if (worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
            return;

        if(!worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].isVisual)
        {
            GameObject newBlock = null;
            worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].isVisual = true;

            switch (worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type)
            {
                case 0:
                    newBlock = (GameObject)Instantiate(blocks[0], blockPos, Quaternion.identity);
                    break;
                case 1:
                    newBlock = (GameObject)Instantiate(blocks[1], blockPos, Quaternion.identity);
                    break;
                case 2:
                    newBlock = (GameObject)Instantiate(blocks[2], blockPos, Quaternion.identity);
                    break;
                case 3:
                    newBlock = (GameObject)Instantiate(blocks[3], blockPos, Quaternion.identity);
                    break;
                case 4:
                    newBlock = (GameObject)Instantiate(blocks[4], blockPos, Quaternion.identity);
                    break;
                default:
                    worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].isVisual = false;
                    break;
            }

            if (newBlock != null)
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].blockObj = newBlock;
        }
    }
    

    IEnumerator GameInit()
    {
        // 맵 만들기
        yield return StartCoroutine(MapInit());

        yield return StartCoroutine(CrateCloud(150, 30));

        // 동굴 만들기 ...
    }

    IEnumerator MapInit()
    {
        for(int x = 0; x< sizeX; x++)
        {
            for(int z=0; z<sizeZ;z++)
            {
                float xCoord = (x + 0) / waveLenght;
                float zCoord = (z + 0) / waveLenght;
                int y = (int)(Mathf.PerlinNoise(xCoord, zCoord) * amplitude + groundHeight);
                Vector3 pos = new Vector3(x, y, z);
                StartCoroutine(CreatBlock(y, pos, true));

                while(y > 0)
                {
                    y--;
                    pos = new Vector3(x, y, z);
                    StartCoroutine(CreatBlock(y, pos, false));
                }

            }
        }

        yield return null;
    }

    IEnumerator CreatBlock(int y, Vector3 blockPos, bool visual)
    {
        if(y > 22)
        {
            if(visual)
            {
                GameObject blockObj = (GameObject)Instantiate(blocks[0], blockPos, Quaternion.identity);
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = 
                    new Block(0, visual, blockObj);
            }
            else
            {
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = 
                    new Block(0, visual, null);
            }
        }
        else if (y > 15)
        {
            if (visual)
            {
                GameObject blockObj = (GameObject)Instantiate(blocks[1], blockPos, Quaternion.identity);
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(1, visual, blockObj);
            }
            else
            {
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(1, visual, null);
            }
        }
        else if (y > 10)
        {
            if (visual)
            {
                GameObject blockObj = (GameObject)Instantiate(blocks[2], blockPos, Quaternion.identity);
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(2, visual, blockObj);
            }
            else
            {
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(2, visual, null);
            }
        }
        else if (y > 5)
        {
            if (visual)
            {
                GameObject blockObj = (GameObject)Instantiate(blocks[3], blockPos, Quaternion.identity);
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(3, visual, blockObj);
            }
            else
            {
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(3, visual, null);
            }
        }
        else if( y > 1)
        {
            if (visual)
            {
                GameObject blockObj = (GameObject)Instantiate(blocks[4], blockPos, Quaternion.identity);
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(4, visual, blockObj);
            }
            else
            {
                worldBlock[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] =
                    new Block(4, visual, null);
            }
        }

        if(visual)
        {
            if (Random.value < 0.01f)
                CreatTree(blockPos);
        }

        yield return null;
    }
    void CreatTree(Vector3 pos)
    {
        Instantiate(treePrefab, pos, Quaternion.identity);
    }
    IEnumerator CrateCloud(int number, int cloudSize)
    {
        for(int i=0; i < number; i++)
        {
            int xPos = Random.Range(0, sizeX + 200);
            int zPos = Random.Range(0, sizeZ + 200);

            for(int j = 0; j < cloudSize; j++)
            {
                Vector3 pos = new Vector3(xPos, 80, zPos);
                Instantiate(cloudPrefab, pos, Quaternion.identity);
                xPos += Random.Range(-1, 2);
                zPos += Random.Range(-1, 2);
            }
        }
        yield return null;
    }
}
