# Benchmarking Script for Unity

## Status : Working (Windows & Android)

### Description
The point of this script is to load a spcecific scene as many times as quality
pre-settings profiles exist. Then count the frames per seconds (not implemented yet)
and also take a screenshot. Finaly you will have a folder with all the screenshots
as well as a text file that contains some info about the benchmark.

### Usage
- Add the script to an empty object of an empty scene
- Add the name of the scene to test
- Deply both scenes to the target device and run
- Retrieve results from the appropriate folder
	- Windows : MyDocuments/UnityBench/
	- Linux   : /home/$USER/UnityBench/
	- Android : /sdcard/UnityBench/

### Ideas / Parts for contributions
Here is how I think it should work in it's final form
- Loops through all quality settings
- Opens the scene
- Counts the seconds it takes to do that
- Waits for x seconds for the scene to "settle"
- Counts FPS for a specific period time
- Maby takes a screenshoot too
- writes that info somewere
- Move on to another quality leve

#### Future Ideas
- GI settings
- Cam animation
- A Lot more options
- More VR friendly (in case of head tracking)

### Licence
This project is under the MIT licence
