Necromancy - Server
===
Server Emulator for the Online Game Wizardry Online.

## Table of contents
- [Disclaimer](#disclaimer)
- [Wiki](#wiki)
- [Setup](#setup)
  - [Visual Studio](#visual-studio)
  - [VS Code](#vs-code)
  - [IntelliJ Rider](#intellij-rider)
- [Project](#project)
  - [Common](#common)
  - [Data](#data)  
  - [Database](#database)
  - [Packet](#packet)
- [Guidelines](#guidelines)
- [Attribution](#attribution)
  - [Contributers](#contributers)
  - [3rd Parties and Libraries](#3rd-parties-and-libraries)

# Disclaimer
The project is intended for educational purpose only.

# Wiki
- Index: https://github.com/necromancyonline/necromancy-server/wiki
- ウィキペディア: https://github.com/necromancyonline/necromancy-server/wiki/%E5%85%A5%E9%96%80

Please check the wiki for additional information.

# Setup
## 1) Clone the repository  
`git clone https://github.com/necromancyonline/necromancy-server.git`

## 2) Install .Net Core 3.0 SDK or later  
https://dotnet.microsoft.com/download

## 3) Use your IDE of choice:

## 3.1) Visual Studio
### Notice:
Minimum version of "Visual Studio 2019 v16.3" or later.

### Open Project:
Open the `necromancy.sln`-file

## 3.2) VS Code
Download IDE: https://code.visualstudio.com/download  
C# Plugin: https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp  

### Open Project:
Open the Project Folder:  
`\necromancy-server`

## 3.3) IntelliJ Rider
https://www.jetbrains.com/rider/

### Open Project:  
Open the `necromancy.sln`-file

## 4) Debug the Project
Run the `Necromancy.Cli`-Project

# Project
### Ids
| Name       | Type       | Uniqueness | Constant | Description                                                                                             |
|:-----------|:-----------|:-----------|:---------|:--------------------------------------------------------------------------------------------------------|
| InstanceId | ObjectId   | Globally   | No       | Identifies a object uniquely globally (No duplicate Ids are possible, can distinguish between anything) |
| Id         | DatabaseId | per Domain | Yes      | Identifies a object uniquely among its domain. (Can distinguish between two npcs with the same NpcId)   |
| NpcId      | SerialId   | None       | Yes      | Identifies a type of npc (Multiple npc can exist with same NpcId)                                               |
| MonsterId  | SerialId   | None       | Yes      | Identifies a type of monster (Multiple monster can exist with same MonsterId)                                   |
| ModelId    | SerialId   | None       | Yes      | Identifies a type of model (Multiple model can exist with same ModelId)                                         |


## Common
## Data
## Database
## Packet

# Guidelines
## Git 
### Workflow
The work on this project should happen via `feature-branches`
   
Feature branches (or sometimes called topic branches) are used to develop new features for the upcoming or a distant future release. 
When starting development of a feature, the target release in which this feature will be incorporated may well be unknown at that point. 
The essence of a feature branch is that it exists as long as the feature is in development, 
but will eventually be merged back into develop (to definitely add the new feature to the upcoming release) or discarded (in case of a disappointing experiment).
   
1) Create a new `feature/feature-name` or `fix/bug-fix-name` branch from master
2) Push all your changes to that branch
3) Create a Pull Request to merge that branch into `master`

## Best Practise
- Do not use Console.WriteLine etc, use the specially designed logger.
- Own the Code: extract solutions, discard libraries.
- Annotate functions with documentation comments (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments).

## C# Coding Standards and Naming Conventions
| Object Name               | Notation    | Char Mask          | Underscores |
|:--------------------------|:------------|:-------------------|:------------|
| Class name                | PascalCase  | [A-z][0-9]         | No          |
| Constructor name          | PascalCase  | [A-z][0-9]         | No          |
| Method name               | PascalCase  | [A-z][0-9]         | No          |
| Method arguments          | camelCase   | [A-z][0-9]         | No          |
| Local variables           | camelCase   | [A-z][0-9]         | No          |
| Constants name            | PascalCase  | [A-z][0-9]         | No          |
| Field name                | _camelCase  | [A-z][0-9]         | Yes         |
| Properties name           | PascalCase  | [A-z][0-9]         | No          |
| Delegate name             | PascalCase  | [A-z]              | No          |
| Enum type name            | PascalCase  | [A-z]              | No          |

# Attribution
## Contributors
- Unknownone69 [@Unknownone69](https://github.com/Unknownone69) 
- WizOnHiraeth [@WizOnHiraeth](https://github.com/WizOnHiraeth) 
- blackice36 [@blackice36](https://github.com/blackice36) 
- Aidoxilia [@Aidoxilia](https://github.com/Aidoxilia) 
- taewyth [@taewyth](https://github.com/taewyth) 
- koffeeMist [@koffeeMist](https://github.com/koffeeMist) 
- WIZONMAGLOVE [@WIZONMAGLOVE](https://github.com/WIZONMAGLOVE) 
- Nothilvien [@sebastian-heinz](https://github.com/sebastian-heinz)

## 3rd Parties and Libraries
- System.Data.SQLite (https://system.data.sqlite.org/)
- MySqlConnector (https://www.nuget.org/packages/MySqlConnector)
- bcrypt.net (https://github.com/BcryptNet/bcrypt.net)
- .NET Standard (https://github.com/dotnet/standard)
- Arrowgene.Services (https://github.com/Arrowgene/Arrowgene.Services)

