# Save System with Save/Load Slot
 
## Save system with save/load slot

Depending on which button clicked, the DataPersistenceManager instance will handle any data saving/loading to a directories with profile id containing a JSON file called STES.game. The file will be placed in C:\Users\Lenovo\AppData\LocalLow\DefaultCompany\SaveSystem

### How it works
#### Save
DataPersistenceManager instance will take the current game data i.e. level, and pass it to the file data handler along with the directory profile id. The file will be written first to the memory to compare it with the available space in disk, if the data is less than the available disk space, then the data will be serialized in a JSON file called STES.game and saved.

#### Load
DataPersistenceManager instance will go to the persistent data path and find the directory with the matching profile id, and load the game data from the file to the game. 