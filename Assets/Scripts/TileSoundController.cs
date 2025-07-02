using UnityEngine;

public class TileSoundController : MonoBehaviour
{
    [Header("Tile Detection")]
    [SerializeField] private LayerMask tileLayerMask;
    [SerializeField] private float checkDistance = 1.1f;
    [SerializeField] private string bigTileTag = "bigTile";
    [SerializeField] private string smallTileTag = "smallTile";
    
    private bool isOnBigTile = false;
    private bool isOnSmallTile = false;
    
    void Update()
    {
        CheckTileType();
    }
    
    void CheckTileType()
    {
        // Cast a ray downward to detect the tile beneath the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, checkDistance, tileLayerMask))
        {
            // Check if it's a big tile
            if (hit.collider.CompareTag(bigTileTag))
            {
                // If we just entered a big tile
                if (!isOnBigTile)
                {
                    AudioManager.Instance.PlayC6Note();
                    isOnBigTile = true;
                    isOnSmallTile = false;
                }
            }
            // Check if it's a small tile
            else if (hit.collider.CompareTag(smallTileTag))
            {
                // If we just entered a small tile
                if (!isOnSmallTile)
                {
                    AudioManager.Instance.PlayC3Note();
                    isOnSmallTile = true;
                    isOnBigTile = false;
                }
            }
        }
        else
        {
            // Player is not on any tile
            isOnBigTile = false;
            isOnSmallTile = false;
        }
    }
}