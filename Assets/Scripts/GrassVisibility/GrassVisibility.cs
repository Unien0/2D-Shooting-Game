using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassVisibility : MonoBehaviour
{
    public Tilemap grassTilemap;
    public Tile transparentTile;
    public Tile replacementTile; // 用于替换的 Tile

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ReplaceTileAtPlayerPosition();
            Debug.Log("替换Tile");
        }
    }

    private void ReplaceTileAtPlayerPosition()
    {
        Vector3Int playerTilePosition = grassTilemap.WorldToCell(transform.position);

        if (grassTilemap.GetTile(playerTilePosition) == transparentTile)
        {
            // 将半透明 Tile 替换为新的 Tile
            grassTilemap.SetTile(playerTilePosition, replacementTile);
            Debug.Log("替换成功");
        }
        else
        {
            Debug.Log("替换失败");
        }
    }

    //旧方法：修改Tilemap颜色
    //private Color originalColor;

    //private void Start()
    //{
    //    originalColor = grassTilemap.GetComponent<TilemapRenderer>().material.color;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        SetTilemapTransparency(0.5f);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        SetTilemapTransparency(1f);
    //    }
    //}

    //private void SetTilemapTransparency(float alpha)
    //{
    //    if (grassTilemap != null)
    //    {
    //        Material material = grassTilemap.GetComponent<TilemapRenderer>().material;
    //        Color currentColor = material.color;
    //        material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        SetTileToTransparentAtPlayerPosition();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        RestoreTileOpacityAtPlayerPosition();
    //    }
    //}

    //private void SetTileToTransparentAtPlayerPosition()
    //{
    //    Vector3Int playerTilePosition = grassTilemap.WorldToCell(transform.position);

    //    if (grassTilemap.GetTile(playerTilePosition) != null)
    //    {
    //        grassTilemap.SetTile(playerTilePosition, transparentTile);
    //    }
    //}

    //private void RestoreTileOpacityAtPlayerPosition()
    //{
    //    Vector3Int playerTilePosition = grassTilemap.WorldToCell(transform.position);

    //    if (grassTilemap.GetTile(playerTilePosition) == transparentTile)
    //    {
    //        grassTilemap.SetTile(playerTilePosition, null);
    //    }
    //}

}
