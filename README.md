# 2dGalacticShooter-GameDevHQ
Game I created and version controlling for a Certification as per the Game Developer requirements at GameDevHQ

Once Done:
  Send an Email as per the requirements at the end of the 2d shooter learning course.
  github link in the email to your project for review
  
Requirements:
  Create a Github Account: Done
  Finish the features listed in Phase One, FrameWork: Attempting
  Finish the features listed in Phase two, Core Programming: Not Done 

What to do:
After each feature is done, commit it on Github.
Once commited, Go to ReadMe and write "Commited" next to the feature.
Note To self is just the programmer talking to himself.

Phase one, Framework:

1#  Thrusters: Commited
    Move the player at an increased rate when the ‘Left Shift’ key is pressed down
    Reset back to normal speed when the ‘LeftShift’ key is released

2#  Shield Strength: Commited
    Visualize the strength of the shield. This can be done through UI on screen or color changing of the shield.
    Allow for 3 hits on the shield to accommodate visualization
    Note to self: Change Color to yellow then Red.

3#  Ammo Count: Commited
    Limit the lasers fired by the player to only 15 shots.
    When the player is out of ammo, provide feedback through on-screen elements or sound effects.(ie:beep or ammo count displayed on screen)
      Note to self: Reverse the PowerUp Clip

4#  Ammo Collectable: Commited
    Create a powerup that refills the ammo count allowing the player to fire again
      Note to self: Sprite This somehow

5#  Health Collectable: Commited
    Create a health collectable that heals the player by 1. Update the visuals of the Player to reflect this.
    Note to self: Add Another set of 3 lives UI to display up to 6 lives.
    
6#  Secondary Fire Powerup: Commited
    Create a new form of projectile. You should already have a tripleshot. Include something new from multi direction shot, to heat seeking shots, etc.
    Replaces the standard fire for 5 seconds.
    Spawns rarely
    Note to self: Create Heat Seaking Shots, sounds more Fun. Maybe they got a slow rotation?
      Also add a laser that covers everything on front of player.
    
7#  Thruster: Scaling Bar HUD: Commited
    Create a UI element to visualize the charge element of your thrusters.
    Cool Down System required.
    Note to self: A green bar that holds a max of 5 seconds of thrusters.
    
8#  Camera Shake: Commited
    When the player takes damage, provide a subtle camera shake.


Phase Two, Core Programming:

1#  New Enemy Movement: Commited
    Enable the Enemies to move in a new way, either from side to side, circling, or coming into the play field at an angle.
    
2#  Player Ammo: Commited
    Visualize on screen the ammo count of the player in the form of current/max.
    
3#  Wave System: Commited
    Implement wave sequencing of enemies with more enemies coming each wave.
    
4#  Negative Pickup: Commited
    Create a powerup that negatively affects the player.
    
5#  New Enemy Type: Commited
    Create enemy types that can fire & damage the player
    Create enemy type with unique projectile.(ie. Laser beam, heat seeking, etc)
    Unique Movement Behavior (zig-zag,wave, etc)
    
6#  Balanced Spawning: Commited
    Create a balanced spawning system between Enemies & pickups
    Ie. Pickups like Health should be rare, where ammo is frequent
    
7#  Enemy Shields: Commited
    Provide logic for some enemies to have shields
    Shields allow the enemies to take 1 free hit.
    
8#  Aggressive Enemy Type: Commited
    Create the functionality to support enemy aggression
    If an enemy is close to a player, the enemy will try and “ram” it.
    
9#  Smart Enemy: Commited
    Create an enemy type that knows when it’s behind the player, and fires a weapon backwards.
    
10#  Enemy Pickups: Commited
     If a pickup is in front of an enemy, the enemy will fire its weapon at the pickup to destroy it before the player can get it.
     
11#  Pickup Collect: Not Done
     When the ‘C’ key is pressed by the Player, Pickups quickly move to the player.
     
12#  Enemy Avoid Shot: Not Done
     Create an enemy type that can avoid the player’s weapon
     When you fire a shot, the enemy should detect a shot in range and try to avoid it.
     
13#  Homing Projectile: Not Done
     Create a homing projectile that seeks the closest target.
     Turn into a rare powerup
     
14#  Boss AI: Not Done
     Create a final wave that includes a boss at the end.
     Moves down the screen to the center and stays there.
     Unique attacks towards the player.
     
