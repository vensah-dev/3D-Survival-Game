# a small survival game
I attempted to make a 3D survival game as my very first game on Unity with my prior experience being 3 years in Scratch. Took me 2 weeks to get a hand of C# but with all the free time in the holidays I was able to make some simple 3D procedural terrain generation following some tutorials. Eventually, I lost interest in it due to the project getting messy and because I had other projects to focus on.

## Features
- 1st person player controller
- infinite terrain generation with multithreading
- normal/heigh based splatmap terrain shaders
- falloff maps to create islands
- Foliage generators, supported for infinite terrain generation
- basic enemy AI: enemy will move around randomly until within a certain range and will attack player
- inventory system with a hot bar similar to minecraft
- item drops from folliage, enemies, etc...
- water

## Procedural Generation
My terrain generator uses a height map generated using layered Perlin noise that is normalised between (-1, 1) unless it is generating terrain infinitely. The height map is then used to displace the vertices of a plane that is generated with the dimensions as the height map with a height multiplier. The infinite terrain is generate in chunks, existing chunks are loaded from a dictionary. A similar system is used to generate trees on infinite terrain. The distribution of trees is based on the same noise map but offset with a threshold creating forests and another layer of trees using random noise. The trees that have been chopped down by the player will be updated on the dictionary. Copious amounts of fog effects are used to cover up the chunks loading. The chunks are loaded in parallel to avoid small lag spikes that are more apparent in lower end hardware.
