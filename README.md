# Benchmarking Script for Unity

## Status : Not operational yet

### Description
The point of this script is to load a scecific scene noumerus times and tryout different settings
in order to bechmark the performance on a specific device

### Usage
- Add the script to an empty object of an empty scene
- Add the name of the scene to test
- Deply on device and run
- Retrieve test

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

### Licence
This project is under the MIT licence