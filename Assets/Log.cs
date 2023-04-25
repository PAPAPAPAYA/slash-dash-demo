// SLASH DASH DEMO v1.0
// 2023_3_6 - 2023_3_14
// 20.5h

//x PLAYER
    //x hold down mouse anywhere to show slash path
    //x move mouse while dragging to change slash path direction
    // xrelease mouse to slash
        // xdecide slashability
            // xdetect if object in slash path ? slash : don't slash
            // xchange slash path material based on slashability
        //x actual slash
            //x stun enemy hit
            //x move player along slash path
            //x rotate player while slashing
            //x do damage
                //x do damge
                //x particle effects
            //x enemy flash white
            //x screen shakes
    // xslash also moves player
    //x slash corpes
    //x slash bullets
    //x if slash successed, extend slash path
//x ENEMY
    // xshow hp realtime
    //x move
        //x baics
        //x avoid other enemies
    //x spawn
        //x speed up spawn rate gradually
            //x basics
        //x spawn different types of enemies
        //x spawn chance
        //? OBJECT POOLING
    //x die
        //x vfx
        //x leaves corpes behind
        //x corpes when slashed add score
    //x different types
        //x shooter
            //x spawn bullets
        //x brawler
        //x slimer
            //x spawn smaller enemies when slashed
        //? snaker
            //? a string of circles with an invincible head, only dies when all body parts are slashed
    //x hurt player
        //x deal damage
        //x knock back enemeis around player
        //x vfxs
            //x flash
            //x hit stop/bullet time
//x CAMERA
    //x camera follow player
    //x pixelated cam
//? POST PROCESSING
//x SYSTEM
    //x game over
    //x restart
    //x show score
    //x show player hp
//x DEBUG
    //x fix shadows
    //x bullets not getting knock back when player is hurt
    //x check count of the list of enemies in knock back area to determine viability, not ontriggerexit
    //x make aim area ui
//x TUNING
    //x spawner
        //x tune rate
    //x shooter
        //x move faster
        //x stay away from player


// SLASH DASH DEMO v2.0
// 2023_3_15

// TUNING
    //? implulse at destination
    //x cameara size change based on size of slash path
    //x make player rotation independent of frame rate
    //x add a key to switch to different modes
    //? enemy acceleration
        //? turn spd
        //? slow down when turning
    //x make scores less obvious
    //x make enemies face player only when player not moving
    // make enemy spawn circle bigger
    // use proper direction in shootoutcorpse()
// DEBUG
    //x fix restart
    //x player is constantly rotating
// FEATURES
    //x SLINGSHOT
        //x silhouettes
        //x charge
    // BULLET
        // consume bullet to slash
        // gaining bullet
    // NEW ENEMY TYPES
        // snaker
        //! REFLECTION
            //? area detection, no line detection, try circlecastall
            //x use one raycast to calculate refleciton
            //x show new slash path
            //x show new slash paths
            //x when dragging, hide paths that don't connect
            //x move player along all the paths
                //x don't include the object that the ray is coming from
                // x填一个list，这个list放target的不同targetpos，根据path的数量来
            //x destroy new paths when no reflection
            //x make relfeciton compatible with enemy boost
            //x and charging
            //x shield enemy
            // debug
                //x now only when all slash paths are valid then player can move
                //x when making the new path, scale difference is 0 since it hasn't begun to shrink
                //x reset slash path's scale when below 0, sometimes when it goes below 0 it keeps going
                //x check if shielded properly? sometimes enemies that are not shielded can reflect
                //x sometimes the order of the target poses are wrong: sometimes the new slash path appears but the old slash path isn't valid
                //x sometimes one father makes two child slash paths (father was assigned after the new path is created, caused time issue)
                //x the slash doesn't need enemies in it to be valid
                //x enemy boost is causing slash path to jump
                //x new path has no enemy boost
                //x when decreasing boost buffer the new path flashes (this is leviated and can't be solved entirely)
                //x enemy boost not working
            // tune
                //x give extension bonus when reflecting
                //x don't ignore the object that the reflection is happening on when detecting if slash is valid
                //x change slime type enemies' particle effect color
                // make reflection slashes faster
        //? onioner
        // shooter slime
            // make slime an upgrade for enemies
// MAINTENANCE
    // clean up
// JUICE
    //x warm up rotation
    // shake when charging
    // dust when stopping
    // dust when starting slash

// SLASH DASH DEMO V3.0
//! rogue system
    