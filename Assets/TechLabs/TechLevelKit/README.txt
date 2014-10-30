These models are designed to lock together using the vertex snap feature.

All models should import at the correct scale, and have lightmap UV 
coordinates already baked in.

The ScriptedPrefabs folder contains gates that Open and Close in response
to "Open" and "Close" messages / function calls. The speed and distance of
the doors can be configured in the GateControl.cs script. There are also
Fans with controllable spin speeds.

To use audio with the gates, add an AudioSource component to the gate, and
assign soundfxOpen and soundfxClose variables with an AudioClip.

These prefabs can be viewed and modifed in the respective Scene in the 
Scenes folder.


