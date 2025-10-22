
using System;
using UnityEngine;

public class GameGrid
{
    private Color[] pixelBuffer;
    private LifeSeeder seeder;

    public RenderTexture StateA { get; private set; }
    public RenderTexture StateB { get; private set; }

    private Texture2D cpuTex;

    public int Width { get; }
    public int Height { get; }

    public static readonly Color AliveColor = new Color(1f, 1f, 1f, 1f);
    public static readonly Color DeadColor = new Color(0f, 0f, 0f, 1f);

    private float fillProbability;

    public GameGrid(int width, int height, float fillProbability)
    {
        Width = width;
        Height = height;
        this.fillProbability = fillProbability;

        pixelBuffer = new Color[height * width];
        seeder = new LifeSeeder(this);

        cpuTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        cpuTex.filterMode = FilterMode.Point;

        StateA = CreateRenderTexture(width, height);
        StateB = CreateRenderTexture(width, height);
    }

    private RenderTexture CreateRenderTexture(int w, int h)
    {
        RenderTexture rt = new RenderTexture(w, h, 0, RenderTextureFormat.RFloat);
        rt.enableRandomWrite = true;
        rt.filterMode = FilterMode.Point;
        rt.Create();
        return rt;
    }

    public void Randomize()
    {
        seeder.RandomizeParallel(pixelBuffer);
    }

    public void Reset()
    {
        Array.Fill(pixelBuffer, DeadColor);
    }

    public void SetCell(int x, int y, bool state)
    {
        Color color = state ? AliveColor : DeadColor;
        int i = y * Width + x;
        pixelBuffer[i] = color;
    }

    public void SyncToRenderTexture(RenderTexture frame)
    {
        cpuTex.SetPixels(pixelBuffer);
        cpuTex.Apply();

        Graphics.Blit(cpuTex, frame);
    }

    public void SyncFromRenderTexture(RenderTexture frame)
    {
        RenderTexture oldRT = RenderTexture.active;
        RenderTexture.active = frame;

        cpuTex.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
        cpuTex.Apply();
        pixelBuffer = cpuTex.GetPixels();

        RenderTexture.active = oldRT;
    }

    public Texture2D GetTexture()
    {
        cpuTex.SetPixels(pixelBuffer);
        cpuTex.Apply();
        return cpuTex;
    }
}
