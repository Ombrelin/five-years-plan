# Five Years Plan

<img src="logo/logo.svg" style="width: 200px"/>

Five Year Plan is an editor to plan you Satisfactory production lines. You can place buildings, set resources inputs flows, and the flows will be calculated automatically along your production lines, so that you can focus on optimizing it.

## Project state

This project is very early in this development. It should be usable for tier 0 and 1 planning.

Demo of current capabilities : 

![](images/five-years-plan.gif)

## Roadmap

- [x] Resource flow framework
- [x] Tier 0 and 1 buildings
  - [x] Splitter
  - [x] Merger
  - [x] Miner
  - [x] Smelter
  - [x] Constructor
  - [ ] Storage container
- [ ] Overclocking
- [ ] Power consumption calculation
- [ ] Nicer UI
  - [ ] Overall design
  - [ ] Buildings and resources icons
- [ ] App packaged as :
  - [ ] `.deb`
  - [ ] `.rpm`
  - [ ] Flatpak published to Flathub
  - [x] Windows installer
- [ ] Tier 2 buildings :
  - [ ] Assembler
- [ ] Tier 3 buildings :
  - [ ] Coal-Powered generator
  - [ ] Water extractor
  - [ ] Foundry
- [ ] Tier 5 buildings :
  - [ ] Oil extractor
  - [ ] Refinery
  - [ ] Packager
  - [ ] Fuel Generator
- [ ] Tier 6 buildings :
  - [ ] Manufacturer
- [ ] Tier 7 buildings :
  - [ ] Blender
- [ ] Tier 8 buildings :
  - [ ] Nuclear Power Plant
  - [ ] Resource Well
  - [ ] Particles accelerator
- [ ] Tier 9 buildings :
  - [ ] Converter
  - [ ] Quantum Encoder

Other Ideas : 

- Conveyor belt capacity requirements ?
- 

## Build & Run from sources

- Install .NET 8 SDK
- Clone the project
- `cd five-years-plan/src/FiveYearsPlan.UI`
- `dotnet run`

## Thanks

- App logo : 
  - <a target="_blank" href="https://icons8.com/icon/64036/blueprint">Blueprint</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>

## Technologies

- [.NET 7, C# 11](https://dotnet.microsoft.com/en-us/)
- [.NET MVVM Toolkit](https://github.com/CommunityToolkit/dotnet)
- [AvaloniaUI](https://avaloniaui.net/)
- [@wieslawsoltes's Node Editor for Avalonia](https://github.com/wieslawsoltes/NodeEditor)
