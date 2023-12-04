# SHIPS
## Open source Unity scripts for a space ship creation game

![alt text](https://arthur-detaille.fr/res/imgs/OpenShipsMaker.png)

### Disclaimer & Introduction
**Disclaimer** : Obviously the overall architecture (as set in the unity's inspector) is not preserved here. I may do it in the future, I don't really know. The goal here was to make the project public and comprehensible for a trained audience.
I think this project reflect my way of doing things (programmaticaly speaking).
To report any issue you may see in the code, please go into the *Issues* tab on github where I will look that up (and thank you!!)

I am sorry for the french/english comments in the code. This may be unreadable for non-french speakers.... I am sorry :/

**Introduction** : This is thought as an Open source project. This is made to be used, modified, forked, and build upon. Please feel free to do so. The most interesting parts (I think) are the `SaveSystem` and the `GameEditor` system. **I only push code that is error-free** on compile.

**Dependencies** : Scripts sometimes uses the free library *LeanTween* wich can be found on the Unity MarketPlace.

### Contents
1. SaveSystem
2. GameEditor System
3. User Interface
4. Parts
5. Procedural Parts Geometry System

### 1. Save System

To understand the save system, one needs to comprehend the architecture of ships in the game. Every part (whether it's a cube, a thruster, etc.) is a child of an anchor, which is itself a child of a part (*PlacedPart -> Anchor -> PlacedPart -> ... -> PlacedPart*). The anchor code can be found in the `Parts/Anchor.cs` script. A part may have zero, one, or many anchors. Each anchor can be empty (i.e., no parts are attached) or full, containing at most ONE part. Parts attached to anchors are called `PlacedPart` and can be found under the `Parts/PlacedPart.cs` script. Some parts can derive from `PlacedPart` (e.g., 'The Core' which is the central piece present in the scene at the start, is derived from it. Devices that are activable `PlacedParts` are also derived from it).

The save system revolves around two scripts: `Parts/Anchor.cs` and `SaveSystem/SaveSystem.cs`, which I encourage you to explore. More precisely, it is constructed from two functions: `BuildPartFromString(...)` and `ParseAnchorContentFromString(...)`, respectively.


### 2. GameEditor System
TODO

### 3. User Interface
TODO

### 4. Parts
TODO

### 5. Procedural Parts Geometry System
TODO

