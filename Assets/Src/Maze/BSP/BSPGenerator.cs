using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BSPGenerator : Generator
{

    private bool isCheeseArea(int x, int y)
    {
        return x > 90 && y > 90;
    }

    private bool isSpawnArea(int x, int y)
    {
        return x < 10 && y < 10;
    }

    private bool isEdge(int x, int y, int size)
    {
        return x == 0 || y == 0 || x == size - 1 || y == size - 1;
    }

    public override Vector2Int GenerateCheese(Maze maze, Vector3[] playerPositions, Vector2Int? currentCheese)
    {
        return new Vector2Int(95, 95);
    }

        private List<Particle> BreakDown(List<Particle> breakdown) {
        List<Particle> breakdownList = new List<Particle>();
        foreach (Particle particle in breakdown) { 
            if (Math.Abs(particle.x0 - particle.x1) < 4) {
                breakdownList.Add(particle);
                continue;
            };
            int x = UnityEngine.Random.Range(particle.x0, particle.x1);
            breakdownList.Add(new Particle(particle.x0, x, particle.y0, particle.y1));
            breakdownList.Add(new Particle(x, particle.x1, particle.y0, particle.y1));
        }

        List<Particle> breakdownListFinal = new List<Particle>();
        foreach (Particle particle in breakdownList) {
            if (Math.Abs(particle.y0 - particle.y1) < 4) {
                breakdownListFinal.Add(particle);
                continue;
            }
            int y = UnityEngine.Random.Range(particle.y0, particle.y1);
            breakdownListFinal.Add(new Particle(particle.x0, particle.x1, particle.y0, y));
            breakdownListFinal.Add(new Particle(particle.x0, particle.x1, y, particle.y1));
        }
        return breakdownListFinal;
    }

    private void SliceMaze(Maze maze) {
        var particles = new List<Particle>() {
            new Particle(0, maze.Size, 0, maze.Size)
        };

        while (particles.Count < 1000) {
            particles = BreakDown(particles);
        }
        
        for(int i = 0; i < particles.Count; i++) {
            if (i%2 == 0) continue;
            var particle = particles[i];
            for (int x = particle.x0; x < particle.x1; x++)
            {
                for (int y = particle.y0; y < particle.y1; y++)
                {
                    maze.Set(x, y, true);
                }
            }
        }
    }


    public override Maze GenerateMaze(int size)
    {
        var maze = new Maze(size);
        // BSP algorithm

        SliceMaze(maze);



        // End of BSP algorithm


        // DO NOT CHANGE
        // Set walls and free spawn and cheese areas
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (isEdge(x, y, size))
                {
                    maze.Set(x, y, true);
                    continue;
                }
                if (isSpawnArea(x, y) || isCheeseArea(x, y))
                {
                    maze.Set(x, y, false);
                    continue;
                }
            }
        }

        return maze;
    }
}
