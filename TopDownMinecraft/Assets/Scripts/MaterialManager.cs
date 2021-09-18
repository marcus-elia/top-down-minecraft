using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Material grassMat;
    public Material grassMatST;
    public Material grassMatT;
    public Material bedrockMat;
    public Material bedrockMatST;
    public Material bedrockMatT;
    public Material stoneMat;
    public Material stoneMatST;
    public Material stoneMatT;

    // The preset types of blocks
    public BlockProperties grass;
    public BlockProperties bedrock;
    public BlockProperties stone;

    // Start is called before the first frame update
    void Start()
    {
        grass = new BlockProperties(BlockType.Grass, grassMat, grassMatT, grassMatST, true);
        bedrock = new BlockProperties(BlockType.Bedrock, bedrockMat, bedrockMatT, bedrockMatST, false);
        stone = new BlockProperties(BlockType.Stone, stoneMat, stoneMatT, stoneMatST, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
