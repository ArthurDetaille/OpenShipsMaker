# SHIPS
## Open source Unity scripts for a space ship creation game
### Disclaimer & Introduction
**Disclaimer** : Obviously the overall architecture as set in the unity's inspector is not conserved here. I may do it in the future, I don't really know. The goal here was to make the project public and comprehensible for a trained audience. I think this project reflect my way of doing thigs programmaticaly speaking.
To report any issue you may see in the code please go into the Issues tab on github where I will look that up (and thank you!!)

**Introduction** : The most interesting part (I think) are the `SaveSystem` and the `GameEditor` system.

### Contents
1. SaveSystem
2. GameEditor System
3. User Interface
4. Part

### 1. Save System
```cs
public class SaveSystem : MonoBehaviour
```

### 2. GameEditor
```cs
public abstract class GameEditor : MonoBehaviour {...}
public class GameEditorManager : MonoBehaviour {...}
```
