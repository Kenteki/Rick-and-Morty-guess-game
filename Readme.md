# Welcome to my Rick and Morty guess game on C#
## About what this game?
This game was created for the Itransition internship task.
The principe of the game is simple.

Morty (controlled by Computer) hides the portal gun in one of **A** boxes.  
The box is selected using provably fair, collaborative, cryptographically secure random generation.  
We, as Rick, try to find the portal gun by selecting one of the boxes.

Morty has different implementations with **unique** behaviors
These behaviors are configurable, you can find it in [configuration of Morty](#configuration-of-morty) section.  
Different Morty implementations provide various strategies for removing boxes or leaving Rick with different choices.

## Summary

### Files description

**Core_Console.cs** Entry point for validation of arguments and files. After success, starts the game.  
**Folder /mods** Contains Morthy "implemintations". Details you can find in [configuration of Morty](#configuration-of-morty) section.  
**Folder /core**  
-**Engine.cs** Contains all main game logic and interactions with KeyManager and Morty implementations.    
-**IMorty.cs** Interface defining rules for Morty implementations.  
-**KeyManager.cs** Handles cryptographic key generation and HMAC calculations  
-**Statistics.cs** Displays Game Statistics with all results and calculated probabilities.  
**Folder /console:**
- **ArgumentValidation.cs** - Validates command-line arguments
- **FileValidation.cs** - Handles loading of Morty implementations from both .cs and .dll files

**Folder /game** Contains release version of the game.

### How the Portal Gun Location is Determined

The game uses a provably fair protocol where both participants contribute to the random selection:

We use the simple formula: `(M + R) % N`

- **N** = Number of boxes
- **M** = Morty's randomly chosen number using `RandomNumberGenerator.GetInt32(N)`, where M ∈ [0,N)
- **R** = User's chosen number

The result determines which box contains the portal gun.

**Example:**
- Morty generated number: 3
- User chooses number: 2
- Number of boxes (N): 4
- Formula: (3 + 2) % 4 = 1
- So the portal gun is in box 1 (in array {0,1,2,3})


### Key Generation

Keys are created using the standard .NET `System.Security.Cryptography.RandomNumberGenerator`. Each key is 32 bytes (256 bits) long. After generation, we create HMAC signatures using the third-party BouncyCastle library.

**Q: Why use a third-party library when .NET 9.0 supports SHA3?**

**A:** While .NET 8+ started supporting SHA3, it's not available on every system. It requires Windows 11 or Linux with OpenSSL 1.1.1+. To support older systems, we use BouncyCastle to generate hash signatures.

This process ensures provably fair, collaborative, cryptographically secure random generation.

### Statistics

All probabilities are calculated using this formula:
**P = A / B**
- **A** = Number of wins when switching/staying
- **B** = Number of times switching/staying was chosen

The estimated probabilities show your actual performance, while exact probabilities show the theoretical expected values based on the Morty implementation's behavior.


## Configuration of Morty

Morty implementations must follow the **IMorty** interface with these methods:

- **Name** - Display name (e.g., "Classic Morty", "Evil Morty", "Lazy Morty")
- **GetHMAC** - Retrieves cryptographic signature from KeyManager
- **ShouldRemoveBoxes** - Determines whether boxes should be removed (core game mechanic)
- **SelectBoxesToKeep** - Chooses which boxes remain after removal
- **CalculateProbability** - Returns theoretical win probability based on strategy

I implemented 3 versions of Morty  

- **Classic Morty**: Always removes boxes, uses random generation for box selection and switching gives `(N-1)/N` probability, staying gives `1/N`  
- **Lazy Morty**: Removes boxes 70% of the time using lowest index selection.  
- **Evil morty**: Never removes boxes, Forces User to stick with original choice and uses `1/N` for switching or staying  


## How to run my project?

To run the project type:  
`dotnet run N Morthy_Implementation`  
`N` amount of Boxes  
`Morhty_Implementation` name of file, you can use with/not extension.  
This build also supports to run both `.cs` and `.dll` files depends you start build or working from your IDE
Also program can receive as first argument `mods` `example` `github`.

For build version:
```bash
dotnet Rick_Morty_console_game.dll 3 ClassicMorty
```


### Requirements

This project requires **.NET 9.0**

To check your version:
```bash
dotnet --version
```

if you don't have it, you can istall from 
- Windows: Download from [Microsoft .NET Downloads](https://dotnet.microsoft.com/en-us/download)
- Linux: Follow instructions on [Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/core/install/linux)

## Libraries Used

- [BouncyCastle.Cryptography](https://github.com/bcgit/bc-csharp) - For SHA3-HMAC cryptographic operations
- [ConsoleTables](https://github.com/khalidabuhakmeh/ConsoleTables) - For formatted statistics display


## Video Demonstration

Video example of the program: [Streamable](https://streamable.com/236ft5)

