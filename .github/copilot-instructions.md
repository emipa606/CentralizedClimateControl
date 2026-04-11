# GitHub Copilot Instructions for "Centralized Climate Control (Continued)" Mod

Welcome to the Copilot instruction file for the Centralized Climate Control (Continued) mod. Below, you'll find comprehensive guidance for understanding, maintaining, and expanding this mod using GitHub Copilot.

## Mod Overview and Purpose

**Centralized Climate Control (Continued)** is an update of the original mod by ColdToad, aimed at creating a centralized air cooling and heating system in RimWorld. The mod enables players to build extensive air climate systems that centralize heating and cooling away from buildings.

## Key Features and Systems

- **Unified Heating/Cooling System**: A single unit provides both heating and cooling, minimizing the need for separate coolers and heaters.
- **Remote Heat Exhaust**: Heat exhausts can be placed away from rooms, allowing adjacent fridges without heat interference.
- **Air Network System**: Functions like an electrical grid, the air network requires efficient management of flow and thermal efficiencies.
- **Air Temperature in Pipes**: The mod considers air temperature within the pipe network, affecting room temperature control.
- **Scalability for Late Game**: Larger intake and climate control units accommodate growing player needs.
- **Pipe Switches**: Allows forced connection to specific pipes near vents for better management.
- **Mod Settings Multipliers**: Airflow and air exhaust can be tuned with percentage sliders in mod settings.

## Coding Patterns and Conventions

- Follow the C# conventions for class and method naming.
- Use XML comments for documenting methods, especially public-facing APIs.
- Maintain consistent indentation and brace styles as per C# standards.
- Separate logic into manageable classes such as `AirFlowNet`, `AirFlowNetManager`, and their components.

## XML Integration

- XML definitions are likely used for defining in-game items, building properties, and other static data.
- Ensure XML files are correctly formatted with consistent tags and attributes.
- Maintain a straightforward mapping between XML definitions and C# classes (e.g., `CompProperties_AirFlow` should correspond to XML configuration of airflow components).

## Harmony Patching

- Utilize Harmony to patch methods responsibly for compatibility with other mods.
- Ensure patches are well-documented and revertible.
- Test patches thoroughly before deployment to avoid conflicts and ensure stability.

## Suggestions for Copilot

Here are specific areas where Copilot can be particularly useful in this project:

1. **Code Completion**: Use Copilot's suggestions to auto-complete routine method setups, especially for commonly repeated operations like network registration and deregistration.

2. **XML Schema Generation**: Leverage Copilot to help generate XML layout templates based on existing schema patterns in the codebase.

3. **Patch Writing**: For Harmony patches, Copilot can assist in suggesting possible base methods to patch and writing the wrapper methods needed.

4. **Debugging**: Utilize Copilot’s suggestions to insert standard debugging outputs or exception handling snippets.

5. **Examples and Documentation**: Use Copilot to create example use cases or document complex code segments for future developers.

## Maintenance and Contribution

This mod is currently not maintained due to life changes of the original author, but it operates under the MIT License, allowing the community to modify and improve it. Use the guide and video resources available to familiarize yourself with the basic setup.

For any errors encountered, test with the mod dependencies first and report issues on GitHub or Discord as outlined. Use RimSort to keep mod lists organized for stable gameplay.

**Thank you for supporting and contributing to the Centralized Climate Control mod!**

This .github/copilot-instructions.md file provides an extensive guide on the various aspects of the mod, including technical details and how GitHub Copilot can assist in development and maintenance. It is designed to aid in onboarding new developers and facilitating smooth transitions in maintaining and enhancing the mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The .github/copilot-instructions.md file is included in the solution under the .github solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution .github copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.

