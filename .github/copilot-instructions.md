# GitHub Copilot Instructions for Centralized Climate Control (Continued) Mod

## Mod Overview and Purpose

The Centralized Climate Control (Continued) mod is an update to the ColdToads mod, introducing a unified heating and cooling system for the RimWorld game. This mod adds a network-like resource called Air, which allows players to build large piped air climate systems away from their buildings. The primary purpose of this mod is to provide a more efficient and flexible way to manage air temperature in the game.

## Key Features and Systems

### Unified Heating/Cooling System
- **Air Intake Fans**: Supply air to the system.
- **Air Climate Control Systems**: Heat or cool air to a target temperature.
- **Air Vents**: Vent out the heated/cooled air.
- **Wall Mounted Air Vents**: Alternative venting option.
- **Large Intakes and Air Climate Units**: Higher-capacity units for later game stages.

### Air Network
- **Flow Efficiency**: Rate at which rooms will cool/heat or maintain temperature due to changes in room temps.
- **Thermal Efficiency**: Rate at which Climate Control units can heat the air.

### Air Temperature inside Pipes
- Manages temperature of the air in the Pipe Network.

### Can share air without Climate Control units
- Intakes take note of the Air Temperature they take in, allowing for climate control-less networks that share the same temperature.

### Pipe Switches
- Allows forcing connection to a specific pipe.

## Coding Patterns and Conventions

### C# Coding Conventions
- **AirFlowNet.cs**: Contains the `AirFlowNet` class with methods for handling air flow, consumption, and production.
- **AirTypePriority.cs**: Defines priorities for different types of air.
- **AirFlowType.cs**: Defines types of air flow.
- **Building_AirFlowControl.cs**: Contains the `Building_AirFlowControl` class with `SpawnSetup` method.
- **Building_AirThermal.cs**: Contains the `Building_AirThermal` class with `SpawnSetup` and `TickRare` methods.

### XML Integration
- **Defs/DesignatorDropdownGroupDef/CCC_Group.xml**: Defines designator dropdown groups for the mod.
- **Defs/ResearchProjectDefs/ResearchProjects.xml**: Defines research projects.
- **Defs/ThingCategoryDefs/ThingCategories.xml**: Defines thing categories.
- **Defs/ThingDefs_Buildings/Buildings_Temperature.xml**: Defines building definitions related to temperature.
- **Defs/ThingDefs_Buildings/Buildings_Vents.xml**: Defines building definitions related to vents.

## Harmony Patching

The mod uses Harmony patching to integrate with existing RimWorld systems. Harmony patches are used to modify or extend the behavior of RimWorld's existing codebase.

### Example Harmony Patches
- **AirFlowNet.cs**: Contains patches for handling air flow.
- **Building_AirFlowControl.cs**: Contains patches for building-related logic.
- **Building_AirThermal.cs**: Contains patches for thermal-related logic.

## Suggestions for Copilot

### General Guidelines
- Follow RimWorld coding conventions and patterns.
- Ensure that any generated code is well-documented and easy to understand.
- Use XML and Harmony patching effectively to integrate with the game.

### Specific Suggestions
- **XML Generation**: When generating XML files, ensure that they follow the correct structure and conventions.
- **Harmony Patching**: When generating Harmony patches, ensure that they are correctly targeting the intended methods and that they do not introduce compatibility issues with other mods.
- **Code Quality**: Focus on generating high-quality, maintainable code that adheres to the coding patterns and conventions established in the mod.

By following these guidelines and suggestions, you can help ensure that the generated code is both effective and maintainable.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

