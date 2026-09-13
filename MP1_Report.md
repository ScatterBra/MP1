# Open-Ended Design Contributions Summary

**Individual Contributor's Name:** Rundong He

**Contributions to:** CS 417 — MP1

**Team Number:** 1

**Team Members:** Rundong He (solo)

**Github Repository Link:** https://github.com/ScatterBra/MP1

**Itch.io Build Link:** [TODO — build not uploaded yet]

---

## Feature Stories Contributed

### Core Requirements

1. **Particle Bursts (2pt):** "BallSpawner.OnPerformed instantiates Assets/Prefabs/SpawnBurst.prefab at the exact position the ball is created, so the 30-particle burst is co-located with the spawned object." Timestamp: [TODO]

2. **Spatial Sound (2pt):** "SpawnBall.prefab carries an AudioSource with Spatial Blend = 1.0 playing Assets/SFX/ball.wav; BallSpawner calls Play() on the spawned instance, so the sound originates at the new object and follows it as it flies." Timestamp: [TODO]

3. **Object Space (1pt):** "Child Sphere is a child of Parent Sphere. PlanetSpin rotates the parent about Y, and the child is carried around it without a script of its own." Timestamp: [TODO]

4. **World Space (1pt):** "The Canvas GameObject uses Render Mode = World Space at world position (0, 7.5, 7.4) with a TextMeshPro child; it stays fixed in the room as the headset moves instead of following the view." Timestamp: [TODO]

5. **Materials (1pt):** "Custom materials are applied throughout the scene — WallMaterial1/2/3 on the walls, M_Floor on the floor, M_Celling on the ceiling, M_Parent and M_Child on the two spheres, and SpawnBallMaterial on every spawned ball." Timestamp: [TODO]

6. **XR Tracked Camera (2pt):** "OpenXR is enabled in XR Plug-in Management with the Oculus Touch interaction profile. The scene's camera is the Main Camera inside XR Origin (XR Rig), driven by a Tracked Pose Driver, and the demonstration video is recorded from the headset POV." Timestamp: [TODO]

7. **Euler Steady (2pt):** "PlanetSpin.Update calls transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime, Space.World), so rotation speed is framerate-independent. It drives Parent Sphere and all 16 office props, each at a randomized speed and direction." Timestamp: [TODO]

8. **Kinematic Double Integrators (2pt):** "CometOrbit.Step accumulates acceleration into a velocity field (velocity += acceleration * dt) and then integrates position from that velocity (transform.position += velocity * dt), giving the comet a semi-implicit Euler orbit rather than a scripted path." Timestamp: [TODO]

9. **XR Controller Inputs (1pt):** "Assets/MP1a.inputactions defines four actions bound to controller buttons — right A to CycleLightColor, right B to Quit, right trigger to Spawn, left X to Teleport — each consumed by a script through an InputActionReference." Timestamp: [TODO]

10. **Quit Key (1pt):** "Quit.cs subscribes to the Quit action and calls Application.Quit() in a build (UnityEditor.EditorApplication.isPlaying = false in the Editor), so pressing right B ends the session." Timestamp: [TODO]

11. **Object Spawning (1pt):** "Pressing the right trigger makes BallSpawner instantiate the prefab Assets/Prefabs/SpawnBall.prefab 0.3 m in front of the right controller." Timestamp: [TODO]

12. **Camera Teleport (3pt):** "ViewpointTeleport moves the XR Origin — and with it the tracked camera and both controllers — between three viewpoints (under the planet, the far room corner facing the centre, and the starting position), cycling one step per press of left X. The rig's CharacterController is disabled for the jump so it cannot pull the rig back." Timestamp: [TODO]

**Core Requirements subtotal:** 19 points

### Side Quests

1. **Object Shooter (2pt):** "SpawnedBody holds an internal velocity field that is added to transform.position every Update. BallSpawner sets that velocity to origin.forward * shootSpeed at spawn time, where origin is the right controller, so the ball flies in the direction the controller is pointing." Timestamp: [TODO]

2. **Arbitrary Orbiter (2pt):** "Both CometOrbit and SpawnedBody compute acceleration as -gravity * (position - attractor.position) / distance^3 — the inverse-square pull is measured from another object's position (Parent Sphere) rather than the world origin." Timestamp: [TODO]

3. **Perfect Orbits (2pt):** "BallSpawner.Launch strips the radial part of the controller aim (tangent = aim - Vector3.Dot(aim, radial) * radial) and sets the launch speed to Mathf.Sqrt(gravity / distance), producing a stable circular orbit. Measured in play mode, a launched ball held a radius of 6.521 m and a speed of 1.751 m/s over 5.7 seconds, matching the predicted sqrt(20 / 6.521)." Timestamp: [TODO]

4. **Skybox Material (1pt):** "Assets/Material/Skybox_sky13.mat (Skybox/6 Sided shader with the six sky13 textures) is assigned as the scene's skybox material." Timestamp: [TODO]

5. **Rainbow Lighting (1pt):** "LightSwitch steps Room Light through five colours — warm white, red, green, blue, yellow — one per press of right A." Timestamp: [TODO]

**Side Quests subtotal:** 8 points

### Content Stories

1. **Object Content — 16 objects (3pt):** "The room is decorated with 16 imported prop assets from the Ultimate Office Props pack: desk, chair, sofa, whiteboard, printer, laptop, display, keyboard, fan, hand cart, fire extinguisher, drinking fountain, coat hanger, cup, and two boxes. Each also carries a PlanetSpin component at a randomized speed and direction." Timestamp: [TODO]

2. **Material Content — 36 materials (4pt):** "36 distinct materials are applied to distinct objects across the scene: 7 authored for the room and spheres (WallMaterial1/2/3, M_Floor, M_Celling, M_Parent, M_Child), SpawnBallMaterial on spawned balls, and the remainder carried by the imported office props." Timestamp: [TODO]

3. **Particle Feedback Content — 4 feedbacks (1pt):** "Four particle emitters are triggered by user input: SpawnBurst at the spawned ball (right trigger), LightBurst tinted to the new light colour at the ceiling lamp (right A), and TeleportBurst at both the departure point and in front of the arrival point (left X)." Timestamp: [TODO]

4. **Spatial Sound Content — 4 feedbacks (1pt):** "Four spatialized audio generators (Spatial Blend = 1.0) are triggered by user input at distributed points in the room: ball.wav on the spawned ball in front of the controller, changeLight.wav at the ceiling lamp 13 m overhead, and teleport.wav at both the departure and arrival points of a teleport." Timestamp: [TODO]

**Content Stories subtotal:** 9 points

---

## Total

36 story points across Core Requirements (19), Side Quests (8), and Content Stories (9).

Solo submission — no team-size multiplier. Proposed grade: 36

---

## Known Limitations

* **Highlight Outline (2pt) not attempted:** no outline vertex shader is present in the project, so this story is not claimed.
* **Skybox visibility:** the room is fully enclosed by four walls, a floor, and a ceiling, so the skybox material may not be visible from any viewpoint inside the room during the demo video.
* **Object Shooter vs. Perfect Orbits:** both behaviours share one spawn button and are selected by the Perfect Orbit checkbox on BallSpawner, so the two stories are demonstrated in separate segments of the video rather than back to back.
* **Orbit radius depends on the spawn position:** launching from the starting viewpoint gives a radius of roughly 8.4 m, which reaches outside the 15 m room; the demo launches from the under-the-planet viewpoint, where the orbit stays inside the room.
* **Teleport feedback shares one input:** two of the four particle/sound feedbacks are triggered by the same teleport button (departure and arrival) rather than by four fully distinct inputs.
* **Spawned balls are never destroyed:** they orbit or fly indefinitely, so a long session accumulates objects.
* **Disabled duplicate rig left in the scene:** the original XR Origin (VR) and its camera remain in the hierarchy but are deactivated, so only one camera and one AudioListener are live.
