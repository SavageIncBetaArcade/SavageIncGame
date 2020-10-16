using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class RenderTexturePool : MonoBehaviour
{

    public class PoolItem
    {
        public RenderTexture Texture;
        public bool Used;
    }

    public static RenderTexturePool Instance;

    public int maxSize = 100;
    private List<PoolItem> pool = new List<PoolItem>();

    private void Start()
    {
        Instance = this;
    }
    public PoolItem GetTexture()
    {
        //check for unused item in pool, grab it, mark it as used and return it

        foreach(var poolItem in pool)
        {
            if(!poolItem.Used)
            {
                poolItem.Used = true;
                return poolItem;
            }
        }

        // all are in use ? make some more!

        if (pool.Count >= maxSize)
        {
            Debug.LogError("Pool is full!");
            throw new OverflowException();
        }

        var newPoolItem = CreateTexture();
        pool.Add(newPoolItem);
        Debug.Log($"New RenderTexture created, pool is now {pool.Count} items big.");
        newPoolItem.Used = true;
        return newPoolItem;
    }

    public void ReleaseTexture(PoolItem item)
    {
        item.Used = false;
    }

    public void ReleaseAllTextures()
    {
        foreach (var poolItem in pool)
        {
            ReleaseTexture(poolItem);
        }
    }

    private PoolItem CreateTexture()
    {
       
        var newTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
        newTexture.Create();

        return new PoolItem
        {
            Texture = newTexture,
            Used = false
        };
    }

    private void DestroyTexture(PoolItem item)
    {
        // release on the GPU...

        item.Texture.Release();

        // remove it from Unity.

        Destroy(item.Texture);
    }

    private void OnDestroy()
    {
        foreach (var poolItem in pool)
        {
            DestroyTexture(poolItem);
        }
    }
}
