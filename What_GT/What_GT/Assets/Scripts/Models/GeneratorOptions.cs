using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class GeneratorOptions
{
    public int width;
    public int height;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;

    public List<List<float>> Generate()
    {
        return GeneratorUtils.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);
    }
}
