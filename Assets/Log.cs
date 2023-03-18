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
    // enemy acceleration
        // turn spd
        // slow down when turning
    //x make scores less obvious
// DEBUG
    //x fix restart
// FEATURES
    //! SLINGSHOT
        //x silhouettes
        // charge
    //! stun after finshing slashing
    //! BULLET
        // consume bullet to slash
        // gaining bullet
    //! NEW ENEMY TYPES
        // snaker
        // shilder
        // onioner
// JUICE
    //x warm up rotation
    