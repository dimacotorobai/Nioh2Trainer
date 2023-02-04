## Overview
An external trainer for the game Nioh 2 written in C# with .NET Framework. This project is mostly complete with some minor changes/features that may be added in the future. This is a .NET Framework 4.8.0 WPF x64 project created using Visual Studio 2022.

## Features
<p align="center">
  <img src="https://user-images.githubusercontent.com/52585921/216781879-e29b528a-5fbe-4d6e-b791-cf0b73a074ac.png?raw=true" alt="Process"/>
  <img src="https://user-images.githubusercontent.com/52585921/216781896-b09e47a7-702f-4020-be6a-39ab15e9fa3f.png?raw=true" alt="Cheats"/>
  <img src="https://user-images.githubusercontent.com/52585921/216781903-af318de2-294a-4a19-a2a5-b8f644c26c95.png?raw=true" alt="Inventory"/>
  <img src="https://user-images.githubusercontent.com/52585921/216781917-92a47323-3799-4ec2-b211-f9782c5ba7a8.png?raw=true" alt="Stats"/>
</p>

## Instructions
To use this application the user must either compile the project themselves for x64 or download the executable provided. Also, the user must have .NET Framework 4.8.0 installed on their computer. Below I have provided step-by-step instructions on how to run the trainer.

### Step By Step
  * Download and install .NET Framework from https://dotnet.microsoft.com/en-us/download/dotnet-framework
  * Download the Nioh 2 Trainer executable from this repository(v2.0.0 targets .NET Framework 4.8.0)
  * Launch the Nioh 2 video game and start an in-game session
  * Once the character is in-game the user can launch the executable
  * Navigate to the Process Tab and click on Attach
  * Enjoy playing the game with the trainer :)
  

## Project Goal
My goal for this project was to create a trainer for the game Nioh 2 written in C# with a WPF GUI. I wanted to learn C# while applying what I learned and this seemed like an interesting project to do. This project also dives more into reverse engineering as I try to apply more tools such as Cheat Engine, ReClass, x64DBG, and Ghidra to get a better understanding of the inner workings of the game. The trainer also uses pattern scanning and function hooking instead of dynamic pointer arithmetic for all of the cheats. This was done to insure compatibility between game updates.

## Help
This project could not have been built without the excellent tutorials provided by GuidedHacking, CheatTheGame, StackOverflow, and Microsoft. The process of finding offsets, reverse engineering, and writing the code required for memory reading/writing was learned from GuidedHacking and CheatTheGame. While the process of writing a C# based tool was learned from StackOverflow and Microsoft.
