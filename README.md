# PaperHumanParticleSystem (Work in Progress)

An advanced Unity component designed for creating a dynamic paper human effect using a **Particle System** tied to a **Skinned Mesh Renderer**. This script ensures particles follow the mesh during animations, with support for detachment, reformation, and fluttering effects. The system is optimized to follow animated skinned meshes, making it suitable for complex animations and dynamic gameplay scenarios.

---

## Features

- **Dynamic Particle Reformation**: Particles seamlessly reform into the human shape based on skinned mesh vertices.
- **Animation Support**: Particles dynamically follow the skinned mesh during animations, maintaining the human form.
- **Fluttering Motion**: Adds a fluttering effect to particles for a more dynamic visual experience.
- **Detachment and Reformation**:
  - Detach particles outward randomly.
  - Reform particles to follow the animated mesh.
- **Scalable Particle Count**: Supports up to 200,000 particles (configurable in the Inspector).
- **World Space Simulation**: Ensures particles move with the parent object while maintaining proper alignment to the skinned mesh.

---

## Usage Instructions

### 1. Attach the Script

Attach the `PaperHumanParticleSystem` script to a GameObject with a **Particle System** component.

### 2. Assign the Skinned Mesh Renderer

In the Inspector:
- Drag the **Skinned Mesh Renderer** of your character model into the `skinnedMeshRenderer` field.

### 3. Configure Settings

- **maxParticles**: Set the desired number of particles (default: 20,000).
- **detachSpeed**: Set the speed at which particles move outward when detached.
- **returnSpeed**: Set the speed of particle reformation.
- **flutterSpeed**: Control the speed of the fluttering effect.
- **flutterAmplitude**: Adjust the size of the fluttering motion.

---

## Input Controls

- **`Space` Key**: Detach particles outward.
- **`R` Key**: Reform particles to the skinned mesh.

---

## Advanced Features (Planned Enhancements)

- **Animation Sync**:
  - The system dynamically updates particle positions to follow the skinned mesh during animations.
  - Particles will maintain their formation even as the mesh moves, rotates, or deforms.

- **Enhanced Detachment**:
  - Detachment will account for real-time forces, such as wind or gravity.

- **Optimized GPU Performance**:
  - Future versions will explore GPU-based particle systems for handling millions of particles efficiently.

---

## Example Use Case

1. Attach the script to a Particle System GameObject.
2. Set up a character model with a **Skinned Mesh Renderer** (e.g., a humanoid rig).
3. Configure particle settings in the Inspector.
4. Play the scene and use the controls to detach and reform particles dynamically.

---

## Known Issues (Work in Progress)

- Particles might lag with extremely high counts on older hardware.
- Advanced synchronization with complex animations is still being optimized.
- Performance drops may occur if the **maxParticles** value exceeds reasonable limits for the target device.

---

## Troubleshooting

### Particles Don’t Follow Animations

- Ensure the **Particle System Simulation Space** is set to `World`.
- Verify that the skinned mesh is properly animated and the script is assigned to the correct mesh.

### Performance Issues

- Reduce the `maxParticles` value in the Inspector.
- Optimize particle materials and rendering settings in the **Particle System Renderer**.
- Consider using LOD (Level of Detail) techniques for lower-end devices.

### Detachment or Reformation Not Working

- Check input mappings for `Space` and `R` keys.
- Ensure the parent object and skinned mesh are properly assigned in the Inspector.

---
[Watch the Demo Video](video/demo.mp4)



## Contributions

This component is a work in progress. Contributions, suggestions, and feedback are welcome to refine and enhance its capabilities.

---

For any questions or issues, feel free to contact the developer or contribute directly to the project repository.

