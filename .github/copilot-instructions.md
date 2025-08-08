# GitHub Copilot Instructions for "Centralized Climate Control (Continued)" Mod

## Mod Overview and Purpose

The "Centralized Climate Control (Continued)" mod for RimWorld introduces a network-like air system that enables centralized air cooling and heating, similar to electrical systems. This system allows players to efficiently manage the temperature of various rooms through a series of air pipes and climate control units, without needing individual heaters and coolers for each room.

## Key Features and Systems

1. **Unified Heating/Cooling System**: A single unit handles both heating and cooling, reducing the need for multiple appliances like heaters and coolers.
  
2. **Remote Heat Exhaust**: Climate Control units have exhausts located away from rooms, making it possible to build adjacent rooms to fridge rooms without temperature disruption.

3. **Air Network**: A complete network system akin to electrical systems, focusing on:
   - **Flow Efficiency**: Determines how well rooms maintain their temperature.
   - **Thermal Efficiency**: Governs the speed at which climate units respond to temperature changes like cold snaps.
   
4. **Airpipes and Network Operations**: Three types of colored pipes that can carry any air type and allow for pipe switching. The mod supports complex pipe networks and climate control units.

5. **Temperature Management**: The mod accounts for air temperature inside pipes, enabling networked temperature control without individual climate control units.

6. **Expansion for Late-Game**: Features larger buildings that offer significantly greater power.

## Coding Patterns and Conventions

- Utilize classes that extend from `ThingComp` for components that manage air flow (`CompAirFlow`), consumers (`CompAirFlowConsumer`), producers (`CompAirFlowProducer`), etc.
  
- Use consistent methods for registering and deregistering components that interact with the Air Network, such as within `AirFlowNetManager`.

- Follow standard practices for component-based design within RimWorld, ensuring that every `Comp` has a reset method for flow variables.

## XML Integration

- XML files define new buildings and their properties within the game, allowing for the custom integration of pipes, vents, and control units.
  
- XML configurations integrate with C# classes by referencing component properties and handling the visualization within the RimWorld game environment.

## Harmony Patching

- If compatibility concerns or game behavior adjustments are needed, employ Harmony patches. Patching should aim to gently adjust behavior without causing conflicts.
  
- Since this mod supports a unique air network, consider using Harmony to adjust the base game's temperature or network mechanics subtly if required.

## Suggestions for Copilot

1. **Class Stub Generation**: Use Copilot to assist in generating class stubs for additional components or system expansions.

2. **Method Suggestions**: Employ Copilot to suggest possible method implementations based on other `Comp` classes in this project, especially for dealing with network management or efficiency calculations.

3. **Optimization Patterns**: Leverage Copilot to explore optimization strategies for climate control calculation methods, ensuring minimal impact on game performance.

4. **Debugging Aids**: Draft debug methods and logs with Copilot guidance to efficiently track and solve issues that may arise in complex pipe networks.

5. **Documentation Assistance**: Utilize Copilot to help draft comments and documentation within the codebase, making future maintenance or handover easier.

6. **Error Handling**: Incorporate error handling and exception management strategies for robust network operations, guided by Copilot for standard practices.

By following these structured guidelines, continuous improvement and maintenance of the mod can be more manageable, ensuring the "Centralized Climate Control (Continued)" remains a valuable addition to the RimWorld modding community.
