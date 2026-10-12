# While the Sakura Blooms

2D visual novel · Unity / C# · Capstone Project 2023–2024

An anime-style isekai visual novel with a cozy city setting, created by
Tsukimi Dango and inspired by East-Asian stories in the genre. Discover new
love and friendships while exploring a magical, foreign world. Will you get
all you desire? Or will it be too late?

**Status:** Playable, with known bugs. Not updated since 2024.

## Roles
This was a team capstone project. Julia was the sole programmer and wrote all of
the game's code. The story, artwork, and presentations were created by
Violet, Zaid, Dani, Lucy, and Ivy.

## Features
- Visual novel dialogue system
- Day and night cycle
- Different end states for the narrative
- Saving game progress

## A challenge we solved: testing the story without replaying it
Testing a branching story from the beginning every time was slow. I added a
scene variable in the code so we could jump to any point in the story by
editing the variable and launching the game from there. It was a practical
fix rather than an ideal one. With more time, I would build an in-game debug
menu for jumping between scenes.

## How we used AI
Our course did not allow AI to write the game's code, and all code was
written by Julia without AI. GPT was used only for smaller supporting tasks,
such as brainstorming the initial story concept.

## Credits

**Art references**
- Hospital Bed Scene: Studio Lab, https://sketchfab.com/3d-models/hospital-room-fd58a2f8e93447c18e52a5cb1c1e77dd
- Artist Dialogue Background: Natalie Zuchetti, https://nataliezuchetti.com.au/workshops/childrens-visual-art-classes/
- Library Dialogue Background: denniswoo1993, https://sketchfab.com/3d-models/classic-library-for-blender-eevee-and-cycles-aa6b6dc085344a009ccd02c9312c0128
- CEO Dialogue Background: Abu Shahariar, https://www.vecteezy.com/free-photos/ceo-office
- Castle Dialogue Background: Henan Tourism, https://www.flickr.com/photos/henantourism/
- Store Dialogue Background: Christian Wiediger
- Theme song "While the Sakura Blooms": commissioned from Alzha Studio

**Coding tutorials we learned from and adapted**
- Visual Novel System: Stellar Studio, https://www.youtube.com/watch?v=cO6NzrvTrkY&list=PLGSox0FgA5B58Ki4t4VqAPDycEpmkBd0i
- Teleport System: Rehope Games, https://www.youtube.com/watch?v=kswg8AsHFIs
- Day and Night Cycle: OB Game Dev, https://www.youtube.com/watch?v=F9NFSTOGYXg&t=294s
