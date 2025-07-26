# GitHub Copilot Instructions for RimWorld Mod: Centralized Climate Control

## Mod Overview and Purpose

The "Centralized Climate Control" mod for RimWorld is designed to enhance the in-game environment management systems by introducing an advanced climate control network. The mod allows players to manage air flow for heating, cooling, and air ventilation more efficiently. It introduces a variety of new components and mechanics to create a robust air management system in the player's colony.

## Key Features and Systems

1. **Air Flow Networks:**
   - Manage complex networks of air pipes and vents with different air flow types like hot, cold, and frozen. 
   - Use producers, consumers, and controllers for maintaining optimal conditions.

2. **Temperature Management:**
   - Control room temperatures with air flow devices and manage climate conditions in various sections of a colony.

3. **Utility Buildings:**
   - New building types like `Building_AirPipe`, `Building_AirThermal`, and `Building_IntakeFan` enhance the control and influence of air flow.

4. **Interactive Components:**
   - Components such as `CompAirFlowProducer`, `CompAirFlowConsumer`, and `CompAirFlowTempControl` ensure dynamic interactions within the air flow network.

## Coding Patterns and Conventions

- **Naming Conventions:**
  - Classes and Methods use PascalCase.
  - Variables and parameters use camelCase.

- **Structure:**
  - Use well-defined methods to separate logical functionality (e.g., `TickProducers()`, `TickConsumers()`).
  - Ensure private methods are used when the functionality is meant to be encapsulated.

- **Comments:**
  - Use XML documentation for methods to describe their purpose and parameters.
  - Inline comments for complex or non-obvious logic.

## XML Integration

The mod relies on XML files to define the game's content and assets:
- **Defining New Objects:**
  - Use XML to create new objects, buildings, and components that the mod scripts will reference and manipulate.
  - Ensure there is a corresponding XML entry for every C# class expected to have a game object representation.

- **Resource Files:**
  - Make sure all graphic files referenced in XML are correctly loaded using the `GraphicsLoader` class and are bundled within the mod's content folder.

## Harmony Patching

Harmony is used to modify or extend the game’s functionality without altering the base game DLL:
- **Patch Annotations:**
  - Use `[HarmonyPatch]` to annotate methods that will be patched.
  - Ensure sufficient pre- and postfix conditions are checked to prevent unintended side effects.

- **Dynamic Method Changes:**
  - Alter methods such as `AirFlowNetTick` within their own context, avoiding changes to wider game mechanics unless necessary.

## Suggestions for Copilot

When using GitHub Copilot, consider the following suggestions to maximize its utility:

1. **Class and Method Templates:**
   - Generate templates for new building or component classes using existing patterns (e.g., `Building_`, `CompAirFlow`).

2. **Helper Methods:**
   - Suggest implementations for helper methods when handling repetitive tasks, like temperature checks or grid scanning.

3. **Optimization Patterns:**
   - Use suggestions to optimize loops, especially within methods like `TickProducers()` and `EnqueueNeighborCells()`.

4. **Error Handling:**
   - Prompt for robust error handling for null checks and unexpected states within network management and component functions.

5. **Consistent Documentation:**
   - Consistently fill out method templates with XML documentation to maintain readability and usability by other developers.

This mod enhances the gameplay experience in RimWorld through thoughtful system integration and detailed mod development practices. By following these guidelines, you can maintain and expand the mod effectively.
