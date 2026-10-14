# Unity-3D-Interaction-System
A versatile 3d Interaction system similar to games like Resident Evil 7

The player movement and camera controls are pretty simple and were made based on this Brackeys's tutorial: https://www.youtube.com/watch?v=_QajrabyTJc&t=860s
The InputManager is a simple script that handles player inputs and fires events based on those, and it ONLY WORKS with the New Input System of unity

# Interaction System

A reusable interaction system for Unity that allows players to detect and interact with objects in the game world. The system prioritizes interactable objects based on the player's camera direction and distance, checks for obstacles that block interaction, and communicates interaction states to UI elements through events.

# Requirements
Unity

Unity's New Input System

# C# Scripts
**IInteractable.cs:**
Provides a base interface for all interactable objects, exposes methods and events that can be used across many interactable object and class types

**InteractableObject.cs:**
Inherits from the IInteractable interface and acts as a base class for any interactable class type that the developer wants to make later, exposes events for interaction state changes, and provides a virtual Interact() method that can be overridden to implement custom behaviors.

**PlayerInteract.cs:**
Handles player's interaction detection and selection. It searches for nearby interactable objects, checks whether the player is looking toward them, uses raycasting to detect obstacles, and calculates a score based on distance and dot product to select the best interaction target. When the player triggers the interaction input, the selected interactable object will invoke the Interact() method.

**InteractableUI.cs:**
A UI handler script that sits on a world space UI and triggers the right animation when the interactable object is in range, not in range, or can currently be interacted with.

# Configuration
Each interacatable object needs to have the InteractableObject.cs or any MonoBehaviour class that inherits from the IInteractable interface, it's layer mask has to be set to a certain interactable object layer, and it needs to be included in PlayerInteract.cs's ConsiderableLayers and InteractablesLayerMask for the interaction to work properly.

The interaction system can be configured through the Unity Inspector:

**Check For Interactables Update Rate:** Controls how frequently nearby interactable objects are checked.

**Player Camera:** The camera transform used to determine the player's viewing direction and detection origin.

**Interact Radius:** Sets the maximum distance for detecting interactable objects.

**Min Interact Dot Product:** Controls how directly the player must look toward an object for it to qualify as a potential target.

**Distance Weight:** Controls how much proximity influences target selection.

**Dot Product Weight:** Controls how much camera alignment influences target selection.

**Considerable Layers:** Defines the layers considered during obstacle and target raycasting.

**Interactables Layer Mask:** Defines which layers are searched for interactable objects.

**Show Gizmos:** Toggles visualization of the interaction radius in the Scene view.

# Extending the System
To create a new interactable object, implement IInteractable or inherit from InteractableObject and override Interact() with your custom interaction logic.
For example, an interactable object could open a door, pick up an item, activate a switch, or display a message. The existing interaction detection and target-selection system can be reused without rewriting the player-side logic.
