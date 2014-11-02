           __                                       
  ___ ___ /\_\   __  _    __      ___ ___     ___   
/' __` __`\/\ \ /\ \/'\ /'__`\  /' __` __`\  / __`\ 
/\ \/\ \/\ \ \ \\/>  <//\ \L\.\_/\ \/\ \/\ \/\ \L\ \
\ \_\ \_\ \_\ \_\/\_/\_\ \__/.\_\ \_\ \_\ \_\ \____/
 \/_/\/_/\/_/\/_/\//\/_/\/__/\/_/\/_/\/_/\/_/\/___/ 

===============================================================
QUICK START GUIDE.
=============================================================== 
 
0. Create a new scene or use your current scene.

1. Drag in the Player prefab

2. Play!

===============================================================
Follow the steps listed below to set up your Motion Pack In Unity3d.
===============================================================

0. Uncompress and Import the downloaded assets .zip file and the script .zip file in your project. Make sure that you have your original character.fbx in the same folder where all the character@motion.fbx files are.

1. Create an Empty GameObject that will act as the master node for the character or NPC

2. Place the character (mesh with skeleton and animations applied) into the new Empty GameObject. (Thus making it a child of the Empty GameObject). Be sure to zero out the position so it is centered in the empty game object.

3. Add the AnimationStateMachine.cs script to the Empty GameObject.

4. Set the Target (field of AnimationStateMachine) to be the character placed inside the Empty GameObject.

5. Set the Graph Text Asset (field of AnimationStateMachine) to be the JSON .txt (motion_pack.txt) file containing all the state information.

6. Add a CharacterController to the Empty GameObject.

7. Set it up to line up with the character's feet.

8. Add the provided controller or your own custom controller.

9. Set Root Motion Mode to Manual in the Animation State Machine script.

10. Play!

==============================================================

WANT A VIDEO WALKTHROUGH?

Watch all our video tutorials here:

http://www.mixamo.com/c/tutorials#motionpacks

==============================================================

Still Stuck?!

Need Help?!   
    
E-mail Us!

Support@Mixamo.com
